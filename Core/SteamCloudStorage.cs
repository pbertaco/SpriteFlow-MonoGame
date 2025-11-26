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

            // Download remote files that are newer than local
            for (int i = 0; i < remoteCount; i++)
            {
                string fileName = SteamRemoteStorage.GetFileNameAndSize(i, out int remoteSize);

                if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string localPath = Path.Combine(targetFolder, fileName);
                long remoteTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
                DateTime remoteTime = DateTimeOffset.FromUnixTimeSeconds(remoteTimestamp).UtcDateTime;

                if (!File.Exists(localPath) || File.GetLastWriteTimeUtc(localPath) < remoteTime)
                {
                    DownloadFile(fileName, localPath, remoteSize, remoteTime);
                }
            }

            // Upload local files that don't exist remotely or are newer
            foreach (string localPath in Directory.GetFiles(targetFolder, "*.bin"))
            {
                string fileName = Path.GetFileName(localPath);

                if (!SteamRemoteStorage.FileExists(fileName))
                {
                    Upload(fileName);
                }
                else
                {
                    long remoteTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
                    DateTime remoteTime = DateTimeOffset.FromUnixTimeSeconds(remoteTimestamp).UtcDateTime;
                    DateTime localTime = File.GetLastWriteTimeUtc(localPath);

                    if (localTime > remoteTime)
                    {
                        Upload(fileName);
                    }
                }
            }
        }
        catch
        {
            // Silently fail - game continues with local saves
        }
#else
        _ = targetFolder;
#endif
    }

    public static void Upload(string fileName)
    {
#if Steam
        if (!cloudEnabled)
        {
            return;
        }

        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            string localPath = DFileManager.path(fileName);
            
            if (!File.Exists(localPath))
            {
                return;
            }

            byte[] data = File.ReadAllBytes(localPath);
            SteamRemoteStorage.FileWrite(fileName, data, data.Length);
        }
        catch
        {
            // Silently fail
        }
#else
        _ = fileName;
#endif
    }

    public static void Delete(string fileName)
    {
#if Steam
        if (!cloudEnabled)
        {
            return;
        }

        try
        {
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
            // Silently fail
        }
#else
        _ = fileName;
#endif
    }

#if Steam
    static void DownloadFile(string fileName, string localPath, int remoteSize, DateTime remoteTimeUtc)
    {
        try
        {
            if (remoteSize <= 0)
            {
                remoteSize = SteamRemoteStorage.GetFileSize(fileName);
            }

            if (remoteSize <= 0)
            {
                return;
            }

            byte[] buffer = new byte[remoteSize];
            int bytesRead = SteamRemoteStorage.FileRead(fileName, buffer, remoteSize);

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
            // Silently fail
        }
    }
#endif
}
