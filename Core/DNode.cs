namespace Dragon;

public class DNode
{
    public Vector2 position;
    public float rotation;
    public Vector2 scale;
    public float alpha;
    public bool hidden;
    public DNode parent;
    public List<DNode> children;

    public Vector2 drawPosition;
    public float drawRotation;
    public Vector2 drawScale;
    public float drawAlpha;

    public static Random random = new();

    public Dictionary<string, DAction> actions;
    public List<string> actionsToRemove;

    public bool userInteractionEnabled;

    public DNode()
    {
        scale = Vector2.One;
        alpha = 1;
        children = new List<DNode>();
        actions = new Dictionary<string, DAction>();
        actionsToRemove = new List<string>();
    }

    public virtual void update()
    {
        for (int i = children.Count - 1; i >= 0; i--)
        {
            children[i].update();
        }
    }

    public virtual void beforeDraw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        drawPosition = currentPosition + (position.rotateBy(currentRotation) * currentScale);
        drawRotation = currentRotation + rotation;
        drawScale = currentScale * scale;
        drawAlpha = currentAlpha * alpha;
    }

    public void drawChildren()
    {
        foreach (DNode node in children)
        {
            node.draw(drawPosition, drawRotation, drawScale, drawAlpha);
        }
    }

    public virtual void draw(Vector2 currentPosition, float currentRotation, Vector2 currentScale, float currentAlpha)
    {
        if (hidden || currentAlpha <= 0.01f)
        {
            return;
        }

        beforeDraw(currentPosition, currentRotation, currentScale, currentAlpha);
        drawChildren();
    }

    public virtual T addChild<T>(T node) where T : DNode
    {
        children.Add(node);
        node.setParent(this);
        return node;
    }

    public virtual T addChild<T>(T node, Vector2 position) where T : DNode
    {
        addChild(node);
        node.position = position;
        return node;
    }

    public T addChild<T>(T node, float x, float y) where T : DNode
    {
        return addChild(node, new Vector2(x, y));
    }

    public virtual T insertChild<T>(T node, int index) where T : DNode
    {
        children.Insert(index, node);
        node.setParent(this);
        return node;
    }

    public virtual T insertChild<T>(T node, Vector2 position, int index) where T : DNode
    {
        insertChild(node, index);
        node.position = position;
        return node;
    }

    public T insertChild<T>(T node, float x, float y, int index) where T : DNode
    {
        return insertChild(node, new Vector2(x, y), index);
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

    public void removeAllChildren(bool recursive = true)
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

    public virtual void removeFromParent(bool recursive = true)
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

    public virtual void touchDown(DTouch touch)
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

    public virtual void touchMoved(DTouch touch)
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

    public virtual void touchUp(DTouch touch)
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

    public static string nextActionKey()
    {
        return $"{DGame.currentTime}{random.NextDouble()}";
    }

    public void run(DAction action)
    {
        run(action, nextActionKey());
    }

    public void run(IEnumerable<DAction> actionList)
    {
        run(DAction.sequence(actionList), nextActionKey());
    }

    public void run(DAction action, string key)
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

    public void run(IEnumerable<DAction> actionList, string key)
    {
        run(DAction.sequence(actionList), key);
    }

    public void run(DAction action, Action completionBlock)
    {
        run(DAction.sequence(new List<DAction>() { action, DAction.run(completionBlock) }));
    }

    public bool hasActions()
    {
        return actions.Count > 0;
    }

    public DAction actionForKey(string key)
    {
        return actions[key];
    }

    public void removeActionForKey(string key)
    {
        actions.Remove(key);
    }

    public void removeAllActions()
    {
        actions.Clear();
    }

    public void evaluateActions(float dt)
    {
        for (int i = 0; i < actions.Count(); i++)
        {
            KeyValuePair<string, DAction> keyValuePair = actions.ElementAt(i);

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

    public void afterDelay(float duration, Action block)
    {
        run(DAction.sequence(new List<DAction>() { DAction.waitForDuration(duration), DAction.run(block) }));
    }

    public void bringToFront()
    {
        if (parent != null)
        {
            parent.children.Remove(this);
            parent.children.Add(this);
        }
    }

    public static void playSound(string name)
    {
        DGame.current.scene.run(DAction.playSoundEffect(name));
    }

    public static void playMusic(string name, string key = "")
    {
        DMusic music = new(name);
        music.play(key);
    }
}
