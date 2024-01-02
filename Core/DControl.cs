namespace Dragon;

public class DControl : DSpriteNode
{
    internal DControl(Color? color = null, Vector2? size = null) : base(color, size)
    {
    }

    internal DControl(Vector2 size) : base(size)
    {
    }

    internal DControl(Texture2D texture, Color? color = null, Vector2? size = null) : base(texture, color, size)
    {
    }

    internal DControl(Texture2D texture, Vector2 size) : base(texture, size)
    {
    }

    internal DControl(string assetName, Color? color = null, Vector2? size = null) : base(assetName, color, size)
    {
    }

    internal DControl(string assetName, Vector2 size) : base(assetName, size)
    {
    }

    internal override void load(Texture2D texture, Color? color, Vector2? size)
    {
        base.load(texture, color, size);
        anchorPoint = new Vector2(0, 0);
    }
}
