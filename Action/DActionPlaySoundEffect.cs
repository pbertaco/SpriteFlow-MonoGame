namespace Dragon;

public class DActionPlaySoundEffect : DAction
{
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
        soundEffect?.Play();
        Console.WriteLine("Playing sound effect" + soundEffect?.Name);
    }
}
