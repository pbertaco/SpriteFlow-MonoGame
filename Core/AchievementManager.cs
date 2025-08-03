namespace Dragon;

public class AchievementManager<T> where T : Achievement, new()
{
    Dictionary<string, Achievement> dictionary = new();

    public void load<N>() where N : T, new()
    {
        N achievement = new();

        bool unlocked = false;

        if (!string.IsNullOrEmpty(Achievement.steamUserName))
        {
            string pchName = achievement.id.ToString();
            SteamUserStats.GetAchievement(pchName, out unlocked);
        }

        achievement.unlocked = unlocked;

        if (!achievement.unlocked)
        {
            dictionary[achievement.id] = achievement;
        }
    }

    public void unlock(Achievement achievement)
    {
        if (!string.IsNullOrEmpty(Achievement.steamUserName))
        {
            string pchName = achievement.id.ToString();
            SteamUserStats.SetAchievement(pchName);
            SteamUserStats.StoreStats();
        }

        achievement.unlocked = true;
    }

    public void update(Func<T, bool> action)
    {
        foreach (T achievement in dictionary.Values)
        {
            if (!achievement.unlocked && action(achievement))
            {
                unlock(achievement);
            }
        }
    }
}