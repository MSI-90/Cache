using System.Collections.Concurrent;

namespace ConsoleApp1;

public class BankOfMemory<T>
{
    internal ConcurrentDictionary<string, T> Bank { get; set; } = [];
    internal ConcurrentDictionary<string, DateTime> TimeToLifeOfValue { get; set; } = [];
    
    public IEnumerable<string> GetItemsFromBank()
    {
        VerifyLifes();
        string s = string.Empty;

        if (!Bank.Any())
            s = "Список пуст";

        foreach (var item in Bank)
            s += item.Key + " : " + item.Value+',';

        return s.Split(',');
    }

    public IEnumerable<string> GetTimesToLifeForValues()
    {
        string s = string.Empty;
        foreach (var item in TimeToLifeOfValue)
            s += item.Key + " : " + VerifyLifes(item.Key) + ',';

        return s.Split(',');
    }

    public bool VerifyLifes()
    {
        foreach (var item in TimeToLifeOfValue)
        {
            if (item.Value < DateTime.Now)
            {
                TimeToLifeOfValue.TryRemove(item.Key, out _);
                Bank.TryRemove(item.Key, out _);
            } 
        }

        if (TimeToLifeOfValue.Count > 0)
            return true;
        return false;
    }

    public int VerifyLifes(string key)
    {
        if (VerifyLifes())
        {
            int value = TimeToLifeOfValue.Where(item => item.Key.Equals(key)).Select(item => (int)(item.Value - DateTime.Now).TotalSeconds).FirstOrDefault();
            return value;
        }

        return 0;
        
    }
}
