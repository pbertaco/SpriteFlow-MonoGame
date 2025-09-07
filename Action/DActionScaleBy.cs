namespace Dragon;

public class DActionScaleBy : DAction
{
    protected Vector2 scale;
    protected Vector2 speed;

    public DActionScaleBy(Vector2 scale, float duration) : base(duration)
    {
        this.scale = scale;
        speed = scale / this.duration;
    }

    public override DAction copy()
    {
        return new DActionScaleBy(scale, duration).with(timingFunction);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration) * duration;

        node.scale += speed * (t1 - t0);

        t0 = t1;
    }
}
