using Godot;
using System;

public partial class Tree : Node3D
{
    [Export] public int idX, idY;
    [Export] private Node3D stump;
    [Export] private Node3D crown;

    private bool isCut;
    private float offset = 0.067f;

    public override void _Ready()
    {

    }

    public void RandomOffset()
    {
        Position += new Vector3((float)GD.RandRange(-offset, offset), 0, (float)GD.RandRange(-offset, offset));
    }

    public void SetCut(bool cut)
    {
        isCut = cut;
        crown.Visible = !cut;
    }
}
