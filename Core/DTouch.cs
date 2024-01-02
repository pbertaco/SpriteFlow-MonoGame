namespace Dragon;

public class DTouch
{
    internal Vector2 lastPosition;
    internal Vector2 position;
    internal Vector2 delta;

    internal DTouch(Vector2 position)
    {
        this.position = position;
        lastPosition = position;
        Console.WriteLine(position);
    }

    internal void moved(Vector2 newPosition)
    {
        lastPosition = position;
        position = newPosition;
        delta = lastPosition - position;
        Console.WriteLine(position);
    }

    internal void up(Vector2 newPosition)
    {
        lastPosition = position;
        position = newPosition;
        delta = lastPosition - position;
        Console.WriteLine(position);
    }

    internal Vector2 locationIn(DNode node)
    {
        return (position - node.drawPosition).rotateBy(-node.drawRotation) / node.drawScale;
    }

    public override string ToString()
    {
        return $"{base.ToString()}: position: {position}, delta: {delta}";
    }
}
