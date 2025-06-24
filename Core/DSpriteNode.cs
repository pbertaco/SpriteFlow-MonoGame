namespace Dragon;

public class DSpriteNode : DNode
{
    public static bool loadFromStream;

    public BlendState blendState;
    protected SpriteEffects spriteEffects;
    protected float layerDepth;

    Color drawColor;

    Color _color;
    public Color color
    {
        get => _color;
        set
        {
            _color = value;
            updateColor();
        }
    }

    float _colorBlendFactor;
    public float colorBlendFactor
    {
        get => _colorBlendFactor;

        set
        {
            _colorBlendFactor = value;
            updateColor();
        }
    }

    Vector2 _origin;
    protected Vector2 origin => _origin;

    Vector2 _textureScale;
    protected Vector2 textureScale => _textureScale;

    Vector2 textureSize => new(_texture?.Width ?? 1, _texture?.Height ?? 1);

    Texture2D _texture;
    public Texture2D texture
    {
        get => _texture;
        set
        {
            _texture = value;
            updateTextureScale();
            updateOrigin();
        }
    }

    Rectangle? _sourceRectangle;
    public Rectangle? sourceRectangle
    {
        get => _sourceRectangle;
        set
        {
            _sourceRectangle = value;
            updateTextureScale();
            updateOrigin();
        }
    }

    Vector2 _size;
    public Vector2 size
    {
        get => _size * scale;
        set
        {
            _size = value / scale;
            updateTextureScale();
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

    public DSpriteNode(Color? color = null, Vector2? size = null)
    {
        Texture2D texture = loadTexture2D();
        load(texture, color, size);
    }

    public DSpriteNode(Vector2 size)
    {
        Texture2D texture = loadTexture2D();
        load(texture, null, size);
    }

    public DSpriteNode(Texture2D texture, Color? color = null, Vector2? size = null)
    {
        load(texture, color, size);
    }

    public DSpriteNode(Texture2D texture, Vector2 size)
    {
        load(texture, null, size);
    }

    public DSpriteNode(string assetName, Color? color = null, Vector2? size = null)
    {
        Texture2D texture = loadTexture2D(assetName);
        load(texture, color, size);
    }

    public DSpriteNode(string assetName, Vector2 size)
    {
        Texture2D texture = loadTexture2D(assetName);
        load(texture, null, size);
    }

    public virtual void load(Texture2D texture, Color? color, Vector2? size)
    {
        _texture = texture;
        position = Vector2.Zero;
        _sourceRectangle = null;
        _color = color ?? Color.White;
        _colorBlendFactor = 1;
        rotation = 0;
        _anchorPoint = new Vector2(0.5f, 0.5f);
        scale = Vector2.One;
        spriteEffects = SpriteEffects.None;
        layerDepth = 0;
        _size = size ?? textureSize;
        updateTextureScale();
        updateOrigin();
        updateColor();
    }

    public override void beforeDraw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        base.beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);

        DGame game = DGame.current;

        if (blendState != game.blendState)
        {
            game.blendState = blendState;
            game.spriteBatch.End();
            game.spriteBatch.Begin(game.sortMode, game.blendState, game.samplerState, game.depthStencilState, game.rasterizerState, game.effect, game.transformMatrix);
        }
    }

    public override void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);
        DGame.current.spriteBatch.Draw(texture, drawPosition, sourceRectangle, drawColor * drawAlpha, drawRotation, origin, drawScale * textureScale, spriteEffects, layerDepth);
        drawChildren();
    }

    public Vector2 calculateScaleToFit(Vector2 size)
    {
        float scale = Math.Min(size.X / _size.X, size.Y / _size.Y);
        return new Vector2(Math.Min(1, scale));
    }

    public Vector2 calculateScaleToFill(Vector2 size)
    {
        float scale = Math.Max(size.X / _size.X, size.Y / _size.Y);
        return new Vector2(Math.Max(1, scale));
    }

    public void setScaleToFit(Vector2 size)
    {
        scale = calculateScaleToFit(size);
    }
    public void setScaleToFill(Vector2 size)
    {
        scale = calculateScaleToFill(size);
    }

    public override void touchDown(DTouch touch)
    {
        if (contains(touch))
        {
            Vector2 touchLocation = touch.locationIn(this);
            DConsole.WriteLine(this, $"{this}: new Vector2({Math.Round(touchLocation.X)}, {Math.Round(touchLocation.Y)})");
        }

        base.touchDown(touch);
    }

    public virtual bool contains(DTouch touch)
    {
        if (parent != null)
        {
            return contains(touch.locationIn(parent).rotateBy(-rotation));
        }

        return false;
    }

    bool contains(Vector2 value)
    {
        Vector2 point = position.rotateBy(-rotation) - (size * anchorPoint);
        return (point.X <= value.X) && (value.X < (point.X + size.X)) && (point.Y <= value.Y) && (value.Y < (point.Y + size.Y));
    }

    void updateTextureScale()
    {
        if (_sourceRectangle != null)
        {
            _textureScale = new Vector2(_size.X / _sourceRectangle.Value.Width, _size.Y / _sourceRectangle.Value.Height);
        }
        else
        {
            _textureScale = _size / textureSize;
        }
    }

    void updateOrigin()
    {
        if (_sourceRectangle != null)
        {
            _origin = new Vector2(_sourceRectangle.Value.Width * _anchorPoint.X, _sourceRectangle.Value.Height * _anchorPoint.Y); ;
        }
        else
        {
            _origin = textureSize * _anchorPoint;
        }
    }

    void updateColor()
    {
        drawColor = Color.Lerp(Color.White, _color, _colorBlendFactor);
    }

    public override string ToString()
    {
        return texture.Name;
    }

    public static Texture2D loadTexture2D(string assetName)
    {
        return loadTexture2D(true, assetName);
    }

    public static Dictionary<string, Texture2D> content = new();

    public static Texture2D loadTexture2D(bool handleException = true, string assetName = "")
    {
        Texture2D texture = null;

        if (assetName == "")
        {
            assetName = "missingTexture2D";
        }

        try
        {
            texture = content.ContainsKey(assetName) ? content[assetName] : null;

            if (texture == null)
            {
                if (loadFromStream)
                {
                    FileStream fileStream = new($"Content/png/{assetName}.png", FileMode.Open);
                    texture = Texture2D.FromStream(DGame.current.GraphicsDevice, fileStream);
                    fileStream.Dispose();
                }
                else
                {
                    texture = DGame.current.Content.Load<Texture2D>($"Texture2D/{assetName}");
                }

                texture.Name = assetName;
                content[assetName] = texture;
                DGame.current.scene.addTexture2D(texture);
            }
        }
        catch (Exception e)
        {
            if (handleException)
            {
                DConsole.WriteLine(e, $"Content.Load<Texture2D> error: {assetName}");
                texture = loadTexture2D(false);
            }
        }

        return texture;
    }
}
