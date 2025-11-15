using SharpDX.Direct3D9;

namespace Dragon;

public class DActionPlaySoundEffect : DAction
{
    static float _volume;
    static float volumeScale = 0.1f;
    public static float volume
    {
        get => _volume / volumeScale;
        set
        {
            _volume = MathHelper.Clamp(value, 0f, 1f) * volumeScale;
        }
    }

    SoundEffect soundEffect;

    public DActionPlaySoundEffect(SoundEffect soundEffect, bool waitForCompletion) : base(0)
    {
        this.soundEffect = soundEffect;

        if (waitForCompletion)
        {
            duration = (float)soundEffect.Duration.TotalSeconds;
        }
    }

    public override void runOnNode(DNode node)
    {
        soundEffect?.Play(volume, 0.0f, 0.0f);
        Console.WriteLine("Playing sound effect" + soundEffect?.Name);
    }
}
