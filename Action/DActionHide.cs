namespace Dragon;

public class DActionHide : DAction
{
    public DActionHide() : base(0)
    {
    }

    public override DAction copy()
    {
        return new DActionHide();
    }

    public override void runOnNode(DNode node)
    {
        node.hidden = true;
    }
}
