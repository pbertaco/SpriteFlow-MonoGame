namespace Dragon;

public class DActionScaleBy : DAction
{
    protected Vector2 scale;
    protected Vector2 speed;

    internal DActionScaleBy(Vector2 scale, float duration) : base(duration)
    {
        this.scale = scale;
        speed = scale / this.duration;
    }

    internal override DAction copy()
    {
        return new DActionScaleBy(scale, duration).with(timingFunction);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;

        node.scale += speed * (t1 - t0);

        t0 = t1;
    }
}
