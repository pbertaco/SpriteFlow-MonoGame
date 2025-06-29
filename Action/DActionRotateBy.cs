namespace Dragon;

public class DActionRotateBy : DAction
{
    protected float radians;
    protected float speed;

    public DActionRotateBy(float radians, float duration) : base(duration)
    {
        this.radians = radians;
        speed = radians / this.duration;
    }

    public override DAction copy()
    {
        return new DActionRotateBy(radians, duration).with(timingFunction);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration) * duration;

        node.rotation += speed * (t1 - t0);

        t0 = t1;
    }
}
