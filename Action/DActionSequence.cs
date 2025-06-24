namespace Dragon;

public class DActionSequence : DActionGroup
{
    IEnumerator<DAction> enumerator;

    public DActionSequence(IEnumerable<DAction> actions) : base(actions)
    {
        duration = 0;

        foreach (DAction action in actions)
        {
            duration += action.duration;
        }
    }

    public override DAction copy()
    {
        return new DActionSequence(actions);
    }

    public override void runOnNode(DNode node)
    {
        enumerator = actions.GetEnumerator();
        if (enumerator.MoveNext())
        {
            enumerator.Current.runOnNode(node);
        }
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        elapsed += dt;

        DAction action = enumerator.Current;

        if (action != null)
        {
            float actionElapsed = action.elapsed;

            action.evaluateWithNode(node, dt);

            if (actionElapsed + dt > action.duration)
            {
                if (enumerator.MoveNext())
                {
                    enumerator.Current.runOnNode(node);
                    dt = actionElapsed + dt - action.duration;
                    elapsed -= dt;
                    evaluateWithNode(node, dt);
                }
            }
        }
    }
}
