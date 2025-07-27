namespace Dragon;

public abstract class Achievement
{
    public static string steamUserName;

    public string id { get => GetType().Name; }
    public bool unlocked;
}