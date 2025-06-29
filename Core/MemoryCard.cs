namespace Dragon;

public class SaveManager<T> where T : DSave, new()
{
    public static SaveManager<T> current { get; set; }

    public T data { get; set; }

    public SaveManager()
    {
        current = this;
    }

    public bool saveGame()
    {
        bool success = false;
        string contents = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

        try
        {
            File.WriteAllText(DFileManager.path($"{DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss-fff")}.bin"), contents);
            success = true;
            removeDuplicateFiles();
        }
        catch (Exception)
        {
        }

        return success;
    }

    public bool loadGame(string fileName)
    {
        bool success = false;
        string file = DFileManager.path(fileName);

        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);
            data = JsonSerializer.Deserialize<T>(json) ?? new T();
            data.updateModelVersion();
            success = true;
        }

        return success;
    }

    public string[] getFiles()
    {
        string gameFolderPath = DFileManager.getFolderPath();

        string[] files = [];

        if (Directory.Exists(gameFolderPath))
        {
            removeDuplicateFiles();
            files = Directory.GetFiles(gameFolderPath, $"*.bin");
        }
        else
        {
            Directory.CreateDirectory(gameFolderPath);
        }

        return files;
    }

    public bool continueGame()
    {
        bool success = false;
        string gameFolderPath = DFileManager.getFolderPath();

        if (Directory.Exists(gameFolderPath))
        {
            string[] files = getFiles(); ;

            if (files.Length > 0)
            {
                Array.Sort(files, (x, y) => File.GetLastWriteTime(y).CompareTo(File.GetLastWriteTime(x)));
                string lastFile = files[0];
                success = loadGame(lastFile);

                while (files.Length > 100)
                {
                    string oldestFile = files[files.Length - 1];
                    File.Delete(oldestFile);
                    Array.Resize(ref files, files.Length - 1);
                }
            }
        }

        return success;
    }

    void removeDuplicateFiles()
    {
        string gameFolderPath = DFileManager.getFolderPath();
        string[] files = Directory.GetFiles(gameFolderPath, $"*.bin");

        Dictionary<string, List<string>> hashToFiles = new();

        foreach (string file in files)
        {
            string hash = DFileManager.getFileStringHash(file);

            if (!hashToFiles.ContainsKey(hash))
            {
                hashToFiles[hash] = new List<string>();
            }

            hashToFiles[hash].Add(file);
        }

        foreach (KeyValuePair<string, List<string>> entry in hashToFiles)
        {
            List<string> fileList = entry.Value;

            if (fileList.Count > 1)
            {
                fileList.Sort((x, y) => File.GetLastWriteTime(y).CompareTo(File.GetLastWriteTime(x)));

                for (int i = 0; i < fileList.Count - 1; i++)
                {
                    File.Delete(fileList[i]);
                }
            }
        }
    }
}