using Godot;
using System;

public partial class GameAudio : Node
{
    [Export] private AudioStreamPlayer musicPlayer;
    [Export] private AudioStreamPlayer atmoPlayerBleak;
    [Export] private AudioStreamPlayer atmoPlayerLively;

    private float minAtmoVolume = -24f;
    private float maxAtmoVolume = -8f;

    public override void _Ready()
    {
        atmoPlayerBleak.VolumeDb = maxAtmoVolume;
        atmoPlayerLively.VolumeDb = minAtmoVolume;
        //OnCompletionChanged(1f);
    }

    public void OnCompletionChanged(float percentCompleted)
    {
        atmoPlayerBleak.VolumeDb = Mathf.Lerp(maxAtmoVolume, minAtmoVolume, percentCompleted);
        atmoPlayerLively.VolumeDb = Mathf.Lerp(minAtmoVolume, maxAtmoVolume, percentCompleted);
        //GD.Print(atmoPlayerBleak.VolumeDb + " - " + atmoPlayerLively.VolumeDb);
    }
}
