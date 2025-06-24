namespace Dragon;

static class RamdomExtension
{
    public static int NextInt(this Random self)
    {
        return (int)Math.Round(self.NextDouble() * int.MaxValue);
    }

    public static int NextInt(this Random self, int min, int max)
    {
        return (int)Math.Round((self.NextDouble() * (max - min)) + min);
    }

    public static int NextInt(this Random self, int min, int max, float luck)
    {
        return (int)Math.Round((self.NextDouble(0, 1, luck) * (max - min)) + min);
    }

    public static float NextFloat(this Random self)
    {
        return (float)self.NextDouble();
    }

    public static float NextFloat(this Random self, float min, float max)
    {
        return (float)self.NextDouble(min, max);
    }

    public static float NextFloat(this Random self, float min, float max, float luck)
    {
        return (float)self.NextDouble(min, max, luck);
    }

    public static double NextDouble(this Random self, double min, double max)
    {
        return (self.NextDouble() * (max - min)) + min;
    }

    public static double NextDouble(this Random self, double min, double max, double luck)
    {
        double scale = (luck - 0.5f) * 2;
        double minLuck = Math.Max(0, scale);
        double maxLuck = Math.Min(0, scale) + 1;
        return (self.NextDouble(minLuck, maxLuck) * (max - min)) + min;
    }
}
