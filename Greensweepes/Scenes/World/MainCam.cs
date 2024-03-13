using Godot;
using System;

public partial class MainCam : Camera3D
{
    [Export] private Node3D holder;
    [Export] private float minY, maxY;
    [Export] private float maxX, maxZ;
    [Export] private float zoomStep = 1;
    [Export] private float moveSpeed = 8;
    [Export] private Vector2 moveStartOffset = new Vector2(160, 90);


    private Vector3 targetPosition;
    private Viewport viewport;
    private Vector2 viewportSize;
    private Vector3 holderPosition;
    private float shakeStrength;

    public override void _Ready()
    {
        targetPosition = GlobalPosition;
        viewport = GetViewport();
        viewportSize = viewport.GetVisibleRect().Size;
        holderPosition = holder.Position;
    }

    public override void _Process(double delta)
    {
        if (shakeStrength > 0f)
        {
            Shake((float)delta);
            return;
        }

        CheckZoom();
        CheckMove((float)delta);
        ClampTargetPosition();

        Move((float)delta);
    }

    private void Shake(float delta)
    {
        holder.Position = holderPosition + new Vector3((float)GD.RandRange(-shakeStrength, shakeStrength), (float)GD.RandRange(-shakeStrength, shakeStrength), (float)GD.RandRange(-shakeStrength, shakeStrength));
        shakeStrength -= delta;

        if (shakeStrength <= 0f)
        {
            shakeStrength = 0f;
            holder.Position = holderPosition;
        }
    }

    private void Move(float delta)
    {
        GlobalPosition = GlobalPosition.MoveToward(targetPosition, moveSpeed * GlobalPosition.DistanceTo(targetPosition) * delta);
    }

    private void CheckMove(float delta)
    {
        Vector2 mousePos = viewport.GetMousePosition();
        Vector3 moveOffset = Vector3.Zero;

        if (mousePos.X < moveStartOffset.X)
        {
            float distance = mousePos.X;
            moveOffset.X = -1 + (distance / moveStartOffset.X);
        }
        else if (mousePos.X > viewportSize.X - moveStartOffset.X)
        {
            float distance = viewportSize.X - mousePos.X;
            moveOffset.X = 1 - (distance / moveStartOffset.X);
        }

        if (mousePos.Y < moveStartOffset.Y)
        {
            float distance = mousePos.Y;
            moveOffset.Z = -1 + (distance / moveStartOffset.Y);
        }
        else if (mousePos.Y > viewportSize.Y - moveStartOffset.Y)
        {
            float distance = viewportSize.Y - mousePos.Y;
            moveOffset.Z = 1 - (distance / moveStartOffset.Y);
        }

        targetPosition += moveOffset * moveSpeed * delta;
    }

    private void CheckZoom()
    {
        if (Input.IsActionJustPressed(Inputs.ZoomIn) && targetPosition.Y > minY)
        {
            targetPosition.Y -= zoomStep;
        }

        if (Input.IsActionJustPressed(Inputs.ZoomOut) && targetPosition.Y < maxY)
        {
            targetPosition.Y += zoomStep;
        }
    }

    private void ClampTargetPosition()
    {
        float zoomOffset = GetZoomOffset();
        float zoomedMaxX = maxX - zoomOffset * 1.2f;
        float zoomedMaxZ = maxZ - zoomOffset * 0.7f;
        targetPosition.X = Mathf.Clamp(targetPosition.X, -zoomedMaxX, zoomedMaxX);
        targetPosition.Z = Mathf.Clamp(targetPosition.Z, -zoomedMaxZ, zoomedMaxZ);
    }

    private float GetZoomOffset()
    {
        return targetPosition.Y - minY;
    }

    public void StartShake()
    {
        shakeStrength = 0.33f;
    }
}
