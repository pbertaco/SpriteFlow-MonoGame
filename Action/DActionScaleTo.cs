namespace Dragon;

public class DActionScaleTo : DActionScaleBy
{
    public DActionScaleTo(Vector2 scale, float duration) : base(scale, duration)
    {
    }

    public DActionScaleTo(float scale, float duration) : base(new Vector2(scale, scale), duration)
    {
    }

    public override DAction copy()
    {
        return new DActionScaleTo(scale, duration).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        speed = (scale - node.scale) / duration;
    }
}
