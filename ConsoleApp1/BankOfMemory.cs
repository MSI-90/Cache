namespace ConsoleApp1;

public class BankOfMemory
{
    public Dictionary<string, Memory?> bank;
    public BankOfMemory() => bank = new Dictionary<string, Memory?>();
    public IEnumerable<string> GetItemsFromBank()
    {
        string s = string.Empty;
        foreach (var item in bank)
            s += item.Key + " : " + item.Value?.VolumeOfMemory+',';

        return s.Split(',');
    }
}
