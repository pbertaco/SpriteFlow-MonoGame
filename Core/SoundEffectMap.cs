using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Dragon
{
    public static class SoundEffectMap
    {
        private static Dictionary<string, string> _map;
        private static bool _loaded = false;
        private static readonly string _jsonPath = Path.Combine("Content", "json", "SoundEffect.json");

        public static string GetMappedName(string assetName)
        {
            EnsureLoaded();
            if (string.IsNullOrEmpty(assetName))
                return null;
            if (_map != null && _map.TryGetValue(assetName, out string mapped))
                return mapped;
            return assetName;
        }

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            if (!File.Exists(_jsonPath))
            {
                _map = null;
                _loaded = true;
                return;
            }
            string json = File.ReadAllText(_jsonPath);
            _map = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            _loaded = true;
        }
    }
}
