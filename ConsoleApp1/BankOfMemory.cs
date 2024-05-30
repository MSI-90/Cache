using System.Collections.Concurrent;

namespace ConsoleApp1;

public class BankOfMemory
{
    internal ConcurrentDictionary<string, Memory?> Bank {  get; set; }
    public BankOfMemory() => Bank = new ConcurrentDictionary<string, Memory?>();
    public IEnumerable<string> GetItemsFromBank()
    {
        string s = string.Empty;
        foreach (var item in Bank)
            s += item.Key + " : " + item.Value?.VolumeOfMemory+',';

        return s.Split(',');
    }
}
