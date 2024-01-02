namespace Dragon;

static class Vector2Extension
{
    internal static float distanceTo(this Vector2 self, Vector2 position)
    {
        return Vector2.Distance(self, position);
    }

    internal static Vector2 rotateBy(this Vector2 self, float rotation)
    {
        if (rotation == 0)
        {
            return self;
        }

        float sin = (float)Math.Sin(rotation);
        float cos = (float)Math.Cos(rotation);
        float x = (self.X * cos) - (self.Y * sin);
        float y = (self.X * sin) + (self.Y * cos);
        return new Vector2(x, y);
    }

    internal static float angleTo(this Vector2 self, Vector2 position)
    {
        return (float)Math.Atan2(position.Y - self.Y, position.X - self.X) + MathHelper.PiOver2;
    }

    internal static Vector2 directionTo(this Vector2 self, Vector2 position)
    {
        return Vector2.Normalize(position - self);
    }

    internal static Vector2 round(this Vector2 self)
    {
        return new Vector2((float)Math.Round(self.X), (float)Math.Round(self.Y));
    }

    internal static Vector2 fromIndex(this Vector2 self, int index)
    {
        return new Vector2(index % (int)self.X, index / (int)self.X);
    }
}
