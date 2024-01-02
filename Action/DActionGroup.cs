namespace Dragon;

public class DActionGroup : DAction
{
    protected List<DAction> actions;

    internal DActionGroup(IEnumerable<DAction> actions) : base(0)
    {
        this.actions = new List<DAction>();
        foreach (DAction action in actions)
        {
            this.actions.Add(action.copy());
        }

        foreach (DAction action in actions)
        {
            duration = Math.Max(duration, action.duration);
        }
    }

    internal override DAction copy()
    {
        return new DActionGroup(actions);
    }

    internal override void runOnNode(DNode node)
    {
        foreach (DAction action in actions)
        {
            action.runOnNode(node);
        }
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        elapsed += dt;

        foreach (DAction action in actions)
        {
            action.evaluateWithNode(node, dt);
        }
    }
}
