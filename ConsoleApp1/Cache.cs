namespace ConsoleApp1;

public delegate void MyInfoDelegate(string message);
internal class Cache<TValue> : ICache<TValue>
{
    MyInfoDelegate? actionNotify = null;
    internal event MyInfoDelegate ActionNotify 
    { 
        add { actionNotify += value; } 
        remove { actionNotify -= value; }
    }
    public void InitializeEvent(string message) => actionNotify?.Invoke(message);
    private BankOfMemory<TValue> BankOfMemory { get; set; }
    private long timeToLife = default;
    public long TimeToLife
    {
        get => timeToLife; 
        set => timeToLife = value;
    }
    public Cache(int time)
    {
        TimeToLife = time;
        BankOfMemory = new();
    }

    public async Task<TValue?> GetOrAdd(string key, Func<Task<TValue>> valueFactory, CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            InitializeEvent($"Запрошена операция отмены для ключа - {key}");
            return default;
        }

        if (BankOfMemory.Bank.ContainsKey(key))
        {
            lock (this)
            {
                TryGet(key, out TValue? value);
                InitializeEvent($"Найдено значение - {value} для ключа - {key}");
                if (DateTime.Now > BankOfMemory.TimeToLifeOfValue[key])
                    Remove(key);

                return value;
            }
        }

        var valueSource = await valueFactory();
        
        lock (this)
        {
            var newItem = BankOfMemory?.Bank.TryAdd(key, valueSource);
            if (newItem is true)
            {
                InitializeEvent($"Добавлено новое значение - {valueSource} для ключа - {key}");
                var timeToLife = BankOfMemory?.TimeToLifeOfValue;
                var addLife = timeToLife?.TryAdd(key, DateTime.Now.AddSeconds(TimeToLife));
            }
      
            return valueSource;
        }
    }

    public bool Remove(string key)
    {
        lock(this)
        {
            if (!TryGet(key, out TValue? _))
                return false;

            if (BankOfMemory.Bank.TryRemove(key, out _))
            {
                InitializeEvent($"Удалён элемент c атрибутами: ключ {key}\n");
                BankOfMemory.TimeToLifeOfValue.TryRemove(key, out DateTime _);
                return true;
            }
        }
        
        return false;
    }

    public bool TryGet(string key, out TValue? value)
    {
        lock (this)
        {
            if (!BankOfMemory.Bank.TryGetValue(key, out value))
            {
                InitializeEvent($"Нет элемента с ключём - {key}");
                return false;
            }
        }
        
        return true;
    }

    public void GetList()
    {
        string fromConsole = string.Empty;
        Console.WriteLine("\nСписок всех элементов:");

        foreach (var item in BankOfMemory.GetItemsFromBank()!)
            fromConsole += item.ToString() + '\n';

        Console.WriteLine(new string('-', 30));
        Console.WriteLine(fromConsole);
    } 

    public void GetTimesLifes()
    {
        string fromConsole = string.Empty;
        Console.WriteLine("\nВремя жизни объектов:");

        if (BankOfMemory?.GetTimesToLifeForValues()?.Any() == true)
        {
            foreach (var item in BankOfMemory?.GetTimesToLifeForValues())
            {
                fromConsole += item.ToString() + '\n';
            }
        }
        else
            fromConsole = "Все объекты закончили свой срок жизни(";
            

        Console.WriteLine(new string('-', 30));
        Console.WriteLine(fromConsole);
    }

    public void Notify (string message)
    {
        Console.WriteLine(message);
    }
}
