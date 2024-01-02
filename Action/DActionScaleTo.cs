namespace Dragon;

public class DActionScaleTo : DActionScaleBy
{
    internal DActionScaleTo(Vector2 scale, float duration) : base(scale, duration)
    {
    }

    internal DActionScaleTo(float scale, float duration) : base(new Vector2(scale, scale), duration)
    {
    }

    internal override DAction copy()
    {
        return new DActionScaleTo(scale, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        speed = (scale - node.scale) / duration;
    }
}
