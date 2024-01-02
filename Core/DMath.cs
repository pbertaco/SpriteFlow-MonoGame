namespace Dragon;

internal static class DMath
{
    internal static float goldenRation = (1 + (float)Math.Sqrt(5)) / 2;

    internal static float distance(float duration, float speed)
    {
        return duration * speed;
    }

    internal static float duration(float distance, float speed)
    {
        return distance / speed;
    }

    internal static float speed(float distance, float duration)
    {
        return distance / duration;
    }
}
