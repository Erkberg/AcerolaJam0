using Godot;
using Godot.Collections;
using System;

public partial class Cell : Area3D
{
    [ExportGroup("Refs")]
    [Export] public Label3D numberText;
    [Export] public MeshInstance3D groundMesh;
    [Export] public Trees trees;
    [Export] public AnimationPlayer animPlayer;
    [Export] private Timer energyTimer;
    [Export] private PackedScene energyParticleScene;
    [Export] private Sprite3D regrowSprite;
    [Export] public Array<Cell> neighbors;

    [ExportGroup("Materials")]
    [Export] private Material hiddenMat;
    [Export] private Material highlightedMat;
    [Export] private Material forestMat;
    [Export] private Material markedMat;
    [Export] private Material cutMat;

    // State
    public State state;
    public bool hasCut = false;
    public int neighborCuts = 0;
    public int idX, idY;

    private Cells cells;
    private bool mouseOver;

    private const string CellName = "Cell";

    public enum State
    {
        Hidden,
        Highlighted,
        Forest,
        Marked,
        Cut,
        SuperTree
    }

    public override void _MouseEnter()
    {
        mouseOver = true;

        if (state == State.Hidden)
        {
            ChangeState(State.Highlighted);
        }

        if (CanRegrow())
        {
            regrowSprite.Visible = true;
        }
    }

    public override void _MouseExit()
    {
        mouseOver = false;

        if (state == State.Highlighted)
        {
            ChangeState(State.Hidden);
        }

        regrowSprite.Visible = false;
    }

    public void Init(Cells cells)
    {
        this.cells = cells;
        idX = Mathf.RoundToInt(GlobalPosition.X);
        idY = Mathf.RoundToInt(GlobalPosition.Z);
        Name = $"{CellName}_{idX}_{idY}";

        energyTimer.Timeout += OnEnergyTimer;
        RandomizeTimer();
        energyTimer.Start();
        Game.inst.ui.menu.showNumbersToggled += OnNumberDisplayChanged;
    }

    public void SetNeighbors(Array<Cell> neighbors)
    {
        this.neighbors = neighbors;
    }

    public void UpdateCuts()
    {
        neighborCuts = 0;

        foreach (Cell neighbor in neighbors)
        {
            if (neighbor.hasCut)
                neighborCuts++;
        }

        if (state == State.Forest)
        {
            trees.SetCutAmount(neighborCuts);
            numberText.Text = neighborCuts > 0 ? neighborCuts.ToString() : string.Empty;
        }
    }

    public override void _Process(double delta)
    {
        if (mouseOver)
        {
            if (Input.IsActionJustPressed(Inputs.LeftMouseButton))
            {
                OnLeftClick();
            }

            if (Input.IsActionJustPressed(Inputs.RightMouseButton))
            {
                OnRightClick();
            }
        }
    }

    public void OnLeftClick()
    {
        if (CanRegrow())
        {
            Game.inst.OnRegrowUsed();
            hasCut = !hasCut;
            ChangeState(State.Hidden);
            Reveal();
            regrowSprite.Visible = false;
            foreach (Cell neighbor in neighbors)
            {
                neighbor.UpdateCuts();
            }

            Game.inst.audio.PlaySound(hasCut ? GameAudio.Sound.Cut : GameAudio.Sound.Regrow);
        }
        else if (!IsMarked())
        {
            Reveal();
            Game.inst.audio.PlaySound(hasCut ? GameAudio.Sound.Cut : GameAudio.Sound.Reveal);
        }
    }

    public void OnRightClick()
    {
        if (!IsRevealed())
        {
            Game.inst.audio.PlaySound(GameAudio.Sound.Mark);
            ChangeState(IsMarked() ? State.Highlighted : State.Marked);
            cells.OnMarkerChanged(IsMarked());
        }
    }

    public void Reveal()
    {
        if (IsRevealed())
            return;

        if (hasCut)
        {
            RevealCut();
        }
        else
        {
            ChangeState(State.Forest);
            trees.SetCutAmount(neighborCuts);
            if (neighborCuts > 0)
            {
                numberText.Text = neighborCuts.ToString();
            }
            else
            {
                foreach (Cell neighbor in neighbors)
                {
                    neighbor.Reveal();
                }
            }
        }

        StopHighlight();

        cells.OnCellRevealed(this);
    }

    private void RevealCut()
    {
        GD.Print("reveal cut");
        World.inst.mainCam.StartShake();
        ChangeState(State.Cut);
        cells.OnCutClicked(this);
        trees.SetCutAmount(8);
    }

    private void Regrow()
    {

    }

    public bool IsRevealed()
    {
        return state == State.Forest || state == State.Cut;
    }

    public bool IsMarked()
    {
        return state == State.Marked;
    }

    private void ChangeState(State newState)
    {
        state = newState;
        regrowSprite.Visible = false;

        switch (state)
        {
            case State.Hidden:
                numberText.Text = string.Empty;
                groundMesh.MaterialOverride = hiddenMat;
                trees.SetHidden();
                break;
            case State.Highlighted:
                groundMesh.MaterialOverride = highlightedMat;
                break;
            case State.Forest:
                groundMesh.MaterialOverride = forestMat;
                break;
            case State.Marked:
                groundMesh.MaterialOverride = markedMat;
                if (CanRegrow())
                {
                    regrowSprite.Visible = true;
                }
                break;
            case State.Cut:
                groundMesh.MaterialOverride = cutMat;
                break;
            case State.SuperTree:
                groundMesh.MaterialOverride = forestMat;
                break;
        }
    }

    public void PlayHighlight()
    {
        animPlayer.Play("Highlight");
    }

    private void StopHighlight()
    {
        animPlayer.Stop();
    }

    public bool CanBecomeCut()
    {
        return !hasCut && state != State.SuperTree;
    }

    public bool IsForest()
    {
        return state == State.Forest;
    }

    public void SetHasCut()
    {
        hasCut = true;
        ChangeState(State.Hidden);
    }

    private void OnEnergyTimer()
    {
        if (IsForest())
        {
            Game.inst.AddEnergy(1);
            Node3D energyParticle = energyParticleScene.Instantiate() as Node3D;
            AddChild(energyParticle);
            energyParticle.GlobalPosition = GlobalPosition;
            RandomizeTimer();
        }
    }

    private void RandomizeTimer()
    {
        energyTimer.WaitTime = GD.RandRange(8f, 16f);
        if (trees.cutAmount > 0)
        {
            energyTimer.WaitTime *= trees.cutAmount;
        }
    }

    private bool CanRegrow()
    {
        return Game.inst.RegrowAvailable() && (state == State.Marked || state == State.Cut);
    }

    public void SetHidden()
    {
        ChangeState(State.Hidden);
    }

    private void OnNumberDisplayChanged(bool visible)
    {
        numberText.Visible = visible;
    }
}
