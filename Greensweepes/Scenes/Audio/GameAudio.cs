using Godot;
using System;

public partial class GameAudio : Node
{
    [Export] private AudioStreamPlayer musicPlayer;
    [Export] private AudioStreamPlayer atmoPlayerBleak;
    [Export] private AudioStreamPlayer atmoPlayerLively;

    [ExportGroup("Sounds")]
    [Export] private RandomStreamPlayer cutPlayer;
    [Export] private RandomStreamPlayer revealPlayer;
    [Export] private RandomStreamPlayer markPlayer;
    [Export] private RandomStreamPlayer regrowPlayer;

    private float minAtmoVolume = -20f;
    private float maxAtmoVolume = -5f;

    public enum Sound
    {
        Cut,
        Reveal,
        Mark,
        Regrow
    }

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

    public void PlaySound(Sound sound)
    {
        switch (sound)
        {
            case Sound.Cut:
                cutPlayer.PlayRandom();
                break;
            case Sound.Reveal:
                revealPlayer.PlayRandom();
                break;
            case Sound.Mark:
                markPlayer.PlayRandom();
                break;
            case Sound.Regrow:
                regrowPlayer.PlayRandom();
                break;

        }
    }
}
