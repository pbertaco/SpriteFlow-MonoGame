namespace Dragon;

public class DActionUnhide : DAction
{
    public DActionUnhide() : base(0)
    {
    }

    public override DAction copy()
    {
        return new DActionUnhide();
    }

    public override void runOnNode(DNode node)
    {
        node.hidden = false;
    }
}
