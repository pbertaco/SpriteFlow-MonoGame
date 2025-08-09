namespace Dragon;

public class QuestManager<T> where T : Quest, new()
{
    Dictionary<string, Quest> dictionary = new();

    public void load<N>(bool start = false) where N : T, new()
    {
        N quest = new();

        quest.started = start || getQuestStarted(quest.id);
        quest.completed = getQuestCompleted(quest.id);

        if (quest.started && !quest.completed)
        {
            dictionary[quest.id] = quest;
        }
    }

    public void update(Func<T, bool> action)
    {
        foreach (T quest in dictionary.Values)
        {
            if (quest.started && quest.completed)
            {
                continue;
            }

            if (quest.started)
            {
                if (action(quest))
                {
                    quest.completed = true;
                    setQuestCompleted(quest.id);
                }
            }
            else
            {
                if (action(quest))
                {
                    quest.started = true;
                    setQuestStarted(quest.id);
                }
            }
        }
    }

    public virtual void setQuestStarted(string id) { }

    public virtual bool getQuestStarted(string id) => false;

    public virtual void setQuestCompleted(string id) { }

    public virtual bool getQuestCompleted(string id) => false;
}


public class Quest
{
    public string id { get => GetType().Name; }
    public bool started;
    public bool completed;
}