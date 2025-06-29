namespace Dragon;

public class DTouch
{
    public Vector2 lastPosition;
    public Vector2 position;
    public Vector2 delta;

    public DTouch(Vector2 position)
    {
        this.position = position;
        lastPosition = position;
    }

    public void moved(Vector2 newPosition)
    {
        lastPosition = position;
        position = newPosition;
        delta = lastPosition - position;
    }

    public void up(Vector2 newPosition)
    {
        lastPosition = position;
        position = newPosition;
        delta = lastPosition - position;
    }

    public Vector2 locationIn(DNode node)
    {
        return (position - node.drawPosition).rotateBy(-node.drawRotation) / node.drawScale;
    }

    public override string ToString()
    {
        return $"{base.ToString()}: position: {position}, delta: {delta}";
    }
}
