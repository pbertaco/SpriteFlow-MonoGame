namespace Dragon;

internal class DConsole
{
    internal static bool enabled = false;

    static List<Type> types = new();

    internal static void WriteLine(object sender, string text)
    {
        if (!enabled)
        {
            return;
        }

        Type type = sender.GetType();
        bool allow = false;

        foreach (Type i in types)
        {
            if (i.IsAssignableFrom(type))
            {
                allow = true;
                break;
            }
        }

        if (allow)
        {
            Console.WriteLine(text);
        }
    }

    internal static void enable(Type type)
    {
        if (types.Contains(type))
        {
            return;
        }

        types.Add(type);
    }

    internal static void disable(Type type)
    {
        if (!types.Contains(type))
        {
            return;
        }

        types.Remove(type);
    }
}
