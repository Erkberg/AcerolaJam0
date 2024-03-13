using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
    public static Game inst;

    [Export] public GameUI ui;
    [Export] public GameAudio audio;
    [Export] private Array<float> energyThresholds;

    private float energy;
    private int regrows;

    public override void _EnterTree()
    {
        inst = this;
    }

    public void OnProgressChanged(float percent)
    {
        audio.OnCompletionChanged(percent);
    }

    public void AddEnergy(float amount)
    {
        if (regrows == energyThresholds.Count)
            return;

        energy += amount;
        float threshold = energyThresholds[regrows];
        ui.ingameUI.SetRegrowProgress(energy / threshold);
        if (energy > threshold && regrows < energyThresholds.Count)
        {
            energy -= threshold;
            regrows++;
            ui.ingameUI.SetRegrowsAmount(regrows);
        }
    }

    public bool RegrowAvailable()
    {
        return regrows > 0;
    }

    public bool MarkerRemovalAvailable()
    {
        return regrows > 0;
    }

    public bool SuperTreeAvailable()
    {
        return regrows > 1;
    }

    public bool ReforestationAvailable()
    {
        return regrows > 2;
    }

    public void OnRegrowUsed(int amount = 1)
    {
        regrows -= amount;
        ui.ingameUI.SetRegrowsAmount(regrows);
    }
}
