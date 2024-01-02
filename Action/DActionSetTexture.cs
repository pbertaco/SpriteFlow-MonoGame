namespace Dragon;

public class DActionSetTexture : DAction
{
    Texture2D texture;
    bool resize;

    internal DActionSetTexture(Texture2D texture, bool resize) : base(0)
    {
        this.texture = texture;
        this.resize = resize;
    }

    internal override DAction copy()
    {
        return new DActionSetTexture(texture, resize);
    }

    internal override void runOnNode(DNode node)
    {
        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        if (resize)
        {
            spriteNode.size = new Vector2(texture.Width, texture.Height);
        }

        spriteNode.texture = texture;
    }
}
