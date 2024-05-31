namespace ConsoleApp1
{
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
        
        public Cache()
        {
            BankOfMemory = new();
        }
        public async Task<TValue?> GetOrAdd(string key, Func<Task<TValue>> valueFactory)
        {
            if (BankOfMemory.Bank.ContainsKey(key))
            {
                TryGet(key, out TValue? value);
                InitializeEvent($"Найдено значение - {value} для ключа - {key}");
                return value;
            }

            var sss = await valueFactory();
            BankOfMemory?.Bank.TryAdd(key, sss);
            InitializeEvent($"Добавлено новое значение - {sss} для ключа - {key}");
            return sss;
        }

        public bool Remove(string key)
        {
            if (!TryGet(key, out TValue? _))
                return false;

            if (BankOfMemory.Bank.TryRemove(key, out _))
            {
                InitializeEvent($"Удалён элемент c атрибутами: ключ {key}\n");
                return true;
            }

            return false;
        }

        public bool TryGet(string key, out TValue? value)
        {
            if (!BankOfMemory.Bank.TryGetValue(key, out value))
            {
                InitializeEvent($"Нет элемента с ключём - {key}");
                return false;
            }
            
            return true;
        }

        public void GetList()
        {
            string fromConsole = string.Empty;

            Console.WriteLine("\nСписок всех элементов:");
            foreach (var item in BankOfMemory.GetItemsFromBank()!)
                fromConsole += item.ToString() + '\n';
            
            Console.WriteLine(fromConsole);
        } 

        public void Notify (string message)
        {
            Console.WriteLine(message);
        }
    }
}
