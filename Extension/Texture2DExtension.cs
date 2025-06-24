namespace Dragon;

class Self
{
    public static List<string> textureNames = new();
    public static Dictionary<string, Texture2D> shadowDictionary = new();
}

static class Texture2DExtension
{

    public static Vector2 size(this Texture2D texture)
    {
        return new Vector2(texture.Width, texture.Height);
    }

    public static Texture2D shadow(this Texture2D self)
    {
        string key = self.Name;

        Texture2D texture;

        if (Self.shadowDictionary.ContainsKey(key))
        {
            texture = Self.shadowDictionary[key];
        }
        else
        {
            int width = self.Width;
            int height = self.Height;
            texture = new Texture2D(self.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            self.GetData(data);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int i = x + (y * width);
                    Color color = data[i];
                    color.R = color.A;
                    color.G = color.A;
                    color.B = color.A;
                    data[i] = color;
                }
            }

            texture.SetData(data);
            Self.shadowDictionary.Add(key, texture);
        }

        return texture;
    }
    public static bool convert(this Texture2D self, IEnumerable<Color> colors)
    {
        if (Self.textureNames.Contains(self.Name))
        {
            return false;
        }

        bool converted = false;

        Color[] data = new Color[self.Width * self.Height];
        self.GetData(data);

        for (int i = 0; i < data.Length; i++)
        {
            Color color = data[i];

            if (color.A > 0)
            {
                Color convertedColor = color.convert(colors);

                if (convertedColor.R != color.R || convertedColor.G != color.G || convertedColor.B != color.B)
                {
                    convertedColor.A = color.A;
                    data[i] = convertedColor;
                    converted = true;
                }
            }
        }

        if (converted)
        {
            ;
        }

        self.SetData(data);

        Self.textureNames.Add(self.Name);

        return converted;
    }
}
