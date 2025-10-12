namespace Dragon;

public class QuestManager<T> where T : Quest, new()
{
    Dictionary<string, Quest> dictionary = new();

    Dictionary<Type, HashSet<Type>> nextByType = new();

    Dictionary<Type, HashSet<Type>> parentsByType = new();

    HashSet<Type> registeredTypes = new();

    public QuestManager<T> register(Type type)
    {
        registeredTypes.Add(type);
        load(type);
        return this;
    }

    public QuestManager<T> register(List<Type> sequence)
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            Type t = sequence[i];

            registeredTypes.Add(t);
            load(t);

            if (i > 0)
            {
                Type prev = sequence[i - 1];
                addEdge(prev, t);
            }
        }

        return this;
    }

    public QuestManager<T> register(Dictionary<Type, IEnumerable<Type>> adjacency)
    {
        foreach (KeyValuePair<Type, IEnumerable<Type>> kv in adjacency)
        {
            registeredTypes.Add(kv.Key);
            load(kv.Key);

            foreach (Type child in kv.Value)
            {
                registeredTypes.Add(child);
                load(child);
                addEdge(kv.Key, child);
            }
        }

        return this;
    }

    public QuestManager<T> registerAndStartRoots(Dictionary<Type, IEnumerable<Type>> adjacency)
    {
        register(adjacency);
        startRoots();
        return this;
    }

    public QuestManager<T> connect(Type from, Type to)
    {
        registeredTypes.Add(from);
        registeredTypes.Add(to);
        load(from);
        load(to);
        addEdge(from, to);
        return this;
    }

    void addEdge(Type from, Type to)
    {
        if (!nextByType.TryGetValue(from, out HashSet<Type> set))
        {
            set = new HashSet<Type>();
            nextByType[from] = set;
        }
        set.Add(to);

        if (!parentsByType.TryGetValue(to, out HashSet<Type> parents))
        {
            parents = new HashSet<Type>();
            parentsByType[to] = parents;
        }
        parents.Add(from);
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

                    if (nextByType.TryGetValue(quest.GetType(), out HashSet<Type> children))
                    {
                        foreach (Type nextType in children)
                        {
                            if (!completed(nextType) && !started(nextType))
                            {
                                bool canStart = true;
                                if (parentsByType.TryGetValue(nextType, out HashSet<Type> parents))
                                {
                                    foreach (Type parent in parents)
                                    {
                                        if (!completed(parent))
                                        {
                                            canStart = false;
                                            break;
                                        }
                                    }
                                }

                                if (canStart)
                                {
                                    nextToAutoStart.Add(nextType);
                                }
                            }
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

    public void startRoots()
    {
        foreach (Type type in registeredTypes)
        {
            bool hasParents = parentsByType.TryGetValue(type, out HashSet<Type> parents) && parents.Count > 0;
            if (!hasParents)
            {
                if (!started(type) && !completed(type))
                {
                    start(type);
                }
            }
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