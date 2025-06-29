namespace Dragon;

static class ListExtensions
{
    static Random r = new();

    public static T random<T>(this List<T> self)
    {
        if (self.Count == 1)
        {
            return self[0];
        }

        int i = r.NextInt(0, self.Count - 1);

        i = Math.Min(self.Count - 1, i);
        i = Math.Max(0, i);

        return self[i];
    }

    public static T random<T>(this List<T> self, float luck)
    {
        if (self.Count == 1)
        {
            return self[0];
        }

        int i = r.NextInt(0, self.Count - 1, luck);

        i = Math.Min(self.Count - 1, i);
        i = Math.Max(0, i);

        return self[i];
    }

    public static List<T> withAllValues<T>(this List<T> self) where T : Enum
    {
        self.AddRange((T[])Enum.GetValues(typeof(T)));
        return self;
    }

    public static void shuffle<T>(this List<T> self)
    {
        int n = self.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = r.Next(0, i + 1);
            T temp = self[i];
            self[i] = self[j];
            self[j] = temp;
        }
    }
}
