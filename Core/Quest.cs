namespace Dragon;

public abstract class Quest
{
    public string id { get => GetType().Name; }
    public bool completed;
}