namespace Dragon;

public static class SteamCloudStorage
{
#if Steam
    static readonly TimeSpan TimestampTolerance = TimeSpan.FromSeconds(2);
#endif

    public static bool IsAvailable
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
        if (!IsAvailable)
        {
            return;
        }

        try
        {
            Directory.CreateDirectory(targetFolder);

            Dictionary<string, CloudFileInfo> remoteFiles = new(StringComparer.OrdinalIgnoreCase);
            int remoteCount = SteamRemoteStorage.GetFileCount();

            for (int i = 0; i < remoteCount; i++)
            {
                string remoteName = SteamRemoteStorage.GetFileNameAndSize(i, out int remoteSize);

                if (string.IsNullOrEmpty(remoteName) || !remoteName.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                DateTime remoteTime = GetUtcFromTimestamp(SteamRemoteStorage.GetFileTimestamp(remoteName));
                remoteFiles[remoteName] = new CloudFileInfo(remoteSize, remoteTime);

                string localPath = Path.Combine(targetFolder, remoteName);

                if (ShouldDownload(localPath, remoteTime))
                {
                    TryDownload(remoteName, localPath, remoteSize, remoteTime);
                }
            }

            foreach (string localPath in Directory.GetFiles(targetFolder, "*.bin"))
            {
                string localName = Path.GetFileName(localPath);
                DateTime localTime = File.GetLastWriteTimeUtc(localPath);

                if (!remoteFiles.TryGetValue(localName, out CloudFileInfo info))
                {
                    Upload(localPath);
                    continue;
                }

                if (localTime - info.TimestampUtc > TimestampTolerance)
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
        if (!IsAvailable)
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
            SteamRemoteStorage.FileWrite(fileName, data);
        }
        catch
        {
        }
#else
        _ = localPath;
#endif
    }

    public static void Delete(string filePath)
    {
#if Steam
        if (!IsAvailable)
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
        }
#else
        _ = filePath;
#endif
    }

#if Steam
    static bool ShouldDownload(string localPath, DateTime remoteTimeUtc)
    {
        if (!File.Exists(localPath))
        {
            return true;
        }

        DateTime localUtc = File.GetLastWriteTimeUtc(localPath);
        return remoteTimeUtc - localUtc > TimestampTolerance;
    }

    static void TryDownload(string remoteName, string localPath, int remoteSize, DateTime remoteTimeUtc)
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

    static DateTime GetUtcFromTimestamp(long timestamp)
    {
        if (timestamp <= 0)
        {
            return DateTime.MinValue;
        }

        return DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
    }

    readonly struct CloudFileInfo
    {
        public CloudFileInfo(int fileSize, DateTime timestampUtc)
        {
            FileSize = fileSize;
            TimestampUtc = timestampUtc;
        }

        public int FileSize { get; }
        public DateTime TimestampUtc { get; }
    }
#endif
}
