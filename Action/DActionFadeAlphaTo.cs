namespace Dragon;

public class DActionFadeAlphaTo : DActionFadeAlphaBy
{
    internal DActionFadeAlphaTo(float factor, float duration) : base(factor, duration)
    {
    }

    internal override DAction copy()
    {
        return new DActionFadeAlphaTo(factor, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        speed = (factor - node.alpha) / duration;
    }
}
