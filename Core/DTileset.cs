namespace Dragon;

public class DTileset : DSpriteNode
{
    Vector2? tileSize;

    internal DTileset(Texture2D texture, Color color, Vector2 size) : base(texture, color, size)
    {
    }

    internal DTileset(Texture2D texture, Vector2 size) : base(texture, size)
    {
    }

    internal DTileset(string assetName, Color color, Vector2 size) : base(assetName, color, size)
    {
    }

    internal DTileset(string assetName, Vector2 size) : base(assetName, size)
    {
    }

    internal override void load(Texture2D texture, Color? color, Vector2? size)
    {
        tileSize = size;
        base.load(texture, color, null);

        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {

            }
        }
    }
}
