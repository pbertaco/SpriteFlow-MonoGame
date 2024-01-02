namespace Dragon;

public class DActionColorize : DAction
{
    Color color;
    float colorBlendFactor;
    Color startColor;

    public DActionColorize(Color color, float colorBlendFactor, float duration) : base(duration)
    {
        this.color = color;
        this.colorBlendFactor = colorBlendFactor;
    }

    internal override DAction copy()
    {
        return new DActionColorize(color, colorBlendFactor, duration).with(timingFunction);
    }

    internal override void runOnNode(DNode node)
    {
        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        startColor = spriteNode.color;
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        spriteNode.color = Color.Lerp(startColor, color, elapsed / duration);
    }
}
