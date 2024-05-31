namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        var cach = new Cache<string>();
        cach.ActionNotify += cach.Notify;

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        //tokenSource.Cancel();

        await cach.GetOrAdd("ddddd", async() => "Hello", token);
        await cach.GetOrAdd("ddddd", async() => "Hello", token);
        await cach.GetOrAdd("ffff", async () => "World", token);
        await cach.GetOrAdd("555t5t", async () => "Сложная задача", token);
        cach.GetList();

        cach.Remove("ffff");
        cach.GetList();
    }
}
