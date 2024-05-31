using System.Collections.Concurrent;

namespace ConsoleApp1;

public class BankOfMemory<T>
{
    internal ConcurrentDictionary<string, T> Bank { get; set; } = [];
    internal ConcurrentDictionary<string, long> TimeToLifeOfValue { get; set; } = [];
    
    public IEnumerable<string> GetItemsFromBank()
    {
        string s = string.Empty;
        foreach (var item in Bank)
            s += item.Key + " : " + item.Value+',';

        return s.Split(',');
    }

    public IEnumerable<string> GetTimesToLifeForValues()
    {
        string s = string.Empty;
        foreach (var item in TimeToLifeOfValue)
            s += item.Key + " : " + item.Value + ',';

        return s.Split(',');
    }
}
