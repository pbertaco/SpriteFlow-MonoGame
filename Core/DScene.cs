namespace Dragon;

public class DScene : DNode
{
    internal Vector2 size;
    internal Color backgroundColor;

    internal Vector2 currentPosition;
    internal Vector2 currentScale;

    internal bool scissorTestEnable;

    internal DScene(Vector2? size = null) : base()
    {
        this.size = size ?? new Vector2(960, 540);
        backgroundColor = Color.CornflowerBlue;
        userInteractionEnabled = true;
    }

    internal virtual void load()
    {
    }

    internal void draw()
    {
        draw(currentPosition, 0, currentScale, 1);
    }

    internal void updateSize(DGame game, Vector2 viewSize)
    {
        currentScale = new Vector2(Math.Min(viewSize.X / size.X, viewSize.Y / size.Y));
        currentPosition = ((viewSize / currentScale) - size) * currentScale / 2f;

        if (scissorTestEnable)
        {
            Rectangle rectangle = new((int)currentPosition.X, (int)currentPosition.Y, (int)(size.X * currentScale.X), (int)(size.Y * currentScale.Y));
            game.graphicsDeviceManager.GraphicsDevice.ScissorRectangle = rectangle;
        }
    }

    internal virtual void presentScene(DScene scene)
    {
        DGame.current.presentScene(scene);
    }

    internal virtual void addTexture2D(Texture2D texture)
    {
    }
}
