namespace Dragon;

public class DActionWait : DAction
{
    static Random random = new Random();

    float durationBase;
    float durationRange;

    internal DActionWait(float durationBase, float durationRange = 0) : base(0)
    {
        this.durationBase = durationBase;
        this.durationRange = durationRange;

        float randomDuration = random.NextFloat() * durationRange;
        duration = this.durationBase - (durationRange / 2) + randomDuration;

        if (duration <= 0)
        {
            duration = 0.001f;
        }
    }

    internal override DAction copy()
    {
        return new DActionWait(durationBase, durationRange);
    }
}
