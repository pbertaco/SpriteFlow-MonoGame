namespace Dragon;

static class ListExtension
{
    static Random r = new();

    internal static T random<T>(this List<T> self)
    {
        float floatValue = r.NextFloat() * self.Count;
        int i = (int)floatValue;
        return self[i];
    }

    internal static T random<T>(this List<T> self, float luck)
    {
        float floatValue = r.NextFloat(0, 1, luck) * (self.Count - 1);    // float floatValue = r.NextFloat(0, 1, luck) * self.Count;  
        int i = (int)floatValue;

        return self[i];
    }

    internal static List<T> withAllValues<T>(this List<T> self) where T : Enum
    {
        self.AddRange((T[])Enum.GetValues(typeof(T)));
        return self;
    }
}
