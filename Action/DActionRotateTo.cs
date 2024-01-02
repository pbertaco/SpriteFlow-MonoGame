namespace Dragon;

public class DActionRotateTo : DActionRotateBy
{
    internal DActionRotateTo(float radians, float duration) : base(radians, duration)
    {
    }

    internal override DAction copy()
    {
        return new DActionRotateTo(radians, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        speed = (radians - node.rotation) / duration;
    }
}
