namespace Dragon;

public class DEasing
{
    public static Func<float, float> createShakeFunction(int oscillations)
    {
        return currentTime => (float)((-Math.Pow(2.0, -10.0 * currentTime) * Math.Sin(currentTime * Math.PI * oscillations * 2.0)) + 1.0f);
    }

    public static float linear(float currentTime)
    {
        return (1.0f * currentTime / 1.0f) + 0.0f;
    }

    public static float expoEaseOut(float currentTime)
    {
        return (currentTime == 1.0f) ? 0.0f + 1.0f : (1.0f * (-(float)Math.Pow(2, -10 * currentTime / 1.0f) + 1)) + 0.0f;
    }

    public static float expoEaseIn(float currentTime)
    {
        return (currentTime == 0) ? 0.0f : (1.0f * (float)Math.Pow(2, 10 * ((currentTime / 1.0f) - 1))) + 0.0f;
    }

    public static float expoEaseInOut(float currentTime)
    {
        if (currentTime == 0)
            return 0.0f;

        if (currentTime == 1.0f)
            return 0.0f + 1.0f;

        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * (float)Math.Pow(2, 10 * (currentTime - 1))) + 0.0f;

        return (1.0f / 2 * (-(float)Math.Pow(2, -10 * --currentTime) + 2)) + 0.0f;
    }

    public static float expoEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return expoEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return expoEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float circEaseOut(float currentTime)
    {
        return (1.0f * (float)Math.Sqrt(1 - ((currentTime = (currentTime / 1.0f) - 1) * currentTime))) + 0.0f;
    }

    public static float circEaseIn(float currentTime)
    {
        return (-1.0f * ((float)Math.Sqrt(1 - ((currentTime /= 1.0f) * currentTime)) - 1)) + 0.0f;
    }

    public static float circEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (-1.0f / 2 * ((float)Math.Sqrt(1 - (currentTime * currentTime)) - 1)) + 0.0f;

        return (1.0f / 2 * ((float)Math.Sqrt(1 - ((currentTime -= 2) * currentTime)) + 1)) + 0.0f;
    }

    public static float circEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return circEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return circEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float quadEaseOut(float currentTime)
    {
        return (-1.0f * (currentTime /= 1.0f) * (currentTime - 2)) + 0.0f;
    }

    public static float quadEaseIn(float currentTime)
    {
        return (1.0f * (currentTime /= 1.0f) * currentTime) + 0.0f;
    }

    public static float quadEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * currentTime * currentTime) + 0.0f;

        return (-1.0f / 2 * (((--currentTime) * (currentTime - 2)) - 1)) + 0.0f;
    }

    public static float quadEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return quadEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return quadEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float sineEaseOut(float currentTime)
    {
        return (1.0f * (float)Math.Sin(currentTime / 1.0f * ((float)Math.PI / 2))) + 0.0f;
    }

    public static float sineEaseIn(float currentTime)
    {
        return (-1.0f * (float)Math.Cos(currentTime / 1.0f * ((float)Math.PI / 2))) + 1.0f + 0.0f;
    }

    public static float sineEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * ((float)Math.Sin((float)Math.PI * currentTime / 2))) + 0.0f;

        return (-1.0f / 2 * ((float)Math.Cos((float)Math.PI * --currentTime / 2) - 2)) + 0.0f;
    }

    public static float sineEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return sineEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return sineEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float cubicEaseOut(float currentTime)
    {
        return (1.0f * (((currentTime = (currentTime / 1.0f) - 1) * currentTime * currentTime) + 1)) + 0.0f;
    }

    public static float cubicEaseIn(float currentTime)
    {
        return (1.0f * (currentTime /= 1.0f) * currentTime * currentTime) + 0.0f;
    }

    public static float cubicEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * currentTime * currentTime * currentTime) + 0.0f;

        return (1.0f / 2 * (((currentTime -= 2) * currentTime * currentTime) + 2)) + 0.0f;
    }

    public static float cubicEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return cubicEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return cubicEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float quartEaseOut(float currentTime)
    {
        return (-1.0f * (((currentTime = (currentTime / 1.0f) - 1) * currentTime * currentTime * currentTime) - 1)) + 0.0f;
    }

    public static float quartEaseIn(float currentTime)
    {
        return (1.0f * (currentTime /= 1.0f) * currentTime * currentTime * currentTime) + 0.0f;
    }

    public static float quartEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * currentTime * currentTime * currentTime * currentTime) + 0.0f;

        return (-1.0f / 2 * (((currentTime -= 2) * currentTime * currentTime * currentTime) - 2)) + 0.0f;
    }

    public static float quartEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return quartEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);

        return quartEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float quintEaseOut(float currentTime)
    {
        return (1.0f * (((currentTime = (currentTime / 1.0f) - 1) * currentTime * currentTime * currentTime * currentTime) + 1)) + 0.0f;
    }

    public static float quintEaseIn(float currentTime)
    {
        return (1.0f * (currentTime /= 1.0f) * currentTime * currentTime * currentTime * currentTime) + 0.0f;
    }

    public static float quintEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * currentTime * currentTime * currentTime * currentTime * currentTime) + 0.0f;
        return (1.0f / 2 * (((currentTime -= 2) * currentTime * currentTime * currentTime * currentTime) + 2)) + 0.0f;
    }

    public static float quintEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return quintEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);
        return quintEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float elasticEaseOut(float currentTime)
    {
        if ((currentTime /= 1.0f) == 1)
            return 0.0f + 1.0f;

        float p = 1.0f * .3f;
        float s = p / 4;

        return (1.0f * (float)Math.Pow(2, -10 * currentTime) * (float)Math.Sin(((currentTime * 1.0f) - s) * (2 * (float)Math.PI) / p)) + 1.0f + 0.0f;
    }

    public static float elasticEaseIn(float currentTime)
    {
        if ((currentTime /= 1.0f) == 1)
            return 0.0f + 1.0f;

        float p = 1.0f * .3f;
        float s = p / 4;

        return -(1.0f * (float)Math.Pow(2, 10 * (currentTime -= 1)) * (float)Math.Sin(((currentTime * 1.0f) - s) * (2 * (float)Math.PI) / p)) + 0.0f;
    }

    public static float elasticEaseInOut(float currentTime)
    {
        if ((currentTime /= 1.0f / 2) == 2)
            return 0.0f + 1.0f;

        float p = 1.0f * (.3f * 1.5f);
        float s = p / 4;

        if (currentTime < 1)
            return (-.5f * (1.0f * (float)Math.Pow(2, 10 * (currentTime -= 1)) * (float)Math.Sin(((currentTime * 1.0f) - s) * (2 * (float)Math.PI) / p))) + 0.0f;
        return (1.0f * (float)Math.Pow(2, -10 * (currentTime -= 1)) * (float)Math.Sin(((currentTime * 1.0f) - s) * (2 * (float)Math.PI) / p) * .5f) + 1.0f + 0.0f;
    }

    public static float elasticEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return elasticEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);
        return elasticEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float bounceEaseOut(float currentTime)
    {
        if ((currentTime /= 1.0f) < (1 / 2.75f))
            return (1.0f * (7.5625f * currentTime * currentTime)) + 0.0f;
        else if (currentTime < (2 / 2.75f))
            return (1.0f * ((7.5625f * (currentTime -= 1.5f / 2.75f) * currentTime) + .75f)) + 0.0f;
        else if (currentTime < (2.5 / 2.75f))
            return (1.0f * ((7.5625f * (currentTime -= 2.25f / 2.75f) * currentTime) + .9375f)) + 0.0f;
        else
            return (1.0f * ((7.5625f * (currentTime -= 2.625f / 2.75f) * currentTime) + .984375f)) + 0.0f;
    }

    public static float bounceEaseIn(float currentTime)
    {
        return 1.0f - bounceEaseOut(1.0f - currentTime, 0, 1.0f, 1.0f) + 0.0f;
    }

    public static float bounceEaseInOut(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return (bounceEaseIn(currentTime * 2, 0, 1.0f, 1.0f) * .5f) + 0.0f;
        else
            return (bounceEaseOut((currentTime * 2) - 1.0f, 0, 1.0f, 1.0f) * .5f) + (1.0f * .5f) + 0.0f;
    }

    public static float bounceEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return bounceEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);
        return bounceEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    public static float backEaseOut(float currentTime)
    {
        return (1.0f * (((currentTime = (currentTime / 1.0f) - 1) * currentTime * (((1.70158f + 1) * currentTime) + 1.70158f)) + 1)) + 0.0f;
    }

    public static float backEaseIn(float currentTime)
    {
        return (1.0f * (currentTime /= 1.0f) * currentTime * (((1.70158f + 1) * currentTime) - 1.70158f)) + 0.0f;
    }

    public static float backEaseInOut(float currentTime)
    {
        float s = 1.70158f;
        if ((currentTime /= 1.0f / 2) < 1)
            return (1.0f / 2 * (currentTime * currentTime * ((((s *= 1.525f) + 1) * currentTime) - s))) + 0.0f;
        return (1.0f / 2 * (((currentTime -= 2) * currentTime * ((((s *= 1.525f) + 1) * currentTime) + s)) + 2)) + 0.0f;
    }

    public static float backEaseOutIn(float currentTime)
    {
        if (currentTime < 1.0f / 2)
            return backEaseOut(currentTime * 2, 0.0f, 1.0f / 2, 1.0f);
        return backEaseIn((currentTime * 2) - 1.0f, 0.0f + (1.0f / 2), 1.0f / 2, 1.0f);
    }

    static float expoEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (currentTime == duration) ? startValue + changeInValue : (changeInValue * (-(float)Math.Pow(2, -10 * currentTime / duration) + 1)) + startValue;
    }

    static float expoEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (currentTime == 0) ? startValue : (changeInValue * (float)Math.Pow(2, 10 * ((currentTime / duration) - 1))) + startValue;
    }

    static float circEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (float)Math.Sqrt(1 - ((currentTime = (currentTime / duration) - 1) * currentTime))) + startValue;
    }

    static float circEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (-changeInValue * ((float)Math.Sqrt(1 - ((currentTime /= duration) * currentTime)) - 1)) + startValue;
    }

    static float quadEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (-changeInValue * (currentTime /= duration) * (currentTime - 2)) + startValue;
    }

    static float quadEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (currentTime /= duration) * currentTime) + startValue;
    }

    static float sineEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (float)Math.Sin(currentTime / duration * ((float)Math.PI / 2))) + startValue;
    }

    static float sineEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (-changeInValue * (float)Math.Cos(currentTime / duration * ((float)Math.PI / 2))) + changeInValue + startValue;
    }

    static float cubicEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (((currentTime = (currentTime / duration) - 1) * currentTime * currentTime) + 1)) + startValue;
    }

    static float cubicEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (currentTime /= duration) * currentTime * currentTime) + startValue;
    }

    static float quartEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (-changeInValue * (((currentTime = (currentTime / duration) - 1) * currentTime * currentTime * currentTime) - 1)) + startValue;
    }

    static float quartEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (currentTime /= duration) * currentTime * currentTime * currentTime) + startValue;
    }

    static float quintEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (((currentTime = (currentTime / duration) - 1) * currentTime * currentTime * currentTime * currentTime) + 1)) + startValue;
    }

    static float quintEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (currentTime /= duration) * currentTime * currentTime * currentTime * currentTime) + startValue;
    }

    static float elasticEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        if ((currentTime /= duration) == 1)
            return startValue + changeInValue;

        float p = duration * .3f;
        float s = p / 4;

        return (changeInValue * (float)Math.Pow(2, -10 * currentTime) * (float)Math.Sin(((currentTime * duration) - s) * (2 * (float)Math.PI) / p)) + changeInValue + startValue;
    }

    static float elasticEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        if ((currentTime /= duration) == 1)
            return startValue + changeInValue;

        float p = duration * .3f;
        float s = p / 4;

        return -(changeInValue * (float)Math.Pow(2, 10 * (currentTime -= 1)) * (float)Math.Sin(((currentTime * duration) - s) * (2 * (float)Math.PI) / p)) + startValue;
    }

    static float bounceEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        if ((currentTime /= duration) < (1 / 2.75f))
            return (changeInValue * (7.5625f * currentTime * currentTime)) + startValue;
        else if (currentTime < (2 / 2.75f))
            return (changeInValue * ((7.5625f * (currentTime -= 1.5f / 2.75f) * currentTime) + .75f)) + startValue;
        else if (currentTime < (2.5 / 2.75f))
            return (changeInValue * ((7.5625f * (currentTime -= 2.25f / 2.75f) * currentTime) + .9375f)) + startValue;
        else
            return (changeInValue * ((7.5625f * (currentTime -= 2.625f / 2.75f) * currentTime) + .984375f)) + startValue;
    }

    static float bounceEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return changeInValue - bounceEaseOut(duration - currentTime, 0, changeInValue, duration) + startValue;
    }

    static float backEaseOut(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (((currentTime = (currentTime / duration) - 1) * currentTime * (((1.70158f + 1) * currentTime) + 1.70158f)) + 1)) + startValue;
    }

    static float backEaseIn(float currentTime, float startValue, float changeInValue, float duration)
    {
        return (changeInValue * (currentTime /= duration) * currentTime * (((1.70158f + 1) * currentTime) - 1.70158f)) + startValue;
    }
}
