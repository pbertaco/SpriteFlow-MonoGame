namespace Dragon;

public class DConsole
{
    public static bool enabled = false;

    static List<Type> types = new();

    public static void WriteLine(object sender, string text)
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

    public static void enable(Type type)
    {
        if (types.Contains(type))
        {
            return;
        }

        types.Add(type);
    }

    public static void disable(Type type)
    {
        if (!types.Contains(type))
        {
            return;
        }

        types.Remove(type);
    }
}
