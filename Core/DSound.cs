namespace Dragon;

public static class DSound
{
    private static Dictionary<string, SoundEffect> content;

    public static bool loadFromStream = false;

    public static SoundEffect loadSoundEffect(string assetName, bool handleException = true)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            assetName = "missingAudio";
        }

        string mappedName = SoundEffectMap.GetMappedName(assetName);

        if (content == null)
        {
            content = new Dictionary<string, SoundEffect>();
            DeleteUnmappedSoundEffectXnbFiles();
        }

        if (content.TryGetValue(assetName, out SoundEffect cached))
        {
            return cached;
        }

        SoundEffect sound = null;

        try
        {
            if (loadFromStream)
            {
                string filePath = $"Content/SoundEffect/{mappedName}.xnb";
#if macOS || Linux
                string preferred = Path.Combine(AppContext.BaseDirectory, "Content", "SoundEffect", $"{mappedName}.xnb");
                string resolved = DFileManager.ResolvePathCaseInsensitive(AppContext.BaseDirectory, Path.Combine("Content", "SoundEffect", $"{mappedName}.xnb"));
                filePath = File.Exists(preferred) ? preferred : (resolved ?? preferred);
#endif
                using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read);
                sound = SoundEffect.FromStream(fileStream);
            }
            else
            {
                sound = DGame.current.Content.Load<SoundEffect>($"SoundEffect/{mappedName}");
            }

            sound.Name = assetName + " -> " + mappedName;
            content[assetName] = sound;
        }
        catch (Exception e)
        {
            if (handleException)
            {
                Console.WriteLine($"Content.Load<SoundEffect> error: {mappedName}");
                sound = loadSoundEffect(null, false);
            }
        }

        return sound;
    }

    public static void DeleteUnmappedSoundEffectXnbFiles()
    {
        string dir = Path.Combine("Content", "SoundEffect");

        if (!Directory.Exists(dir)) return;

        string[] xnbFiles = Directory.GetFiles(dir, "*.xnb");

        HashSet<string> mappedNames = new HashSet<string>();
        System.Reflection.FieldInfo mapField = typeof(SoundEffectMap).GetField("_map", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Reflection.MethodInfo ensureLoadedMethod = typeof(SoundEffectMap).GetMethod("EnsureLoaded", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        ensureLoadedMethod.Invoke(null, null);
        Dictionary<string, string> map = mapField.GetValue(null) as Dictionary<string, string>;

        if (map != null)
        {
            foreach (string v in map.Values)
                mappedNames.Add(v);
        }

        foreach (string file in xnbFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);

            if (!mappedNames.Contains(fileName))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    ;
                }
            }
        }
    }
}
