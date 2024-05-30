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
        private BankOfMemory<TValue>? BankOfMemory { get; set; }
        
        public Cache()
        {
            BankOfMemory = new();
        }
        public async Task<TValue?> GetOrAdd(string key, Func<Task<TValue>> valueFactory)
        {
            TValue? value;
            if (TryGet(key, out value))
                return value;

            var sss = await valueFactory();
            BankOfMemory?.Bank.TryAdd(key, sss);
            InitializeEvent($"Добавлено новое значение - {sss.ToString()} для ключа - {key}");
            return sss;
        }

        public bool Remove(string key)
        {
            TValue? removedValue = default;
            TryGet(key, out removedValue);

            try
            {
                if ((bool)BankOfMemory?.Bank.TryRemove(key, out removedValue)!)
                {
                    InitializeEvent($"Удалён элемент c атрибутами: ключ {key}");
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool TryGet(string key, out TValue? value)
        {
            value = default;
            if ((bool)!BankOfMemory?.Bank.TryGetValue(key, out value)!)
            {
                InitializeEvent($"Нет элемента с ключём - {key}");
                return false;
            }
                
            //value = BankOfMemory?.Bank[key];
            return true;
        }

        public void GetList()
        {
            string fromConsole = string.Empty;
            
            foreach (var item in BankOfMemory?.GetItemsFromBank()!)
                fromConsole += item.ToString() + '\n';
            
            Console.WriteLine(fromConsole);
        } 

        public void Notify (string message)
        {
            Console.WriteLine(message);
        }
    }
}
