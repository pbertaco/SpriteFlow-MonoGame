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


    public override void runOnNode(DNode node)
    {
        if (block != null)
        {
            block();
            block = null;
        }
    }
}

