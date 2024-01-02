namespace Dragon;

public class DEasing
{
    internal static float linear(float t, float b, float c, float d)
    {
        return (c * t / d) + b;
    }

    internal static float expoEaseOut(float t, float b, float c, float d)
    {
        return (t == d) ? b + c : (c * (-(float)Math.Pow(2, -10 * t / d) + 1)) + b;
    }

    internal static float expoEaseIn(float t, float b, float c, float d)
    {
        return (t == 0) ? b : (c * (float)Math.Pow(2, 10 * ((t / d) - 1))) + b;
    }

    internal static float expoEaseInOut(float t, float b, float c, float d)
    {
        if (t == 0)
            return b;

        if (t == d)
            return b + c;

        if ((t /= d / 2) < 1)
            return (c / 2 * (float)Math.Pow(2, 10 * (t - 1))) + b;

        return (c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2)) + b;
    }

    internal static float expoEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return expoEaseOut(t * 2, b, c / 2, d);

        return expoEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float circEaseOut(float t, float b, float c, float d)
    {
        return (c * (float)Math.Sqrt(1 - ((t = (t / d) - 1) * t))) + b;
    }

    internal static float circEaseIn(float t, float b, float c, float d)
    {
        return (-c * ((float)Math.Sqrt(1 - ((t /= d) * t)) - 1)) + b;
    }

    internal static float circEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (-c / 2 * ((float)Math.Sqrt(1 - (t * t)) - 1)) + b;

        return (c / 2 * ((float)Math.Sqrt(1 - ((t -= 2) * t)) + 1)) + b;
    }

    internal static float circEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return circEaseOut(t * 2, b, c / 2, d);

        return circEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float quadEaseOut(float t, float b, float c, float d)
    {
        return (-c * (t /= d) * (t - 2)) + b;
    }

    internal static float quadEaseIn(float t, float b, float c, float d)
    {
        return (c * (t /= d) * t) + b;
    }

    internal static float quadEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (c / 2 * t * t) + b;

        return (-c / 2 * (((--t) * (t - 2)) - 1)) + b;
    }

    internal static float quadEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return quadEaseOut(t * 2, b, c / 2, d);

        return quadEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float sineEaseOut(float t, float b, float c, float d)
    {
        return (c * (float)Math.Sin(t / d * ((float)Math.PI / 2))) + b;
    }

    internal static float sineEaseIn(float t, float b, float c, float d)
    {
        return (-c * (float)Math.Cos(t / d * ((float)Math.PI / 2))) + c + b;
    }

    internal static float sineEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (c / 2 * ((float)Math.Sin((float)Math.PI * t / 2))) + b;

        return (-c / 2 * ((float)Math.Cos((float)Math.PI * --t / 2) - 2)) + b;
    }

    internal static float sineEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return sineEaseOut(t * 2, b, c / 2, d);

        return sineEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float cubicEaseOut(float t, float b, float c, float d)
    {
        return (c * (((t = (t / d) - 1) * t * t) + 1)) + b;
    }

    internal static float cubicEaseIn(float t, float b, float c, float d)
    {
        return (c * (t /= d) * t * t) + b;
    }

    internal static float cubicEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (c / 2 * t * t * t) + b;

        return (c / 2 * (((t -= 2) * t * t) + 2)) + b;
    }

    internal static float cubicEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return cubicEaseOut(t * 2, b, c / 2, d);

        return cubicEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float quartEaseOut(float t, float b, float c, float d)
    {
        return (-c * (((t = (t / d) - 1) * t * t * t) - 1)) + b;
    }

    internal static float quartEaseIn(float t, float b, float c, float d)
    {
        return (c * (t /= d) * t * t * t) + b;
    }

    internal static float quartEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (c / 2 * t * t * t * t) + b;

        return (-c / 2 * (((t -= 2) * t * t * t) - 2)) + b;
    }

    internal static float quartEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return quartEaseOut(t * 2, b, c / 2, d);

        return quartEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float quintEaseOut(float t, float b, float c, float d)
    {
        return (c * (((t = (t / d) - 1) * t * t * t * t) + 1)) + b;
    }

    internal static float quintEaseIn(float t, float b, float c, float d)
    {
        return (c * (t /= d) * t * t * t * t) + b;
    }

    internal static float quintEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) < 1)
            return (c / 2 * t * t * t * t * t) + b;
        return (c / 2 * (((t -= 2) * t * t * t * t) + 2)) + b;
    }

    internal static float quintEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return quintEaseOut(t * 2, b, c / 2, d);
        return quintEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float elasticEaseOut(float t, float b, float c, float d)
    {
        if ((t /= d) == 1)
            return b + c;

        float p = d * .3f;
        float s = p / 4;

        return (c * (float)Math.Pow(2, -10 * t) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p)) + c + b;
    }

    internal static float elasticEaseIn(float t, float b, float c, float d)
    {
        if ((t /= d) == 1)
            return b + c;

        float p = d * .3f;
        float s = p / 4;

        return -(c * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p)) + b;
    }

    internal static float elasticEaseInOut(float t, float b, float c, float d)
    {
        if ((t /= d / 2) == 2)
            return b + c;

        float p = d * (.3f * 1.5f);
        float s = p / 4;

        if (t < 1)
            return (-.5f * (c * (float)Math.Pow(2, 10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p))) + b;
        return (c * (float)Math.Pow(2, -10 * (t -= 1)) * (float)Math.Sin(((t * d) - s) * (2 * (float)Math.PI) / p) * .5f) + c + b;
    }

    internal static float elasticEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return elasticEaseOut(t * 2, b, c / 2, d);
        return elasticEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float bounceEaseOut(float t, float b, float c, float d)
    {
        if ((t /= d) < (1 / 2.75f))
            return (c * (7.5625f * t * t)) + b;
        else if (t < (2 / 2.75f))
            return (c * ((7.5625f * (t -= 1.5f / 2.75f) * t) + .75f)) + b;
        else if (t < (2.5 / 2.75f))
            return (c * ((7.5625f * (t -= 2.25f / 2.75f) * t) + .9375f)) + b;
        else
            return (c * ((7.5625f * (t -= 2.625f / 2.75f) * t) + .984375f)) + b;
    }

    internal static float bounceEaseIn(float t, float b, float c, float d)
    {
        return c - bounceEaseOut(d - t, 0, c, d) + b;
    }

    internal static float bounceEaseInOut(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return (bounceEaseIn(t * 2, 0, c, d) * .5f) + b;
        else
            return (bounceEaseOut((t * 2) - d, 0, c, d) * .5f) + (c * .5f) + b;
    }

    internal static float bounceEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return bounceEaseOut(t * 2, b, c / 2, d);
        return bounceEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }

    internal static float backEaseOut(float t, float b, float c, float d)
    {
        return (c * (((t = (t / d) - 1) * t * (((1.70158f + 1) * t) + 1.70158f)) + 1)) + b;
    }

    internal static float backEaseIn(float t, float b, float c, float d)
    {
        return (c * (t /= d) * t * (((1.70158f + 1) * t) - 1.70158f)) + b;
    }

    internal static float backEaseInOut(float t, float b, float c, float d)
    {
        float s = 1.70158f;
        if ((t /= d / 2) < 1)
            return (c / 2 * (t * t * ((((s *= 1.525f) + 1) * t) - s))) + b;
        return (c / 2 * (((t -= 2) * t * ((((s *= 1.525f) + 1) * t) + s)) + 2)) + b;
    }

    internal static float backEaseOutIn(float t, float b, float c, float d)
    {
        if (t < d / 2)
            return backEaseOut(t * 2, b, c / 2, d);
        return backEaseIn((t * 2) - d, b + (c / 2), c / 2, d);
    }
}
