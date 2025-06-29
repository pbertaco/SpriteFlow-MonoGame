namespace Dragon;

public class DActionMoveBy : DAction
{
    protected Vector2 delta;
    protected Vector2 speed;

    public DActionMoveBy(Vector2 delta, float duration) : base(duration)
    {
        this.delta = delta;
        speed = delta / this.duration;
    }

    public override DAction copy()
    {
        return new DActionMoveBy(delta, duration).with(timingFunction);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration) * duration;

        node.position += speed * (t1 - t0);

        t0 = t1;
    }
}
