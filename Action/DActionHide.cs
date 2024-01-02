namespace Dragon;

public class DActionHide : DAction
{
    internal DActionHide() : base(0)
    {
    }

    internal override DAction copy()
    {
        return new DActionHide();
    }

    internal override void runOnNode(DNode node)
    {
        node.hidden = true;
    }
}
