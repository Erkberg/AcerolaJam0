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
        bool scaleUp = isCut && !cut;

        isCut = cut;
        crown.Visible = !cut;

        if (scaleUp)
        {
            crown.Scale = Vector3.One * 0.01f;
            Tween tween = CreateTween();
            tween.TweenProperty(crown, "scale", Vector3.One, 1f);
        }
    }
}
