namespace Dragon;

public class QuestManager<T> where T : Quest, new()
{
    Dictionary<string, Quest> dictionary = new();

    Dictionary<Type, Type> nextByType = new();
    HashSet<Type> registeredTypes = new();

    public QuestManager<T> register(Type type)
    {
        return register(new List<Type> { type });
    }

    public QuestManager<T> register(List<Type> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            Type t = sequence[i];

            registeredTypes.Add(t);

            if (i > 0)
            {
                Type prev = sequence[i - 1];
                nextByType[prev] = t;
            }

            load(t);
        }

        return this;
    }

    public void load(Type type)
    {
        string id = type.Name;

        T quest = null;

        if (dictionary.ContainsKey(id))
        {
            quest = (T)dictionary[id];
        }
        else
        {
            quest = (T)Activator.CreateInstance(type);
        }


        quest.started = started(quest.id);
        quest.completed = completed(quest.id);

        if (!quest.completed)
        {
            dictionary[quest.id] = quest;
        }
    }

    public void update(Func<T, bool> action)
    {
        List<Type> nextToAutoStart = new();

        foreach (T quest in dictionary.Values)
        {
            if (quest.started && !quest.completed)
            {
                if (action(quest))
                {
                    quest.completed = true;
                    complete(quest.id);

                    if (nextByType.TryGetValue(quest.GetType(), out Type nextType))
                    {
                        if (!completed(nextType) && !started(nextType))
                        {
                            nextToAutoStart.Add(nextType);
                        }
                    }
                }
            }
        }

        foreach (Type type in nextToAutoStart)
        {
            start(type);
        }
    }

    public virtual void start(string id) { }
    public virtual bool started(string id) => false;
    public virtual void complete(string id) { }
    public virtual bool completed(string id) => false;

    public bool started(Type type) => started(type.Name);
    public bool completed(Type type) => completed(type.Name);

    public void start(Type type)
    {
        start(type.Name);
        load(type);
    }

    public void complete(Type type)
    {
        complete(type.Name);
        load(type);
    }

}

public class Quest
{
    public string id { get => GetType().Name; }

    public bool started;

    public bool completed;
}