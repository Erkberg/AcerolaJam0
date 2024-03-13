using Godot;
using System;

public partial class World : Node3D
{
    public static World inst;

    [Export] public MainCam mainCam;
    [Export] public Cells cells;
    [Export] private Node3D animalsHolder;
    [Export] private PackedScene animalScene;

    private int totalForests;

    public override void _EnterTree()
    {
        inst = this;
    }

    public Cell GetRandomForestCell()
    {
        return cells.GetCellsWithState(Cell.State.Forest).PickRandom();
    }

    public void OnForestCellRevealed()
    {
        totalForests++;
        if (totalForests % 12 == 0)
        {
            //SpawnAnimal();
        }
    }

    private void SpawnAnimal()
    {
        Animal animal = animalScene.Instantiate() as Animal;
        animalsHolder.AddChild(animal);
        animal.GlobalPosition = Vector3.Zero;
    }
}
