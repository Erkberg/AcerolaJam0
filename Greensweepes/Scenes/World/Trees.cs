using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Trees : Node3D
{
    [Export] private Array<Tree> trees;

    public int cutAmount;

    public override void _Ready()
    {
        foreach (Tree tree in trees)
        {
            tree.Visible = false;
            tree.RandomOffset();
        }

        trees.Shuffle();
    }

    public void SetHidden()
    {
        foreach (Tree tree in trees)
        {
            tree.Visible = false;
        }
    }

    public void SetCutAmount(int cutAmount)
    {
        this.cutAmount = cutAmount;

        foreach (Tree tree in trees)
        {
            tree.Visible = true;
            tree.SetCut(false);
        }

        for (int i = 0; i < cutAmount; i++)
        {
            trees[i].SetCut(true);
        }
    }

    private Tree GetTree(int idX, int idY)
    {
        return trees.Where((tree) => tree.idX == idX && tree.idY == idY).FirstOrDefault();
    }
}
