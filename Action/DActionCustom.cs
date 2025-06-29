namespace Dragon;

public class DActionCustom : DAction
{
    Action<DNode, float> block;

    public DActionCustom(float duration, Action<DNode, float> block) : base(duration)
    {
        this.block = block;
    }

    public override DAction copy()
    {
        return new DActionCustom(duration, block);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);
        float scale = timingFunction(elapsed / duration);
        block?.Invoke(node, scale);
    }
}