namespace Dragon;

public class DActionRunBlock : DAction
{
    Action block;

    public DActionRunBlock(Action block) : base(0)
    {
        this.block = block;
    }

    public override DAction copy()
    {
        return new DActionRunBlock(block);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        if (elapsed >= duration)
        {
            block();
        }
    }
}

