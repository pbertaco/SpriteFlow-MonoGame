namespace Dragon;

public class DMusic
{
    public static DMusic current { get; set; }
    public static Song currentSong { get; set; }
    public static string currentKey { get; set; }
    public static bool loadFromStream { get; set; }

    public Song song;
    public string key;

    static float volumeScale = 0.1f;
    static float _volume;
    static bool _mediaAvailable = true;
    static bool _mediaProbed = false;

    static bool mediaAvailable()
    {
#if Windows || macOS || Linux
        if (_mediaProbed)
            return _mediaAvailable;

        _mediaProbed = true;

        try
        {
            var _ = MediaPlayer.State;
            _mediaAvailable = true;
        }
        catch (Exception)
        {
            _mediaAvailable = false;
        }
#endif

        return _mediaAvailable;
    }

    public static float volume
    {
        get => _volume / volumeScale;
        set
        {
            _volume = MathHelper.Clamp(value, 0f, 1f);
#if Windows || macOS || Linux
            if (mediaAvailable())
            {
                try
                {
                    MediaPlayer.Volume = _volume * volumeScale;
                }
                catch (Exception)
                {
                    _mediaAvailable = false;
                }
            }
#endif
        }
    }

    public DMusic(string assetName)
    {
        load(loadSong(assetName));
    }

    public DMusic(Song song)
    {
        load(song);
    }

    private void load(Song song)
    {
        this.song = song;
        current = this;
    }

    public void play(string key = "")
    {
        if (song != currentSong)
        {
            currentSong = song;

            if (song != null)
            {
                this.key = string.IsNullOrEmpty(key) ? song.Name : key;
                TimeSpan startPosition = TimeSpan.Zero;

                if (this.key == currentKey)
                {
#if Windows || macOS || Linux
                    if (mediaAvailable())
                    {
                        try 
                        {
                            startPosition = MediaPlayer.PlayPosition; 
                        }
                        catch (Exception)
                        {
                            _mediaAvailable = false;
                        }
                    }
#endif
                }

                currentKey = this.key;

#if Windows || macOS || Linux
                if (mediaAvailable())
                {
                    try
                    {
                        MediaPlayer.IsRepeating = true;
                        MediaPlayer.Play(song, startPosition);
                    }
                    catch (Exception)
                    {
                        _mediaAvailable = false;
                    }
                }
#endif
            }
        }
    }

    private static readonly Dictionary<string, Song> ContentCache = new();

    public static Song loadSong(string assetName)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            assetName = "missingAudio";
        }

        if (ContentCache.TryGetValue(assetName, out Song cachedSong))
        {
            return cachedSong;
        }

        try
        {
#if Windows || macOS || Linux
            if (!mediaAvailable())
            {
                return null;
            }
#endif
            Song song;

            if (loadFromStream)
            {
                string filePath = $"Content/Song/{assetName}.mp3";

#if macOS || iOS
                filePath = Path.Combine(AppContext.BaseDirectory, "Content", "mp3", $"{assetName}.mp3");
#endif
#if Linux
                string preferred = Path.Combine(AppContext.BaseDirectory, "Content", "Song", $"{assetName}.mp3");
                string resolved = DFileManager.ResolvePathCaseInsensitive(AppContext.BaseDirectory, Path.Combine("Content", "Song", $"{assetName}.mp3"));
                filePath = File.Exists(preferred) ? preferred : (resolved ?? preferred);
#endif

                using FileStream fileStream = new(filePath, FileMode.Open);
                song = Song.FromUri(assetName, new Uri(fileStream.Name));
            }
            else
            {
                song = DGame.current.Content.Load<Song>($"Song/{assetName}");
            }

            ContentCache[assetName] = song;
            return song;
        }
        catch (Exception e)
        {
            DConsole.WriteLine(e, $"Content.Load<Song> error: {assetName}");
            return null;
        }
    }
}
