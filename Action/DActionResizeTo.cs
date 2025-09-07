namespace Dragon;

public class DActionResizeTo : DActionResizeBy
{
    public DActionResizeTo(Vector2 size, float duration) : base(size, duration)
    {
    }

    public override DAction copy()
    {
        return new DActionResizeTo(size, duration).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        speed = (size - spriteNode.size) / duration;
    }
}
