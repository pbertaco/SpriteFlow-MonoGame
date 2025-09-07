namespace Dragon;

public class DScene : DNode
{
    public Vector2 size;
    public Color backgroundColor;

    public Vector2 currentPosition;
    public Vector2 currentScale;

    public bool scissorTestEnable;

    public DScene(Vector2? size = null)
    {
        this.size = size ?? Vector2.One;
        backgroundColor = Color.CornflowerBlue;
        userInteractionEnabled = true;
    }

    public virtual void load()
    {
    }

    public void draw()
    {
        draw(currentPosition, 0, currentScale, 1);
    }

    public void updateSize(DGame game, Vector2 viewSize)
    {
        currentScale = new Vector2(Math.Min(viewSize.X / size.X, viewSize.Y / size.Y));
        currentPosition = ((viewSize / currentScale) - size) * currentScale / 2f;

        if (scissorTestEnable)
        {
            Rectangle rectangle = new((int)currentPosition.X, (int)currentPosition.Y, (int)(size.X * currentScale.X), (int)(size.Y * currentScale.Y));
            game.graphicsDeviceManager.GraphicsDevice.ScissorRectangle = rectangle;
        }
    }

    public virtual void presentScene(DScene scene)
    {
        DGame.current.presentScene(scene);
    }

    public virtual void addTexture2D(Texture2D texture)
    {
    }
}
