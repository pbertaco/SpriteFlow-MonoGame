namespace Dragon;

using DActionTimingFunction = Func<float, float, float, float, float>;

public class DAction
{
    public float duration;
    public DActionTimingFunction timingFunction = DEasing.linear;

    public float elapsed;

    public float t0;

    public DAction(float duration)
    {
        if (duration <= 0)
        {
            this.duration = 0.001f;
        }
        else
        {
            this.duration = duration;
        }
    }

    public DAction with(DActionTimingFunction timingFunction)
    {
        this.timingFunction = timingFunction;
        return this;
    }

    public static DAction moveBy(Vector2 delta, float duration)
    {
        return new DActionMoveBy(delta, duration);
    }

    public static DAction moveBy(float deltaX, float deltaY, float duration)
    {
        return new DActionMoveBy(new Vector2(deltaX, deltaY), duration);
    }

    public static DAction moveTo(Vector2 location, float duration)
    {
        return new DActionMoveTo(location, duration);
    }

    public static DAction moveTo(float locationX, float locationY, float duration)
    {
        return new DActionMoveTo(new Vector2(locationX, locationY), duration);
    }

    public static DAction rotateBy(float radians, float duration)
    {
        return new DActionRotateBy(radians, duration);
    }

    public static DAction rotateTo(float radians, float duration)
    {
        return new DActionRotateTo(radians, duration);
    }

    public static DAction resizeBy(Vector2 size, float duration)
    {
        return new DActionResizeBy(size, duration);
    }

    public static DAction resizeBy(float sizeX, float sizeY, float duration)
    {
        return new DActionResizeBy(new Vector2(sizeX, sizeY), duration);
    }

    public static DAction resizeTo(Vector2 size, float duration)
    {
        return new DActionResizeTo(size, duration);
    }

    public static DAction resizeTo(float sizeX, float sizeY, float duration)
    {
        return new DActionResizeTo(new Vector2(sizeX, sizeY), duration);
    }

    public static DAction scaleBy(Vector2 scale, float duration)
    {
        return new DActionScaleBy(scale, duration);
    }

    public static DAction scaleBy(float scaleX, float scaleY, float duration)
    {
        return new DActionScaleBy(new Vector2(scaleX, scaleY), duration);
    }

    public static DAction scaleBy(float scale, float duration)
    {
        return new DActionScaleBy(new Vector2(scale, scale), duration);
    }

    public static DAction scaleTo(Vector2 scale, float duration)
    {
        return new DActionScaleTo(scale, duration);
    }

    public static DAction scaleTo(float scaleX, float scaleY, float duration)
    {
        return new DActionScaleTo(new Vector2(scaleX, scaleY), duration);
    }

    public static DAction scaleTo(float scale, float duration)
    {
        return new DActionScaleTo(new Vector2(scale, scale), duration);
    }

    public static DAction sequence(IEnumerable<DAction> actions)
    {
        return new DActionSequence(actions);
    }

    public static DAction group(IEnumerable<DAction> actions)
    {
        return new DActionGroup(actions);
    }

    public static DAction repeat(DAction action, int count)
    {
        return new DActionRepeat(action, count);
    }

    public static DAction repeatForever(DAction action)
    {
        return new DActionRepeat(action, int.MaxValue);
    }

    public static DAction fadeIn(float duration)
    {
        return new DActionFadeAlphaTo(1, duration);
    }

    public static DAction fadeOut(float duration)
    {
        return new DActionFadeAlphaTo(0, duration);
    }

    public static DAction fadeAlphaBy(float factor, float duration)
    {
        return new DActionFadeAlphaBy(factor, duration);
    }

    public static DAction fadeAlphaTo(float alpha, float duration)
    {
        return new DActionFadeAlphaTo(alpha, duration);
    }

    public static DAction hide()
    {
        return new DActionHide();
    }

    public static DAction unhide()
    {
        return new DActionUnhide();
    }

    public static DAction setTexture(Texture2D texture, bool resize = false)
    {
        return new DActionSetTexture(texture, resize);
    }

    public static DAction setTexture(string assetName, bool resize = false)
    {
        return new DActionSetTexture(DSpriteNode.loadTexture2D(assetName), resize);
    }

    public static DAction animate(IEnumerable<Texture2D> textures, float duration, bool resize = false, bool restore = false)
    {
        return new DActionAnimate(textures, duration, resize, restore);
    }

    public static DAction animate(IEnumerable<string> textures, float duration, bool resize = false, bool restore = false)
    {
        List<Texture2D> list = new List<Texture2D>();

        foreach (string i in textures)
        {
            list.Add(DSpriteNode.loadTexture2D(i));
        }

        return new DActionAnimate(list, duration, resize, restore);
    }

    public static DAction playSoundEffect(string assetName, bool waitForCompletion = false)
    {
        return new DActionPlaySoundEffect(DGame.current.Content.Load<SoundEffect>(assetName), waitForCompletion);
    }

    public static DAction colorize(Color color, float colorBlendFactor, float duration)
    {
        return new DActionColorize(color, colorBlendFactor, duration);
    }

    public static DAction colorize(float colorBlendFactor, float duration)
    {
        return new DActionColorize(Color.White, colorBlendFactor, duration);
    }

    public static DAction speedBy(float speed, float duration)
    {
        throw new NotImplementedException();
    }

    public static DAction speedTo(float speed, float duration)
    {
        throw new NotImplementedException();
    }

    public static DAction waitForDuration(float duration)
    {
        return new DActionWait(duration, 0);
    }

    public static DAction waitForDuration(float duration, float durationRange)
    {
        return new DActionWait(duration, durationRange);
    }

    public static DAction removeFromParent()
    {
        return new DActionRemoveFromParent();
    }

    public static DAction run(Action block)
    {
        return new DActionRunBlock(block);
    }

    public static DAction afterDelay(float delay, DAction action)
    {
        return sequence(new[] { waitForDuration(delay), action });
    }

    public static DAction afterDelay(float delay, Action block)
    {
        return afterDelay(delay, new DActionRunBlock(block));
    }

    public static DAction removeFromParentAfterDelay(float delay)
    {
        return afterDelay(delay, new DActionRemoveFromParent());
    }

    public virtual DAction copy()
    {
        return this;
    }

    public virtual void runOnNode(DNode node)
    {
    }

    public virtual void evaluateWithNode(DNode node, float dt)
    {
        if (elapsed + dt > duration)
        {
            dt = duration - elapsed;
        }

        elapsed += dt;
    }

    public virtual DAction reversed()
    {
        return this;
    }

    public static DAction colorize(Color black, int v)
    {
        throw new NotImplementedException();
    }
}
