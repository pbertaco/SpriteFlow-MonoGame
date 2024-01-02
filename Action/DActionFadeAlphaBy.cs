namespace Dragon;

public class DActionFadeAlphaBy : DAction
{
    protected float factor;
    protected float speed;

    internal DActionFadeAlphaBy(float factor, float duration) : base(duration)
    {
        this.factor = factor;
        speed = factor / this.duration;
    }

    internal override DAction copy()
    {
        return new DActionFadeAlphaBy(factor, duration).with(timingFunction);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;

        node.alpha += speed * (t1 - t0);

        t0 = t1;
    }
}
