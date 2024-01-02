namespace Dragon;

public class DActionUnhide : DAction
{
    internal DActionUnhide() : base(0)
    {
    }

    internal override DAction copy()
    {
        return new DActionUnhide();
    }

    internal override void runOnNode(DNode node)
    {
        node.hidden = false;
    }
}
