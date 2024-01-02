namespace Dragon;

public class DActionRepeat : DAction
{
    DAction action;
    int count;

    int executionCount;

    internal DActionRepeat(DAction action, int count) : base(0)
    {
        this.action = action;
        this.count = count;

        duration = action.duration * count;
    }

    internal override DAction copy()
    {
        return new DActionRepeat(action, count);
    }

    internal override void runOnNode(DNode node)
    {
        action.runOnNode(node);
    }

    internal override void evaluateWithNode(DNode node, float dt)
    {
        elapsed += dt;

        if (executionCount < count)
        {
            float actionElapsed = action.elapsed;

            action.evaluateWithNode(node, dt);

            if (actionElapsed + dt > action.duration)
            {
                executionCount += 1;

                if (executionCount < count)
                {
                    action = action.copy(); //TODO: performance
                    action.runOnNode(node);
                    dt = actionElapsed + dt - action.duration;
                    elapsed -= dt;
                    evaluateWithNode(node, dt);
                }
            }
        }
    }
}
