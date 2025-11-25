namespace Dragon;

public struct SaveFileInfo
{
    public string fileName;
    public string fullPath;
    public DateTime lastModified;
    public long fileSize;
}

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
        string savesFolderPath = DFileManager.getFolderPath();
        string fileName = $"{DateTime.Now.ToString("yyyy-MM-dd--HH-mm-ss-fff")}.bin";
        string filePath = DFileManager.path(fileName);

        try
        {
            SteamCloudStorage.SynchronizeDirectory(savesFolderPath);
            File.WriteAllText(filePath, contents);
            success = true;
            SteamCloudStorage.Upload(filePath);
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

        SteamCloudStorage.SynchronizeDirectory(DFileManager.getFolderPath());

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
            SteamCloudStorage.SynchronizeDirectory(gameFolderPath);
            removeDuplicateFiles();
            files = Directory.GetFiles(gameFolderPath, $"*.bin");
        }
        else
        {
            Directory.CreateDirectory(gameFolderPath);
        }

        return files;
    }

    public List<SaveFileInfo> getSaveFileInfoList()
    {
        List<SaveFileInfo> saveInfoList = new();
        string[] files = getFiles();

        foreach (string file in files)
        {
            try
            {
                FileInfo fileInfo = new(file);
                SaveFileInfo info = new()
                {
                    fileName = Path.GetFileName(file),
                    fullPath = file,
                    lastModified = fileInfo.LastWriteTime,
                    fileSize = fileInfo.Length
                };
                saveInfoList.Add(info);
            }
            catch
            {
            }
        }

        saveInfoList.Sort((x, y) => y.lastModified.CompareTo(x.lastModified));
        return saveInfoList;
    }

    public bool tryGetSaveData(string fileName, out T saveData)
    {
        saveData = default;
        string file = DFileManager.path(fileName);

        if (!File.Exists(file))
        {
            return false;
        }

        try
        {
            string json = File.ReadAllText(file);
            saveData = JsonSerializer.Deserialize<T>(json);
            return saveData != null;
        }
        catch
        {
            return false;
        }
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
                    SteamCloudStorage.Delete(oldestFile);
                    Array.Resize(ref files, files.Length - 1);
                }
            }
        }

        return success;
    }

    void removeDuplicateFiles()
    {
        string gameFolderPath = DFileManager.getFolderPath();
        if (!Directory.Exists(gameFolderPath))
        {
            return;
        }

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
                    string duplicate = fileList[i];
                    File.Delete(duplicate);
                    SteamCloudStorage.Delete(duplicate);
                }
            }
        }
    }
}