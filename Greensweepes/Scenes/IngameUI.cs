using Godot;
using Godot.Collections;
using System;

public partial class IngameUI : Control
{
    [Export] private ProgressBar regrowBar;
    [Export] private Array<Control> regrowImages;

    public void SetRegrowsAmount(int amount)
    {
        foreach (Control regrowImage in regrowImages)
        {
            regrowImage.Visible = false;
        }

        for (int i = 0; i < amount; i++)
        {
            regrowImages[i].Visible = true;
        }
    }

    public void SetRegrowProgress(float percent)
    {
        regrowBar.Value = percent;
    }
}
