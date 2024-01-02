namespace Dragon;

static class RamdomExtension
{
    internal static int NextInt(this Random self)
    {
        return (int)Math.Round(self.NextDouble() * int.MaxValue);
    }

    internal static int NextInt(this Random self, int min, int max)
    {
        return (int)Math.Round((self.NextDouble() * (max - min)) + min);
    }

    internal static int NextInt(this Random self, int min, int max, float luck)
    {
        return (int)Math.Round((self.NextDouble(0, 1, luck) * (max - min)) + min);
    }

    internal static float NextFloat(this Random self)
    {
        return (float)self.NextDouble();
    }

    internal static float NextFloat(this Random self, float min, float max)
    {
        return (float)self.NextDouble(min, max);
    }

    internal static float NextFloat(this Random self, float min, float max, float luck)
    {
        return (float)self.NextDouble(min, max, luck);
    }

    internal static double NextDouble(this Random self, double min, double max)
    {
        return (self.NextDouble() * (max - min)) + min;
    }

    internal static double NextDouble(this Random self, double min, double max, double luck)
    {
        double scale = (luck - 0.5f) * 2;
        double minLuck = Math.Max(0, scale);
        double maxLuck = Math.Min(0, scale) + 1;
        return (self.NextDouble(minLuck, maxLuck) * (max - min)) + min;
    }
}
