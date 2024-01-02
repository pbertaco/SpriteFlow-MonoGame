namespace Dragon;

public class DButton : DControl
{
    internal Action onTouchDownInside;
    internal Action onTouchUpInside;
    internal Action onTouchUpOutside;
    internal Action onTouchMovedInside;
    internal Action onTouchMovedOutside;

    List<DTouch> touchList = new List<DTouch>();

    DSpriteNode icon;

    internal DButton(Color? color = null, Vector2? size = null) : base(color, size)
    {
    }

    internal DButton(Vector2 size) : base(size)
    {
    }

    internal DButton(Texture2D texture, Color? color = null, Vector2? size = null) : base(texture, color, size)
    {
    }

    internal DButton(Texture2D texture, Vector2 size) : base(texture, size)
    {
    }

    internal DButton(string assetName, Color? color = null, Vector2? size = null) : base(assetName, color, size)
    {
    }

    internal DButton(string assetName, Vector2 size) : base(assetName, size)
    {
    }

    internal override void load(Texture2D texture, Color? color, Vector2? size)
    {
        base.load(texture, color, size);
        userInteractionEnabled = true;
    }

    internal void addIcon(DSpriteNode icon, bool scaleToFit = true)
    {
        if (this.icon != null)
        {
            this.icon.removeFromParent();
        }

        addChild(icon);
        icon.position = size / 2;

        if (scaleToFit)
        {
            icon.setScaleToFit(size);
        }

        this.icon = icon;
    }

    internal void addIcon(string assetName, bool scaleToFit = true)
    {
        addIcon(new DSpriteNode(assetName), scaleToFit);
    }

    internal override void touchDown(DTouch touch)
    {
        base.touchDown(touch);

        if (userInteractionEnabled && contains(touch))
        {
            touchList.Add(touch);
            onTouchDownInside?.Invoke();
        }
    }

    internal override void touchMoved(DTouch touch)
    {
        if (userInteractionEnabled && touchList.Contains(touch))
        {
            if (contains(touch))
            {
                onTouchMovedInside?.Invoke();
            }
            else
            {
                onTouchMovedOutside?.Invoke();
            }
        }
    }

    internal override void touchUp(DTouch touch)
    {
        base.touchUp(touch);

        if (userInteractionEnabled && touchList.Remove(touch))
        {
            if (contains(touch))
            {
                onTouchUpInside?.Invoke();
            }
            else
            {
                onTouchUpOutside?.Invoke();
            }
        }
    }

    internal override void removeFromParent(bool recursive = true)
    {
        base.removeFromParent(recursive);

        if (recursive)
        {
            onTouchDownInside = null;
            onTouchUpInside = null;
            onTouchUpOutside = null;
            onTouchMovedInside = null;
            onTouchMovedOutside = null;
        }
    }

    internal void setOnTouchActionSetTexture(string assetName, DSpriteNode target = null)
    {
        target = target ?? this;
        setOnTouchAction(DAction.setTexture(assetName), DAction.setTexture(target.texture), target);
    }

    internal void setOnTouchAction(DAction actionDown, DAction actionUp, DNode target = null)
    {
        target = target ?? this;
        string actionDownKey = nextActionKey();
        string actionUpKey = nextActionKey();

        onTouchDownInside += () =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionDown, actionDownKey);
        };

        onTouchUpInside += () =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionUp, actionUpKey);
        };

        onTouchUpOutside += () =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionUp, actionUpKey);
        };
    }
}
