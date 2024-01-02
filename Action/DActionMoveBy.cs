namespace Dragon;

public class DActionMoveBy : DAction
{
    protected Vector2 delta;
    protected Vector2 speed;

    internal DActionMoveBy(Vector2 delta, float duration) : base(duration)
    {
        this.delta = delta;
        speed = delta / this.duration;
    }

    internal override DAction copy()
    {
        return new DActionMoveBy(delta, duration).with(timingFunction);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;

        node.position += speed * (t1 - t0);

        t0 = t1;
    }
}
