using Godot;
using System;

[Tool]
public partial class LevelCreator : Node
{
    [Export] public PackedScene cellScene;
    [Export] public int sizeX, sizeY;
    [Export] private Node3D cellsHolder;
    [Export] private bool create;
    [Export] private bool clear;

    private const int CellSize = 1;

    public override void _Process(double delta)
    {
        if (create)
        {
            create = false;
            CreateCells();
        }

        if (clear)
        {
            clear = false;
            ClearCells();
        }
    }

    public void CreateCells()
    {
        CreateCells(-sizeX, sizeX, -sizeY, sizeY);
    }

    public void CreateCells(int newBorderLeft, int newBorderRight, int newBorderBottom, int newBorderTop)
    {
        GD.Print($"create cells from {newBorderLeft}_{newBorderBottom} to {newBorderRight}_{newBorderTop}");
        for (int x = newBorderLeft; x < newBorderRight; x++)
        {
            for (int y = newBorderBottom; y < newBorderTop; y++)
            {
                CreateCell(x, y);
            }
        }
    }

    private void CreateCell(int x, int y)
    {
        Node3D cell = cellScene.Instantiate() as Node3D;
        cellsHolder.AddChild(cell);
        cell.Owner = GetTree().EditedSceneRoot;
        cell.Position = new Vector3(x * CellSize, 0f, y * CellSize);
    }

    private void ClearCells()
    {
        foreach (Node child in cellsHolder.GetChildren())
        {
            child.QueueFree();
        }
    }
}
