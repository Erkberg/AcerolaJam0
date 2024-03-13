using Godot;
using System;

public partial class Menu : Control
{
    public Action<bool> showNumbersToggled;

    [Export] private CheckBox showNumbersCheckbox;
    [Export] private Button playButton;
    [Export] private Button quitButton;

    public override void _Ready()
    {
        showNumbersCheckbox.Toggled += OnShowNumbersToggled;
        playButton.Pressed += Close;
        quitButton.Pressed += () => GetTree().Quit();

        Open();
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed(Inputs.Menu))
        {
            ToggleOpen();
        }
    }

    private void ToggleOpen()
    {
        if (Visible)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        GetTree().Paused = true;
        Visible = true;
    }

    private void Close()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    public bool IsOpen()
    {
        return Visible;
    }

    private void OnShowNumbersToggled(bool toggledOn)
    {
        showNumbersToggled?.Invoke(toggledOn);
    }
}
