namespace Dragon;

static class ColorExtensions
{
    public static Color multiply(this Color self, Color other)
    {
        int r = self.R * other.R / 255;
        int g = self.G * other.G / 255;
        int b = self.B * other.B / 255;
        return new Color(r, g, b);
    }

    public static Color convert(this Color self, IEnumerable<Color> colors)
    {
        int i = 0;
        Color color = new();
        double distance = int.MaxValue;

        foreach (Color value in colors)
        {
            double x = Math.Pow(self.R - value.R, 2) + Math.Pow(self.G - value.G, 2) + Math.Pow(self.B - value.B, 2);

            if (x < distance)
            {
                distance = x;
                color = value;
            }

            if (distance == 0)
            {
                break;
            }

            i++;
        }

        return color;
    }
}
