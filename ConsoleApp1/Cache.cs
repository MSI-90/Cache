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
    public Cache() => BankOfMemory = new();

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
                var addLife = timeToLife?.TryAdd(key, DateTimeOffset.Now.ToUnixTimeSeconds());
                //if (addLife is true)
                //    InitializeEvent($"Время жизни - {timeToLife?[key]} для значения - {valueSource}");
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
        string fromConsole2 = string.Empty;

        Console.WriteLine("\nСписок всех элементов:");
        foreach (var item in BankOfMemory.GetItemsFromBank()!)
            fromConsole += item.ToString() + '\n';

        Console.WriteLine(new string('-', 30));

        foreach (var item in BankOfMemory.GetTimesToLifeForValues()!)
            fromConsole += item.ToString() + '\n';

        Console.WriteLine(fromConsole);
        Console.WriteLine(fromConsole2);
    } 

    public void Notify (string message)
    {
        Console.WriteLine(message);
    }
}
