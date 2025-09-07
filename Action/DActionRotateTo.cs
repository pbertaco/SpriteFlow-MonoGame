namespace Dragon;

public class DActionRotateTo : DActionRotateBy
{
    public DActionRotateTo(float radians, float duration) : base(radians, duration)
    {
    }

    public override DAction copy()
    {
        return new DActionRotateTo(radians, duration).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        speed = (radians - node.rotation) / duration;
    }
}
