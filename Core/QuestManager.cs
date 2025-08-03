namespace Dragon;

public class QuestManager<T> where T : Quest, new()
{
    Dictionary<string, Quest> dictionary = new();

    public void load<N>() where N : T, new()
    {
        N quest = new();

        bool completed = false;

        completed = getQuestCompletionStatus(quest.id);

        quest.completed = completed;

        if (!quest.completed)
        {
            dictionary[quest.id] = quest;
        }
    }

    public void update(Func<T, bool> action)
    {
        foreach (T quest in dictionary.Values)
        {
            if (!quest.completed && action(quest))
            {
                setQuestCompleted(quest.id);
                quest.completed = true;
            }
        }
    }

    public virtual void setQuestCompleted(string id)
    {
    }

    public virtual bool getQuestCompletionStatus(string id)
    {
        return false;
    }
}