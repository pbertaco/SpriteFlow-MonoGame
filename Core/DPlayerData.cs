

public class DSave : ITimestampedSave
{
    public virtual string saveName { get; set; } = "";
    public virtual string saveTimestamp { get; set; } = "";

    public virtual void UpdateTimestamp()
    {
        saveTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public virtual void newGame()
    {
    }

    public virtual void updateModelVersion()
    {
    }
}
