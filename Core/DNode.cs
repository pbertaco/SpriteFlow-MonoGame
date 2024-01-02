namespace Dragon;

public class DNode
{
    internal Vector2 position;
    internal float rotation;
    internal Vector2 scale;
    internal float alpha;
    internal bool hidden;
    internal DNode parent;
    internal List<DNode> children;

    internal Vector2 drawPosition;
    internal float drawRotation;
    internal Vector2 drawScale;
    internal float drawAlpha;

    internal static Random random = new();

    internal Dictionary<string, DAction> actions;
    internal List<string> actionsToRemove;

    internal bool userInteractionEnabled;

    internal DNode()
    {
        scale = Vector2.One;
        alpha = 1;
        children = new List<DNode>();
        actions = new Dictionary<string, DAction>();
        actionsToRemove = new List<string>();
    }

    internal virtual void update()
    {
        for (int i = children.Count - 1; i >= 0; i--)
        {
            children[i].update();
        }
    }

    internal virtual void beforeDraw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        drawPosition = currentPosition + (position.rotateBy(currentRotation) * currentScale);
        drawRotation = currentRotation + rotation;
        drawScale = currentScale * scale;
        drawAlpha = currentAlpha * alpha;
    }

    internal void drawChildren()
    {
        foreach (DNode node in children)
        {
            node.draw(drawPosition, drawRotation, drawScale, drawAlpha);
        }
    }

    internal virtual void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);
        drawChildren();
    }

    internal virtual T addChild<T>(T node) where T : DNode
    {
        children.Add(node);
        node.setParent(this);
        return node;
    }

    internal virtual T addChild<T>(T node, Vector2 position) where T : DNode
    {
        addChild(node);
        node.position = position;
        return node;
    }

    internal virtual T insertChild<T>(T node, int index) where T : DNode
    {
        children.Insert(index, node);
        node.setParent(this);
        return node;
    }

    internal virtual T insertChild<T>(T node, Vector2 position, int index) where T : DNode
    {
        insertChild(node, index);
        node.position = position;
        return node;
    }

    void setParent(DNode node)
    {
        parent = node;

        if (userInteractionEnabled)
        {
            node.userInteractionEnabled = true;
        }

        if (node.parent != null)
        {
            node.setParent(node.parent);
        }
    }

    void removeChildren(IEnumerable<DNode> nodes)
    {
        foreach (DNode node in nodes)
        {
            if (node.parent == this)
            {
                node.removeFromParent();
            }
        }
    }

    internal void removeAllChildren(bool recursive = true)
    {
        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (i >= children.Count)
            {
                continue;
            }

            children[i].removeFromParent(recursive);
        }
    }

    internal virtual void removeFromParent(bool recursive = true)
    {
        if (parent != null)
        {
            parent.children.Remove(this);
            parent = null;
        }

        if (recursive)
        {
            removeAllActions();
            removeAllChildren(recursive);
        }
    }

    void moveToParent(DNode node)
    {
        removeFromParent(false);
        node.addChild(this);
    }

    internal virtual void touchDown(DTouch touch)
    {
        if (userInteractionEnabled && !hidden)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (i >= children.Count)
                {
                    continue;
                }

                DNode node = children[i];
                node.touchDown(touch);
            }
        }
    }

    internal virtual void touchMoved(DTouch touch)
    {
        if (userInteractionEnabled && !hidden)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (i >= children.Count)
                {
                    continue;
                }

                DNode node = children[i];
                node.touchMoved(touch);
            }
        }
    }

    internal virtual void touchUp(DTouch touch)
    {
        if (userInteractionEnabled && !hidden)
        {
            for (int i = children.Count - 1; i >= 0; i--)
            {
                if (i >= children.Count)
                {
                    continue;
                }

                DNode node = children[i];
                node.touchUp(touch);
            }
        }
    }

    internal static string nextActionKey()
    {
        return $"{DGame.currentTime}{random.NextDouble()}";
    }

    internal void run(DAction action)
    {
        run(action, nextActionKey());
    }

    internal void run(IEnumerable<DAction> actionList)
    {
        run(DAction.sequence(actionList), nextActionKey());
    }

    internal void run(DAction action, string key)
    {
        DAction copy = action.copy();
        copy.runOnNode(this);

        if (actions.ContainsKey(key))
        {
            actions[key] = copy;
        }
        else
        {
            actions.Add(key, copy);
        }
    }

    internal void run(IEnumerable<DAction> actionList, string key)
    {
        run(DAction.sequence(actionList), key);
    }

    internal void run(DAction action, Action completionBlock)
    {
        run(DAction.sequence(new List<DAction>() { action, DAction.run(completionBlock) }));
    }

    internal bool hasActions()
    {
        return actions.Count > 0;
    }

    internal DAction actionForKey(string key)
    {
        return actions[key];
    }

    internal void removeActionForKey(string key)
    {
        actions.Remove(key);
    }

    internal void removeAllActions()
    {
        actions.Clear();
    }

    internal void evaluateActions(float dt)
    {
        foreach (KeyValuePair<string, DAction> keyValuePair in actions)
        {
            DAction action = keyValuePair.Value;
            action.evaluateWithNode(this, dt);

            if (action.elapsed >= action.duration)
            {
                actionsToRemove.Add(keyValuePair.Key);
            }
        }

        if (actionsToRemove.Count > 0)
        {
            foreach (string key in actionsToRemove)
            {
                actions.Remove(key);
            }

            actionsToRemove.Clear();
        }

        for (int i = children.Count - 1; i >= 0; i--)
        {
            if (i >= children.Count)
            {
                continue;
            }

            children[i].evaluateActions(dt);
        }
    }

    internal void afterDelay(float duration, Action block)
    {
        run(DAction.sequence(new List<DAction>() { DAction.waitForDuration(duration), DAction.run(block) }));
    }

    internal void bringToFront()
    {
        if (parent != null)
        {
            parent.children.Remove(this);
            parent.children.Add(this);
        }
    }
}
