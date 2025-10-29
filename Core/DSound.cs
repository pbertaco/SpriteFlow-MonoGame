namespace Dragon;

public static class DSound
{
    private static readonly Dictionary<string, SoundEffect> content = new();

    public static bool loadFromStream = false;

    public static SoundEffect loadSoundEffect(string assetName, bool handleException = true)
    {
        if (string.IsNullOrEmpty(assetName))
            assetName = "missingAudio";

        if (content.TryGetValue(assetName, out SoundEffect cached))
            return cached;

        SoundEffect sound = null;

        try
        {
            if (loadFromStream)
            {
                string filePath = $"Content/SoundEffect/{assetName}.xnb";
#if macOS || Linux
                string preferred = Path.Combine(AppContext.BaseDirectory, "Content", "SoundEffect", $"{assetName}.xnb");
                string resolved = DFileManager.ResolvePathCaseInsensitive(AppContext.BaseDirectory, Path.Combine("Content", "SoundEffect", $"{assetName}.xnb"));
                filePath = File.Exists(preferred) ? preferred : (resolved ?? preferred);
#endif
                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
                sound = SoundEffect.FromStream(fileStream);
            }
            else
            {
                sound = DGame.current.Content.Load<SoundEffect>($"SoundEffect/{assetName}");
            }

            content[assetName] = sound;
        }
        catch (Exception e)
        {
            if (handleException)
            {
                DConsole.WriteLine(e, $"Content.Load<SoundEffect> error: {assetName}");
                sound = loadSoundEffect(null, false);
            }
        }
        return sound;
    }
}
