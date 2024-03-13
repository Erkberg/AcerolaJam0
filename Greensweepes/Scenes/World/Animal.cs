using Godot;
using System;

public partial class Animal : Node3D
{
    [Export] private Timer idleTimer;

    private Vector3 targetPosition;
    private float moveSpeed;

    public override void _Ready()
    {
        idleTimer.Timeout += NewTargetPosition;
        NewTargetPosition();
        Scale *= (float)GD.RandRange(0.8f, 1.2f);
    }

    public override void _Process(double delta)
    {
        if (idleTimer.IsStopped())
        {
            Move((float)delta);
        }
    }

    private void Move(float delta)
    {
        if (GlobalPosition.DistanceTo(targetPosition) < 0.1f)
        {
            idleTimer.WaitTime = GD.RandRange(1f, 8f);
            idleTimer.Start();
        }
        else
        {
            GlobalPosition = GlobalPosition.MoveToward(targetPosition, delta);
        }
    }

    private void NewTargetPosition()
    {
        Cell targetCell = World.inst.GetRandomForestCell();
        GD.Print(targetCell.Name);
        moveSpeed = (float)GD.RandRange(0.33f, 2f);
        if (targetPosition == targetCell.GlobalPosition)
        {
            idleTimer.Start();
        }
        else
        {
            targetPosition = targetCell.GlobalPosition + new Vector3((float)GD.RandRange(-0.5f, 0.5f), 0f, (float)GD.RandRange(-0.5f, 0.5f));
            LookAt(targetPosition - GlobalPosition);
        }
    }
}
