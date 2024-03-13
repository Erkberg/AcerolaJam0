using Godot;
using System;

public partial class World : Node3D
{
    public static World inst;

    [Export] public MainCam mainCam;

    public override void _EnterTree()
    {
        inst = this;
    }
}
