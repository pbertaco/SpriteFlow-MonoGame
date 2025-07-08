using Core;

namespace Dragon;

public class AchievementManager
{
    Dictionary<AchievementID, Achievement> dictionary = new();

    public static string steamUserName;
    public static AchievementManager current { get; set; }

    public AchievementManager()
    {
        current = this;
    }

    public void register(Achievement achievement)
    {
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
    public AchievementID id;
    public bool unlocked;

    public virtual bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        return false;
    }
}

public class AchievementDefeatWarriorBumblebee : Achievement
{
    public AchievementDefeatWarriorBumblebee()
    {
        id = AchievementID.DefeatWarriorBumblebee;
    }

    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.WarriorBumblebee)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}