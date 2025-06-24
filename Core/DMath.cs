namespace Dragon;

public static class DMath
{
    public static float goldenRation = (1 + (float)Math.Sqrt(5)) / 2;

    public static float distance(float duration, float speed)
    {
        return duration * speed;
    }

    public static float duration(float distance, float speed)
    {
        return distance / speed;
    }

    public static float speed(float distance, float duration)
    {
        return distance / duration;
    }
}
