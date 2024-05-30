namespace ConsoleApp1
{
    public delegate CacheException MyInfoDelegate(string message);
    internal class Cache : ICache<Memory>
    {
        MyInfoDelegate actionNotify = null;
        public event MyInfoDelegate ActionNotify 
        { 
            add { actionNotify += value; } 
            remove { actionNotify -= value; }
        }
        public void InitializeEvent(string message) => actionNotify.Invoke(message);
        private BankOfMemory? BankOfMemory { get; set; }
        //private Memory? ValueOfMemory;
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

                BankOfMemory?.bank.Add(key, new Memory
                {
                    VolumeOfMemory = (byte)random.Next(byte.MinValue, byte.MaxValue)
                });
                InitializeEvent($"Добавлено новое значение {BankOfMemory?.bank[key]?.VolumeOfMemory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public bool Remove(string key)
        {
            if ((bool)BankOfMemory?.bank.ContainsKey(key)!)
            {
                try
                {
                    BankOfMemory?.bank.Remove(key);
                    InitializeEvent($"Удалён элемент");
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
            if ((bool)!BankOfMemory?.bank.ContainsKey(key)!)
                InitializeEvent($"Нет ключа - {key}");

            if ((bool)BankOfMemory?.bank.ContainsKey(key))
                InitializeEvent($"Уже имеется ключ в памяти - {key}");

            if (BankOfMemory?.bank[key] is null)
                InitializeEvent($"Нет элемента по данному ключу - {key}");
            
            value = BankOfMemory.bank[key];
            return true;
            throw new CacheException($"Найдено значение: {BankOfMemory.bank[key]} для ключа: {key}");
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
