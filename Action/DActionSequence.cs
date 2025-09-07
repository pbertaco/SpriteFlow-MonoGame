namespace Dragon;

public class DActionSequence : DActionGroup
{
    int currentIndex;

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
    }

    public override void evaluateWithNode(DNode node, float dt)
    {
        if (elapsed + dt > duration)
        {
            dt = duration - elapsed;
        }

        elapsed += dt;

        while (currentIndex < actions.Count)
        {
            DAction action = actions[currentIndex];

            if (action.elapsed == 0)
            {
                action.runOnNode(node);
            }

            float remainingActionTime = action.duration - action.elapsed;

            float timeForAction = Math.Min(dt, remainingActionTime);

            action.evaluateWithNode(node, timeForAction);

            action.elapsed += timeForAction;

            if (action.elapsed >= action.duration)
            {
                currentIndex++;
            }
            else
            {
                break;
            }

            dt -= timeForAction;
        }
    }

}
