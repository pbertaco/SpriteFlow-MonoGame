namespace Dragon;

public class DLabelNode : DNode
{
    public static SpriteFont defaultSpriteFont;

    Color color;
    SpriteEffects spriteEffects;
    float layerDepth;

    Vector2 _origin;
    Vector2 origin => _origin;

    float _spriteFontScale;
    float spriteFontScale => _spriteFontScale;

    Vector2 textureSize => spriteFont.MeasureString(text);

    string _text;
    public string text
    {
        get => _text;
        set
        {
            _text = value;
            updateOrigin();
        }
    }

    SpriteFont _spriteFont;
    public SpriteFont spriteFont
    {
        get => _spriteFont;
        set
        {
            _spriteFont = value;
            updateSpriteFontScale();
            updateOrigin();
        }
    }

    float _fontSize;
    public float fontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            updateSpriteFontScale();
        }
    }

    Vector2 _anchorPoint;
    public Vector2 anchorPoint
    {
        get => _anchorPoint;
        set
        {
            _anchorPoint = value;
            updateOrigin();
        }
    }

    public DLabelNode(string text, SpriteFont font, Color? color = null, float? fontSize = null)
    {
        load(font, fontSize, color, text);
    }

    public DLabelNode(string text, string assetName, Color? color = null, float? fontSize = null)
    {
        SpriteFont font = loadSpriteFont(assetName);
        load(font, fontSize, color, text);
    }

    public DLabelNode(string text, SpriteFont font, float fontSize)
    {
        load(font, fontSize, null, text);
    }

    public DLabelNode(string text, string assetName, float fontSize)
    {
        SpriteFont font = loadSpriteFont(assetName);
        load(font, fontSize, null, text);
    }

    public DLabelNode(string text, Color? color = null, float? fontSize = null)
    {
        SpriteFont font = defaultSpriteFont ?? loadSpriteFont();
        load(font, fontSize, color, text);
    }

    public DLabelNode(string text, float fontSize)
    {
        SpriteFont font = defaultSpriteFont ?? loadSpriteFont();
        load(font, fontSize, null, text);
    }

    void load(SpriteFont font, float? fontSize, Color? color, string text)
    {
        _spriteFont = font;
        _text = text;
        position = Vector2.Zero;
        this.color = color ?? Color.White;
        rotation = 0;
        _anchorPoint = new Vector2(0.5f, 0.5f);
        scale = Vector2.One;
        spriteEffects = SpriteEffects.None;
        layerDepth = 0;
        _fontSize = fontSize ?? spriteFont.LineSpacing;
        updateSpriteFontScale();
        updateOrigin();
    }

    public override void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);
        DGame.current.spriteBatch.DrawString(spriteFont, text, drawPosition, color * drawAlpha, drawRotation, origin, drawScale * spriteFontScale, spriteEffects, layerDepth);
        drawChildren();
    }

    void updateSpriteFontScale()
    {
        _spriteFontScale = _fontSize / (_spriteFont != null ? _spriteFont.LineSpacing : 1);
    }

    void updateOrigin()
    {
        _origin = textureSize * _anchorPoint;
    }

    public static SpriteFont loadSpriteFont(string assetName = "missingSpriteFont", bool handleException = true)
    {
        SpriteFont spriteFont = null;

        try
        {
            spriteFont = DGame.current.Content.Load<SpriteFont>($"SpriteFont/{assetName}");
        }
        catch (Exception)
        {
            if (handleException)
            {
                spriteFont = loadSpriteFont(null, false);
            }
        }

        return spriteFont;
    }
}
