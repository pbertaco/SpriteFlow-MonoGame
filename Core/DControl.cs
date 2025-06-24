namespace Dragon;

public class DControl : DSpriteNode
{
    public DControl(Color? color = null, Vector2? size = null) : base(color, size)
    {
    }

    public DControl(Vector2 size) : base(size)
    {
    }

    public DControl(Texture2D texture, Color? color = null, Vector2? size = null) : base(texture, color, size)
    {
    }

    public DControl(Texture2D texture, Vector2 size) : base(texture, size)
    {
    }

    public DControl(string assetName, Color? color = null, Vector2? size = null) : base(assetName, color, size)
    {
    }

    public DControl(string assetName, Vector2 size) : base(assetName, size)
    {
    }

    public override void load(Texture2D texture, Color? color, Vector2? size)
    {
        base.load(texture, color, size);
        anchorPoint = new Vector2(0, 0);
    }
}
