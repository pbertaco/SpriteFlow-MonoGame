namespace Dragon;

using DActionTimingFunction = Func<float, float, float, float, float>;

public class DAction
{
    internal float duration;
    internal DActionTimingFunction timingFunction = DEasing.linear;

    internal float elapsed;

    internal float t0;

    internal DAction(float duration)
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

    internal DAction with(DActionTimingFunction timingFunction)
    {
        this.timingFunction = timingFunction;
        return this;
    }

    internal static DAction moveBy(Vector2 delta, float duration)
    {
        return new DActionMoveBy(delta, duration);
    }

    internal static DAction moveBy(float deltaX, float deltaY, float duration)
    {
        return new DActionMoveBy(new Vector2(deltaX, deltaY), duration);
    }

    internal static DAction moveTo(Vector2 location, float duration)
    {
        return new DActionMoveTo(location, duration);
    }

    internal static DAction moveTo(float locationX, float locationY, float duration)
    {
        return new DActionMoveTo(new Vector2(locationX, locationY), duration);
    }

    internal static DAction rotateBy(float radians, float duration)
    {
        return new DActionRotateBy(radians, duration);
    }

    internal static DAction rotateTo(float radians, float duration)
    {
        return new DActionRotateTo(radians, duration);
    }

    internal static DAction resizeBy(Vector2 size, float duration)
    {
        return new DActionResizeBy(size, duration);
    }

    internal static DAction resizeBy(float sizeX, float sizeY, float duration)
    {
        return new DActionResizeBy(new Vector2(sizeX, sizeY), duration);
    }

    internal static DAction resizeTo(Vector2 size, float duration)
    {
        return new DActionResizeTo(size, duration);
    }

    internal static DAction resizeTo(float sizeX, float sizeY, float duration)
    {
        return new DActionResizeTo(new Vector2(sizeX, sizeY), duration);
    }

    internal static DAction scaleBy(Vector2 scale, float duration)
    {
        return new DActionScaleBy(scale, duration);
    }

    internal static DAction scaleBy(float scaleX, float scaleY, float duration)
    {
        return new DActionScaleBy(new Vector2(scaleX, scaleY), duration);
    }

    internal static DAction scaleBy(float scale, float duration)
    {
        return new DActionScaleBy(new Vector2(scale, scale), duration);
    }

    internal static DAction scaleTo(Vector2 scale, float duration)
    {
        return new DActionScaleTo(scale, duration);
    }

    internal static DAction scaleTo(float scaleX, float scaleY, float duration)
    {
        return new DActionScaleTo(new Vector2(scaleX, scaleY), duration);
    }

    internal static DAction scaleTo(float scale, float duration)
    {
        return new DActionScaleTo(new Vector2(scale, scale), duration);
    }

    internal static DAction sequence(IEnumerable<DAction> actions)
    {
        return new DActionSequence(actions);
    }

    internal static DAction group(IEnumerable<DAction> actions)
    {
        return new DActionGroup(actions);
    }

    internal static DAction repeat(DAction action, int count)
    {
        return new DActionRepeat(action, count);
    }

    internal static DAction repeatForever(DAction action)
    {
        return new DActionRepeat(action, int.MaxValue);
    }

    internal static DAction fadeIn(float duration)
    {
        return new DActionFadeAlphaTo(1, duration);
    }

    internal static DAction fadeOut(float duration)
    {
        return new DActionFadeAlphaTo(0, duration);
    }

    internal static DAction fadeAlphaBy(float factor, float duration)
    {
        return new DActionFadeAlphaBy(factor, duration);
    }

    internal static DAction fadeAlphaTo(float alpha, float duration)
    {
        return new DActionFadeAlphaTo(alpha, duration);
    }

    internal static DAction hide()
    {
        return new DActionHide();
    }

    internal static DAction unhide()
    {
        return new DActionUnhide();
    }

    internal static DAction setTexture(Texture2D texture, bool resize = false)
    {
        return new DActionSetTexture(texture, resize);
    }

    internal static DAction setTexture(string assetName, bool resize = false)
    {
        return new DActionSetTexture(DSpriteNode.loadTexture2D(assetName), resize);
    }

    internal static DAction animate(IEnumerable<Texture2D> textures, float duration, bool resize = false, bool restore = false)
    {
        return new DActionAnimate(textures, duration, resize, restore);
    }

    internal static DAction animate(IEnumerable<string> textures, float duration, bool resize = false, bool restore = false)
    {
        List<Texture2D> list = new List<Texture2D>();

        foreach (string i in textures)
        {
            list.Add(DSpriteNode.loadTexture2D(i));
        }

        return new DActionAnimate(list, duration, resize, restore);
    }

    internal static DAction playSoundEffect(string assetName, bool waitForCompletion = false)
    {
        return new DActionPlaySoundEffect(DGame.current.Content.Load<SoundEffect>(assetName), waitForCompletion);
    }

    internal static DAction colorize(Color color, float colorBlendFactor, float duration)
    {
        return new DActionColorize(color, colorBlendFactor, duration);
    }

    internal static DAction colorize(float colorBlendFactor, float duration)
    {
        return new DActionColorize(Color.White, colorBlendFactor, duration);
    }

    internal static DAction speedBy(float speed, float duration)
    {
        throw new NotImplementedException();
    }

    internal static DAction speedTo(float speed, float duration)
    {
        throw new NotImplementedException();
    }

    internal static DAction waitForDuration(float duration)
    {
        return new DActionWait(duration, 0);
    }

    internal static DAction waitForDuration(float duration, float durationRange)
    {
        return new DActionWait(duration, durationRange);
    }

    internal static DAction removeFromParent()
    {
        return new DActionRemoveFromParent();
    }

    internal static DAction run(Action block)
    {
        return new DActionRunBlock(block);
    }

    internal static DAction afterDelay(float delay, DAction action)
    {
        return sequence(new[] { waitForDuration(delay), action });
    }

    internal static DAction afterDelay(float delay, Action block)
    {
        return afterDelay(delay, new DActionRunBlock(block));
    }

    internal static DAction removeFromParentAfterDelay(float delay)
    {
        return afterDelay(delay, new DActionRemoveFromParent());
    }

    internal virtual DAction copy()
    {
        return this;
    }

    internal virtual void runOnNode(DNode node)
    {
    }

    internal virtual void evaluateWithNode(DNode node, float dt)
    {
        if (elapsed + dt > duration)
        {
            dt = duration - elapsed;
        }

        elapsed += dt;
    }

    internal virtual DAction reversed()
    {
        return this;
    }

    internal static DAction colorize(Color black, int v)
    {
        throw new NotImplementedException();
    }
}
