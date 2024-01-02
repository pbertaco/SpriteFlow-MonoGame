namespace Dragon;

public class DActionResizeTo : DActionResizeBy
{
    internal DActionResizeTo(Vector2 size, float duration) : base(size, duration)
    {
    }

    internal override DAction copy()
    {
        return new DActionResizeTo(size, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        speed = (size - spriteNode.size) / duration;
    }
}
