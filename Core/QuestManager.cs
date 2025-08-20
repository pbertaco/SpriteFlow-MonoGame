namespace Dragon;

public class QuestManager<T> where T : Quest, new()
{
    Dictionary<string, Quest> dictionary = new();

    Dictionary<Type, Type> parentByType = new();
    Dictionary<Type, Type> nextByType = new();
    HashSet<Type> registeredTypes = new();

    public QuestManager<T> registerQuestLine(List<Type> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            Type t = sequence[i];

            registeredTypes.Add(t);

            if (i > 0)
            {
                Type prev = sequence[i - 1];
                parentByType[t] = prev;
                nextByType[prev] = t;
            }
        }

        return this;
    }

    public void start(Type type)
    {
        setQuestStarted(type);
        load(type);
    }

    public void start<Q>() where Q : T, new() => start(typeof(Q));

    public void load<N>() where N : T, new()
    {
        load(typeof(N));
    }

    public void load(Type type)
    {
        string id = type.Name;

        if (dictionary.ContainsKey(id))
        {
            return;
        }

        T quest = (T)Activator.CreateInstance(type)!;

        quest.started = getQuestStarted(quest.id);
        quest.completed = getQuestCompleted(quest.id);

        if (quest.started && !quest.completed)
        {
            dictionary[quest.id] = quest;
        }
    }

    public void update(Func<T, bool> action)
    {
        List<Type> nextToAutoStart = new();

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

                    if (nextByType.TryGetValue(quest.GetType(), out Type nextType))
                    {
                        if (!getQuestCompleted(nextType) && !getQuestStarted(nextType))
                        {
                            nextToAutoStart.Add(nextType);
                        }
                    }
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

        foreach (Type t in nextToAutoStart)
        {
            setQuestStarted(t);
            load(t);
        }
    }

    public virtual void setQuestStarted(string id) { }
    public virtual void setQuestStarted(Type type) => setQuestStarted(type.Name);

    public virtual bool getQuestStarted(string id) => false;
    public virtual bool getQuestStarted(Type type) => getQuestStarted(type.Name);

    public virtual void setQuestCompleted(string id) { }
    public virtual void setQuestCompleted(Type type) => setQuestCompleted(type.Name);

    public virtual bool getQuestCompleted(string id) => false;
    public virtual bool getQuestCompleted(Type type) => getQuestCompleted(type.Name);

    public void loadAll()
    {
        foreach (Type t in registeredTypes)
        {
            if (getQuestStarted(t) && !getQuestCompleted(t))
            {
                load(t);
            }
        }
    }
}

public class Quest
{
    public string id { get => GetType().Name; }
    public bool started;
    public bool completed;
}