namespace Dragon;

public class DFileManager
{
    public static string getFolderPath()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

#if iOS
        string personalPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        documentsPath = Path.Combine(personalPath, "..", "Library");
#endif

        string gameFolderPath = Path.Combine(documentsPath, "SunnyHorizonGameStudio");

        return gameFolderPath;
    }

    public static string getFileStringHash(string filePath)
    {
        string content = File.ReadAllText(filePath, Encoding.UTF8);

        using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(content));
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }
    }

    public static string path(string fileName)
    {
        string gameFolderPath = getFolderPath();

        if (!Directory.Exists(gameFolderPath))
        {
            Directory.CreateDirectory(gameFolderPath);
        }

        return Path.Combine(gameFolderPath, fileName);
    }

    public static void loadObject<T>(string fileName) where T : new()
    {
        string filePath = path(fileName);

        if (File.Exists(filePath))
        {
            string json = decode(File.ReadAllText(filePath));
            JsonSerializer.Deserialize<T>(json);
        }
        else
        {
            new T();
        }
    }

    public static bool checkKey(string value)
    {
        return value == "" || DateTime.Parse(value) < DateTime.Now;
    }

    public static string encode(string text)
    {
        text = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        char[] list = text.ToCharArray();
        Array.Reverse(list);
        return new string(list);
    }

    static string decode(string text)
    {
        char[] list = text.ToCharArray();
        Array.Reverse(list);
        text = new string(list);
        return Encoding.UTF8.GetString(Convert.FromBase64String(text));
    }

    public static string ResolvePathCaseInsensitive(string baseDirectory, string relativePath)
    {
        if (string.IsNullOrEmpty(baseDirectory) || string.IsNullOrEmpty(relativePath))
        {
            return null;
        }

        string current = baseDirectory;
        string[] parts = relativePath.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);

        foreach (string part in parts)
        {
            if (!Directory.Exists(current))
            {
                return null;
            }

            string match = null;
            try
            {
                foreach (string entry in Directory.EnumerateFileSystemEntries(current))
                {
                    string name = Path.GetFileName(entry);
                    if (string.Equals(name, part, StringComparison.Ordinal))
                    {
                        match = entry;
                        break;
                    }
                    if (match == null && string.Equals(name, part, StringComparison.OrdinalIgnoreCase))
                    {
                        match = entry;
                    }
                }
            }
            catch
            {
                return null;
            }

            if (match == null)
            {
                return null;
            }

            current = match;
        }

        return current;
    }
}