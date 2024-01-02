namespace Dragon;

public class DActionRunBlock : DAction
{
    Action block;

    internal DActionRunBlock(Action block) : base(0)
    {
        this.block = block;
    }

    internal override DAction copy()
    {
        return new DActionRunBlock(block);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        if (elapsed >= duration)
        {
            block();
        }
    }
}

