namespace Dragon;

public class DActionMoveTo : DActionMoveBy
{
    Vector2 position;

    internal DActionMoveTo(Vector2 position, float duration) : base(position, duration)
    {
        this.position = position;
    }

    internal override DAction copy()
    {
        return new DActionMoveTo(position, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        speed = (position - node.position) / duration;
    }
}
