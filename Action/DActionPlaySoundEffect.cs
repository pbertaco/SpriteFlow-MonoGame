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

    const float PitchVariance = 0.08f;

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
        float randomPitch = 0.0f;

        try
        {
            randomPitch = (DNode.random.NextFloat() * 2f - 1f) * PitchVariance;
        }
        catch
        {
            randomPitch = 0.0f;
        }

        soundEffect?.Play(volume, randomPitch, 0.0f);
        Debug.WriteLine("Playing sound effect" + soundEffect?.Name + $" pitch={randomPitch:0.000}");
    }
}
