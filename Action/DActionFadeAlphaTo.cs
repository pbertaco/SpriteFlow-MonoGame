namespace Dragon;

public class DActionFadeAlphaTo : DActionFadeAlphaBy
{
    public DActionFadeAlphaTo(float factor, float duration) : base(factor, duration)
    {
    }

    public override DAction copy()
    {
        return new DActionFadeAlphaTo(factor, duration).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        speed = (factor - node.alpha) / duration;
    }
}
