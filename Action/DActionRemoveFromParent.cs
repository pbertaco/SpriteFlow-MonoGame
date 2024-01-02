namespace Dragon;

public class DActionRemoveFromParent : DAction
{
    internal DActionRemoveFromParent() : base(0)
    {
    }

    internal override DAction copy()
    {
        return new DActionRemoveFromParent();
    }

    internal override void runOnNode(DNode node)
    {
        node.removeFromParent(false);
    }
}