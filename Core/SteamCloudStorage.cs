namespace Dragon;

public static class SteamCloudStorage
{
    public static bool cloudEnabled
    {
        get
        {
#if Steam
            try
            {
                return SteamRemoteStorage.IsCloudEnabledForApp() && SteamRemoteStorage.IsCloudEnabledForAccount();
            }
            catch
            {
                return false;
            }
#else
            return false;
#endif
        }
    }

    public static void SynchronizeDirectory(string targetFolder)
    {
#if Steam
        if (!cloudEnabled)
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(targetFolder);
            int remoteCount = SteamRemoteStorage.GetFileCount();

            for (int i = 0; i < remoteCount; i++)
            {
                string remoteName = SteamRemoteStorage.GetFileNameAndSize(i, out int remoteSize);

                if (string.IsNullOrEmpty(remoteName) || !remoteName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string localPath = Path.Combine(targetFolder, remoteName);
                long remoteTimestamp = SteamRemoteStorage.GetFileTimestamp(remoteName);
                DateTime remoteTime = DateTimeOffset.FromUnixTimeSeconds(remoteTimestamp).UtcDateTime;

                if (!File.Exists(localPath))
                {
                    DownloadFile(remoteName, localPath, remoteSize, remoteTime);
                }
            }

            // Upload local files that don't exist remotely or are newer
            foreach (string localPath in Directory.GetFiles(targetFolder, "*.bin"))
            {
                string fileName = Path.GetFileName(localPath);

                if (!SteamRemoteStorage.FileExists(fileName))
                {
                    Upload(localPath);
                }
            }
        }
        catch
        {
        }
#else
        _ = targetFolder;
#endif
    }

    public static void Upload(string localPath)
    {
#if Steam
        if (!cloudEnabled)
        {
            return;
        }

        try
        {
            string fileName = Path.GetFileName(localPath);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(localPath))
            {
                return;
            }

            byte[] data = File.ReadAllBytes(localPath);
            SteamRemoteStorage.FileWrite(fileName, data, data.Length);
        }
        catch
        {
            DConsole.WriteLine(typeof(SteamCloudStorage), $"Failed to upload file: {localPath}");
        }
#else
        _ = localPath;
#endif
    }

    public static void Delete(string filePath)
    {
#if Steam
        if (!cloudEnabled)
        {
            return;
        }

        try
        {
            string fileName = Path.GetFileName(filePath);

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            if (SteamRemoteStorage.FileExists(fileName))
            {
                SteamRemoteStorage.FileDelete(fileName);
            }
        }
        catch
        {
            DConsole.WriteLine(typeof(SteamCloudStorage), $"Failed to delete file: {filePath}");
        }
#else
        _ = filePath;
#endif
    }

#if Steam
    static void DownloadFile(string remoteName, string localPath, int remoteSize, DateTime remoteTimeUtc)
    {
        try
        {
            if (remoteSize <= 0)
            {
                remoteSize = SteamRemoteStorage.GetFileSize(remoteName);
            }

            if (remoteSize <= 0)
            {
                return;
            }

            byte[] buffer = new byte[remoteSize];
            int bytesRead = SteamRemoteStorage.FileRead(remoteName, buffer, remoteSize);

            if (bytesRead <= 0)
            {
                return;
            }

            if (bytesRead != buffer.Length)
            {
                Array.Resize(ref buffer, bytesRead);
            }

            File.WriteAllBytes(localPath, buffer);

            if (remoteTimeUtc > DateTime.MinValue)
            {
                File.SetLastWriteTimeUtc(localPath, remoteTimeUtc);
            }
        }
        catch
        {
            DConsole.WriteLine(typeof(SteamCloudStorage), $"Failed to download file: {remoteName}");
        }
    }
#endif
}
