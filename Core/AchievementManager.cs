namespace Dragon;

public class AchievementManager<T> where T : Achievement, new()
{
    Dictionary<string, Achievement> dictionary = [];

    public void load<N>() where N : T, new()
    {
        N achievement = new();

        if (dictionary.ContainsKey(achievement.id))
        {
            return;
        }

        bool unlocked = false;

#if Steam
        if (!string.IsNullOrEmpty(Achievement.steamUserName))
        {
            try
            {
                string pchName = achievement.id.ToString();
                SteamUserStats.GetAchievement(pchName, out unlocked);
            }
            catch
            {
            }
        }
#endif

        achievement.unlocked = unlocked;
        dictionary[achievement.id] = achievement;
    }

    public void unlock(Achievement achievement)
    {
#if Steam
        if (!string.IsNullOrEmpty(Achievement.steamUserName))
        {
            try
            {
                string pchName = achievement.id.ToString();
                SteamUserStats.SetAchievement(pchName);
                SteamUserStats.StoreStats();
            }
            catch
            {
            }
        }
#endif

        achievement.unlocked = true;
    }

    public void update(Func<T, bool> action)
    {
        foreach (T achievement in dictionary.Values)
        {
            if (achievement.unlocked)
            {
                continue;
            }

            if (action(achievement))
            {
                unlock(achievement);
            }
        }
    }
}