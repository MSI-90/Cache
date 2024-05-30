namespace ConsoleApp1
{
    public delegate CacheException MyInfoDelegate(string message);
    internal class Cache : ICache<Memory>
    {
        MyInfoDelegate actionNotify = null;
        internal event MyInfoDelegate ActionNotify 
        { 
            add { actionNotify += value; } 
            remove { actionNotify -= value; }
        }
        public void InitializeEvent(string message) => actionNotify.Invoke(message);
        private BankOfMemory? BankOfMemory { get; set; }
        
        public Cache()
        {
            BankOfMemory = new();
            //ValueOfMemory = default;
        }
        public async Task<Memory?> GetOrAdd(string key, Func<Task<Memory>> valueFactory)
        {
            try
            {
                Memory? value;
                if (TryGet(key, out value))
                    return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                Random random = new Random();

                BankOfMemory?.Bank.TryAdd(key, new Memory
                {
                    VolumeOfMemory = (byte)random.Next(byte.MinValue, byte.MaxValue)
                });
                InitializeEvent($"Добавлено новое значение {BankOfMemory?.Bank[key]?.VolumeOfMemory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public bool Remove(string key)
        {
            Memory? removedValue = null;
            if ((bool)BankOfMemory?.Bank.TryRemove(key, out removedValue)!)
            {
                try
                {
                    InitializeEvent($"Удалён элемент со значением {removedValue?.ToString()}");
                    return true;
                }
                catch(Exception ex)
                { 
                    Console.WriteLine(ex.Message); 
                    return false; 
                }
            }

            InitializeEvent($"Нет ключа - {key}");
            return false;
        }

        public bool TryGet(string key, out Memory? value)
        {
            value = null;
            if ((bool)!BankOfMemory?.Bank.ContainsKey(key)!)
                InitializeEvent($"Нет ключа - {key}");

            if ((bool)BankOfMemory?.Bank.ContainsKey(key))
                InitializeEvent($"Уже имеется ключ в памяти - {key}");

            if (BankOfMemory?.Bank[key] is null)
                InitializeEvent($"Нет элемента по данному ключу - {key}");
            
            value = BankOfMemory.Bank[key];
            return true;
            throw new CacheException($"Найдено значение: {BankOfMemory.Bank[key]} для ключа: {key}");
        }

        public string GetList()
        {
            string fromConsole = string.Empty;
            
            foreach (var item in BankOfMemory?.GetItemsFromBank()!)
                fromConsole += item.ToString() + '\n';
            
            return fromConsole;
        } 

        public CacheException Notify (string message)
        {
            throw new CacheException(message);
        }
    }
}
