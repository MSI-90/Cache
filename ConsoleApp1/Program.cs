using System.Reflection.Metadata;

namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        var cach = new Cache<string>();
        cach.ActionNotify += cach.Notify;

        await cach.GetOrAdd("ddddd", async() => "Hello");
        await cach.GetOrAdd("ddddd", async() => "Hello");
        await cach.GetOrAdd("ffff", async () => "World");
        await cach.GetOrAdd("555t5t", async () => "Сложная задача");
        cach.GetList();

        cach.Remove("ffff");
        cach.GetList();
    }

}
