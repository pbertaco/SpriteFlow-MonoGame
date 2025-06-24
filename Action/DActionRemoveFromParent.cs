namespace Dragon;

public class DActionRemoveFromParent : DAction
{
    public DActionRemoveFromParent() : base(0)
    {
    }

    public override DAction copy()
    {
        return new DActionRemoveFromParent();
    }

    public override void runOnNode(DNode node)
    {
        node.removeFromParent(false);
    }
}