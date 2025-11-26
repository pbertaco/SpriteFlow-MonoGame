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
                string fileName = SteamRemoteStorage.GetFileNameAndSize(i, out int remoteSize);

                if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string localPath = Path.Combine(targetFolder, fileName);
                long remoteTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
                DateTime remoteTime = DateTimeOffset.FromUnixTimeSeconds(remoteTimestamp).UtcDateTime;

                if (!File.Exists(localPath))
                {
                    DownloadFile(fileName, localPath, remoteSize, remoteTime);
                }
            }

            foreach (string localPath in Directory.GetFiles(targetFolder, "*.bin"))
            {
                string fileName = Path.GetFileName(localPath);

                if (!SteamRemoteStorage.FileExists(fileName))
                {
                    Upload(fileName);
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
        }
    }
#endif
}
