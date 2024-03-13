using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class Cells : Node3D
{
    [Export] public int borderOffsetX, borderOffsetY;
    [Export] public float minCutChance, maxCutChance;
    [Export] public int minCutChanceUntil, maxCutChanceFrom;
    [Export] private LevelCreator levelCreator;
    [Export] private Array<Cell> cells = new Array<Cell>();

    private Vector2 fieldSize;
    private int cutsRemaining;
    private int borderLeft, borderRight, borderBottom, borderTop;

    public override void _Ready()
    {
        Init();
    }

    private void Init()
    {
        foreach (Node3D child in GetChildren())
        {
            cells.Add(child as Cell);
        }

        foreach (Cell cell in cells)
        {
            cell.Init(this);
            //GD.Print($"Create cell {x}_{y}");

            if (GetCutChanceAtPosition(cell.idX, cell.idY) > GD.Randf())
            {
                cell.hasCut = true;
                //GD.Print($"{cell.idX}_{cell.idY} has cut");
            }
        }

        UpdateAllNeighbors();
        UpdateAllCuts();

        GetCell(0, 0).PlayHighlight();
    }

    private void UpdateAllNeighbors()
    {
        foreach (Cell cell in cells)
        {
            cell.SetNeighbors(GetNeighbors(cell));
        }
    }

    private void UpdateAllCuts()
    {
        foreach (Cell cell in cells)
        {
            cell.UpdateCuts();
        }
    }

    public Array<Cell> GetNeighbors(Cell cell)
    {
        Array<Cell> neighbors = new Array<Cell>();

        foreach (Cell otherCell in cells)
        {
            int xDistance = Mathf.Abs(cell.idX - otherCell.idX);
            int yDistance = Mathf.Abs(cell.idY - otherCell.idY);
            if ((xDistance == 1 && yDistance == 1) || (xDistance == 1 && yDistance == 0) || (xDistance == 0 && yDistance == 1))
            {
                neighbors.Add(otherCell);
            }
        }

        return neighbors;
    }

    private float GetCutChanceAtPosition(int idX, int idY)
    {
        float cutPercentX = (float)Mathf.Abs(idX) / levelCreator.sizeX;
        float cutChanceX = Mathf.Lerp(minCutChance, maxCutChance, cutPercentX);

        float cutPercentY = (float)Mathf.Abs(idY) / levelCreator.sizeY;
        float cutChanceY = Mathf.Lerp(minCutChance, maxCutChance, cutPercentY);

        float cutChance = Mathf.Max(cutChanceX, cutChanceY);

        // no cuts in initial cells
        if (Mathf.Abs(idX) <= 1 && Mathf.Abs(idY) <= 1)
        {
            cutChance = 0;
        }

        return cutChance;
    }

    private Cell GetCell(int idX, int idY)
    {
        return cells.Where((cell) => cell.idX == idX && cell.idY == idY).FirstOrDefault();
    }

    public void OnCellRevealed(Cell cell)
    {
        Game.inst.OnProgressChanged(GetProgressPercent());
    }

    public void OnMarkerChanged(bool markerAdded)
    {

    }

    public void OnCutClicked(Cell cell)
    {
        Array<Cell> potentialNeighbors = new Array<Cell>();
        foreach (Cell neighbor in cell.neighbors)
        {
            neighbor.SetHidden();
            if (neighbor.CanBecomeCut())
            {
                potentialNeighbors.Add(neighbor);
            }
        }

        if (potentialNeighbors.Count == 0)
        {
            foreach (Cell candidate in cells)
            {
                if (cell.CanBecomeCut() && cell.IsForest())
                {
                    potentialNeighbors.Add(candidate);
                }
            }
        }

        if (potentialNeighbors.Count > 0)
        {
            potentialNeighbors.PickRandom().SetHasCut();
        }

        UpdateAllCuts();
        //GD.Print($"cut clicked! {cell.idX}_{cell.idY}");
    }

    private float GetProgressPercent()
    {
        float forests = 0f;
        foreach (Cell cell in cells)
        {
            if (cell.IsForest())
            {
                forests++;
            }
        }

        return forests / cells.Count;
    }

    public Array<Cell> GetCellsWithState(Cell.State state)
    {
        Array<Cell> stateCells = new Array<Cell>();
        foreach (Cell cell in cells)
        {
            if (cell.state == state)
            {
                stateCells.Add(cell);
            }
        }
        return stateCells;
    }
}
