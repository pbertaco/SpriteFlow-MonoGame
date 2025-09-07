namespace Dragon;

public class DButton : DControl
{
    public static List<DButton> allButtons = new();

    public Action<DTouch> onTouchDownInside;
    public Action<DTouch> onTouchUpInside;
    public Action<DTouch> onTouchUpOutside;
    public Action<DTouch> onTouchMovedInside;
    public Action<DTouch> onTouchMovedOutside;

    List<DTouch> touchList = new();

    DSpriteNode icon;

    public DButton(Color? color = null, Vector2? size = null) : base(color, size)
    {
    }

    public DButton(Vector2 size) : base(size)
    {
    }

    public DButton(Texture2D texture, Color? color = null, Vector2? size = null) : base(texture, color, size)
    {
    }

    public DButton(Texture2D texture, Vector2 size) : base(texture, size)
    {
    }

    public DButton(string assetName, Color? color = null, Vector2? size = null) : base(assetName, color, size)
    {
    }

    public DButton(string assetName, Vector2 size) : base(assetName, size)
    {
    }

    public override void load(Texture2D texture, Color? color, Vector2? size)
    {
        base.load(texture, color, size);
        userInteractionEnabled = true;
    }

    public void addIcon(DSpriteNode icon, bool scaleToFit = true)
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

    public void addIcon(string assetName, bool scaleToFit = true)
    {
        addIcon(new DSpriteNode(assetName), scaleToFit);
    }

    public override void touchDown(DTouch touch)
    {
        base.touchDown(touch);

        if (userInteractionEnabled && contains(touch))
        {
            touchList.Add(touch);
            onTouchDownInside?.Invoke(touch);
        }
    }

    public override void touchMoved(DTouch touch)
    {
        if (userInteractionEnabled && touchList.Contains(touch))
        {
            if (contains(touch))
            {
                onTouchMovedInside?.Invoke(touch);
            }
            else
            {
                onTouchMovedOutside?.Invoke(touch);
            }
        }
    }

    public override void touchUp(DTouch touch)
    {
        base.touchUp(touch);

        if (userInteractionEnabled && touchList.Remove(touch))
        {
            if (contains(touch))
            {
                onTouchUpInside?.Invoke(touch);
            }
            else
            {
                onTouchUpOutside?.Invoke(touch);
            }
        }
    }

    public override void removeFromParent(bool recursive = true)
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

    public void setOnTouchActionSetTexture(string assetName, DSpriteNode target = null)
    {
        target = target ?? this;
        setOnTouchAction(DAction.setTexture(assetName), DAction.setTexture(target.texture), target);
    }

    public void setOnTouchAction(DAction actionDown, DAction actionUp, DNode target = null)
    {
        target = target ?? this;
        string actionDownKey = nextActionKey();
        string actionUpKey = nextActionKey();

        onTouchDownInside += (touch) =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionDown, actionDownKey);
        };

        onTouchUpInside += (touch) =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionUp, actionUpKey);
        };

        onTouchUpOutside += (touch) =>
        {
            target.removeActionForKey(actionDownKey);
            target.removeActionForKey(actionUpKey);
            target.run(actionUp, actionUpKey);
        };
    }
}
