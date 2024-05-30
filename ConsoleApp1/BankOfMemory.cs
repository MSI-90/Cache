using System.Collections.Concurrent;

namespace ConsoleApp1;

public class BankOfMemory<T>
{
    internal ConcurrentDictionary<string, T?> Bank {  get; set; }
    public BankOfMemory() => Bank = new ConcurrentDictionary<string, T?>();
    public IEnumerable<string> GetItemsFromBank()
    {
        string s = string.Empty;
        foreach (var item in Bank)
            s += item.Key + " : " + item.Value+',';

        return s.Split(',');
    }
}
