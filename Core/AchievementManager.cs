using Core;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dragon;

public class AchievementManager
{
    Dictionary<string, Achievement> dictionary = new();

    public static string steamUserName;
    public static AchievementManager current { get; set; }

    public AchievementManager()
    {
        current = this;
    }

    public void load<T>() where T : Achievement, new()
    {
        T achievement = new();

        bool unlocked = false;

        if (!string.IsNullOrEmpty(steamUserName))
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
        if (!string.IsNullOrEmpty(steamUserName))
        {
            string pchName = achievement.id.ToString();
            SteamUserStats.SetAchievement(pchName);
            SteamUserStats.StoreStats();
        }

        achievement.unlocked = true;
    }

    public static void update(Func<Achievement, bool> action)
    {
        if (current == null)
        {
            return;
        }

        foreach (Achievement achievement in current.dictionary.Values)
        {
            if (!achievement.unlocked && action(achievement))
            {
                current.unlock(achievement);
            }
        }
    }
}

public class Achievement
{
    public string id { get => GetType().Name; }
    public bool unlocked;

    public virtual bool takeDamage(Character self, Character enemy, int damage, bool critical) => false;
    public virtual bool addItem(Item item) => false;
    public virtual bool upgradeItem(Item item) => false;
    public virtual bool levelUp(Character character) => false;
    public virtual bool sell(PlayerProgress data, ItemNode itemNode, int price) => false;
}