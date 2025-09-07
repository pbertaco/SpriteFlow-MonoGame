namespace Dragon;

public class DActionAnimate : DAction
{
    IEnumerable<Texture2D> textures;
    Texture2D texture;
    float timePerFrame;
    bool resize;
    bool restore;

    public DActionAnimate(IEnumerable<Texture2D> textures, float duration, bool resize, bool restore) : base(duration)
    {
        this.textures = textures;
        this.timePerFrame = duration / textures.Count();
        this.resize = resize;
        this.restore = restore;
    }

    public override DAction copy()
    {
        return new DActionAnimate(textures, duration, resize, restore).with(timingFunction);
    }

    public override void runOnNode(DNode node)
    {
        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        if (restore)
        {
            texture = spriteNode.texture;
        }
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        int index = (int)(elapsed / timePerFrame);

        if (index >= textures.Count())
        {
            index = textures.Count() - 1;
        }

        spriteNode.texture = textures.ElementAt(index);

        if (resize)
        {
            spriteNode.size = spriteNode.texture.size();
        }

        if (elapsed >= duration)
        {
            if (restore)
            {
                spriteNode.texture = texture;
            }

            if (resize)
            {
                spriteNode.size = spriteNode.texture.size();
            }
        }
    }

    public override DAction reversed()
    {
        return new DActionAnimate(textures.Reverse(), duration, resize, restore);
    }
}
