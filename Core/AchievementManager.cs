using Core;

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

    public void load(Achievement achievement)
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

    public virtual bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        return false;
    }

    public virtual bool addItem(Item item, List<Item> list)
    {
        return false;
    }
}

public class AchievementAcquiredUncommonItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.uncommon;
    }
}

public class AchievementAcquiredRareItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.rare;
    }
}

public class AchievementAcquiredEpicItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.epic;
    }
}

public class AchievementAcquiredHeroicItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.heroic;
    }
}

public class AchievementAcquiredLegendaryItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.legendary;
    }
}

public class AchievementAcquiredSupremeItem : Achievement
{
    public override bool addItem(Item item, List<Item> list)
    {
        return item.rarity == Rarity.supreme;
    }
}

public class AchievementDefeatTheWarriorBumblebee : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.WarriorBumblebee)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheFireAlphaWolf : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.FireAlphaWolf)
        {
            return enemy.health <= 0;
        }
        return false;
    }
}

public class AchievementDefeatTheQueenSpider : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.QueenSpider)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheDemonLord : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.DemonLord)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheEliteBlueDragon : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.EliteBlueDragon)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheQueenSnake : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.QueenSnake)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheAnubis : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.Anubis)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheKingIceGolem : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.KingIceGolem)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheMammoth : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.Mammoth)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheCrystalCrab : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.CrystalCrab)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheMegaCrystalSlime : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.MegaCrystalSlime)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}

public class AchievementDefeatTheCorruptedCrystalElemental : Achievement
{
    public override bool takeDamage(Character self, Character enemy, int damage, bool critical)
    {
        if (enemy is Foe foe && foe.foeType == FoeType.CorruptedCrystalElemental)
        {
            return enemy.health <= 0;
        }

        return false;
    }
}