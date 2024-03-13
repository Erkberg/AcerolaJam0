using Godot;
using System;

public partial class RandomStreamPlayer : AudioStreamPlayer
{
    [Export] AudioStream[] streams;
    [Export] bool randomizeVolume = true;
    [Export] float minVolume = -1f, maxVolume = 1f;
    [Export] bool randomizePitch = true;
    [Export] float minPitch = 0.9f, maxPitch = 1.1f;

    public void PlayRandom()
    {
        if (streams != null && streams.Length > 0)
        {
            Stream = streams[GD.RandRange(0, streams.Length - 1)];

            if (randomizeVolume)
            {
                VolumeDb = (float)GD.RandRange(minPitch, maxPitch);
            }

            if (randomizePitch)
            {
                PitchScale = (float)GD.RandRange(minPitch, maxPitch);
            }

            Play();
        }
    }
}
