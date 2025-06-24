namespace Dragon;

public class DActionFadeAlphaBy : DAction
{
    protected float factor;
    protected float speed;

    public DActionFadeAlphaBy(float factor, float duration) : base(duration)
    {
        this.factor = factor;
        speed = factor / this.duration;
    }

    public override DAction copy()
    {
        return new DActionFadeAlphaBy(factor, duration).with(timingFunction);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;

        node.alpha += speed * (t1 - t0);

        t0 = t1;
    }
}
