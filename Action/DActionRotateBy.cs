namespace Dragon;

public class DActionRotateBy : DAction
{
    protected float radians;
    protected float speed;

    internal DActionRotateBy(float radians, float duration) : base(duration)
    {
        this.radians = radians;
        speed = radians / this.duration;
    }

    internal override DAction copy()
    {
        return new DActionRotateBy(radians, duration).with(timingFunction);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;

        node.rotation += speed * (t1 - t0);

        t0 = t1;
    }
}
