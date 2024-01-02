using System.Reflection;
using System.Text.Json;

namespace Dragon;

public class MemoryCard<T> where T : DPlayerData, new()
{
    internal static MemoryCard<T> current;

    internal T data;
    string fileName;

    public MemoryCard(string fileName)
    {
        current = this;
        loadGame(fileName);
    }

    internal void saveGame()
    {
        saveGame(fileName ?? DateTime.Now.ToString());
    }

    void saveGame(string newFileName)
    {
        fileName = newFileName;
        string contents = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path(fileName), contents);
    }

    internal void loadGame(string loadFileName)
    {
        fileName = loadFileName;
        string path = this.path(fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            data = (T)JsonSerializer.Deserialize(json, typeof(T));
            data.updateModelVersion();
        }
        else
        {
            data = new T();
            data.newGame();
        }
    }

    internal void delete(string fileName)
    {
        File.Delete(path(fileName));
    }

    internal string path(string fileName, string extension = "")
    {
        if (extension.Length > 0)
        {
            return Path.Combine(defaultStorageFolder.Value, $"{fileName}.{extension}");
        }
        else
        {
            return Path.Combine(defaultStorageFolder.Value, fileName);
        }
    }

    static readonly List<string> potentialStorageFolders = new()
    {
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        Path.Combine(Directory.GetCurrentDirectory(), "Documents")
    };

    static readonly Lazy<string> defaultStorageFolder = new(() =>
    {
        if (tryGetUWPFolder(out string folder))
        {
            return folder;
        }

        foreach (string potentialFolder in potentialStorageFolders)
        {
            if (tryGetFolder(() => potentialFolder, out folder))
            {
                return folder;
            }
        }

        throw new InvalidOperationException();
    });

    static bool tryGetUWPFolder(out string folder) => tryGetFolder(() =>
    {
        Type applicationData = Type.GetType("Windows.Storage.ApplicationData, Windows, Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime");

        if (applicationData == null)
        {
            return null;
        }

        PropertyInfo currentProperty = applicationData.GetProperty("Current", BindingFlags.Static | BindingFlags.Public);
        PropertyInfo localFolderProperty = applicationData.GetProperty("LocalFolder", BindingFlags.Public | BindingFlags.Instance);
        PropertyInfo pathProperty = localFolderProperty.PropertyType.GetProperty("Path", BindingFlags.Public | BindingFlags.Instance);
        object currentApplicationData = currentProperty.GetValue(null);
        object localFolder = localFolderProperty.GetValue(currentApplicationData);
        return (string)pathProperty.GetValue(localFolder);
    }, out folder);

    static bool tryGetFolder(Func<string> getter, out string folder)
    {
        try
        {
            string result = getter();

            if (result != null)
            {
                if (!Directory.Exists(result))
                {
                    Directory.CreateDirectory(result);
                }

                if (isDirectoryWritable(result))
                {
                    folder = result;
                    return true;
                }
            }
        }
        catch
        {
        }

        folder = null;
        return false;
    }

    static bool isDirectoryWritable(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        if (!Directory.Exists(path))
        {
            return false;
        }

        try
        {
            using (File.Create(Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose)) { }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
