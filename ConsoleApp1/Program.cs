namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        var cach = new Cache<string>(15);
        cach.ActionNotify += cach.Notify;

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        //tokenSource.Cancel();

        //await cach.GetOrAdd("ddddd", async() => "Hello", token);
        //await cach.GetOrAdd("ddddd", async() => "Hello", token);
        //await cach.GetOrAdd("ffff", async () => "World", token);
        //await cach.GetOrAdd("555t5t", async () => "Сложная задача", token);
        //cach.GetList();

        //cach.Remove("ffff");
        //cach.GetList();
        //cach.GetTimesLifes();

        while (true)
        {
            Console.WriteLine("Введите ключ: ");
            var keyboardValue = Console.ReadLine();
            switch (keyboardValue)
            {
                case "exit":
                    return;

                case "list":
                    await Console.Out.WriteLineAsync("Список всех элементов:");
                    cach.GetList();
                    break;

                case "life":
                    await Console.Out.WriteLineAsync("Список всех элементов:");
                    cach.GetTimesLifes();
                    break;

                case "remove":
                    Console.WriteLine("Укажите ключ для удаления элемента: ");
                    string? key = Console.ReadLine();
                    cach.Remove(key);
                    break;
                
                default:
                    await cach.GetOrAdd(keyboardValue, MemoryFactory, token);
                    break;
            }
        }
    }
    static async Task<string> MemoryFactory()
    {
        return new string("feefefeefefef");
    }
}
