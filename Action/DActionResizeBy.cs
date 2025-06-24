namespace Dragon;

public class DActionResizeBy : DAction
{
    protected Vector2 size;
    protected Vector2 speed;

    public DActionResizeBy(Vector2 size, float duration) : base(duration)
    {
        this.size = size;
        speed = size / this.duration;
    }

    public override DAction copy()
    {
        return new DActionResizeBy(size, duration).with(timingFunction);
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        base.evaluateWithNode(node, dt);

        DSpriteNode spriteNode = (DSpriteNode)node;

        if (spriteNode == null)
        {
            return;
        }

        float t1 = timingFunction(elapsed / duration, 0, 1, 1) * duration;
        spriteNode.size += speed * (t1 - t0);
        t0 = t1;
    }
}
