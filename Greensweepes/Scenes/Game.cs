using Godot;
using Godot.Collections;
using System;

public partial class Game : Node
{
    public static Game inst;

    [Export] public Menu menu;
    [Export] public IngameUI ingameUI;
    [Export] private Array<float> energyThresholds;

    private float energy;
    private int regrows;

    public override void _EnterTree()
    {
        inst = this;
    }

    public void AddEnergy(float amount)
    {
        if (regrows == energyThresholds.Count)
            return;

        energy += amount;
        float threshold = energyThresholds[regrows];
        ingameUI.SetRegrowProgress(energy / threshold);
        if (energy > threshold && regrows < energyThresholds.Count)
        {
            energy -= threshold;
            regrows++;
            ingameUI.SetRegrowsAmount(regrows);
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
        ingameUI.SetRegrowsAmount(regrows);
    }
}
