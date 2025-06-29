

public static class StringExtensions
{
    public static string translate(this string text)
    {
        return DLabelNode.translate(text);
    }

    public static string translate(this string text, params object[] args)
    {
        return string.Format(text.translate(), args);
    }

    public static string format(this string text, params object[] args)
    {
        return string.Format(text, args);
    }
}