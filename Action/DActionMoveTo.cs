namespace Dragon;

public class DActionMoveTo : DActionMoveBy
{
    Vector2 position;

    public DActionMoveTo(Vector2 position, float duration) : base(position, duration)
    {
        this.position = position;
    }

    public override DAction copy()
    {
        return new DActionMoveTo(position, duration).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        speed = (position - node.position) / duration;
    }
}
