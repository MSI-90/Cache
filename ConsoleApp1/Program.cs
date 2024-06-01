namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        var firstThread = new Thread(FirstThread);
        firstThread.Start();
        Thread.Sleep(500);

        var twoThread = new Thread(TwoThread);
        twoThread.Start();
        Thread.Sleep(200);
        
        //тест через консоль)

        //while (true)
        //{
        //    Console.WriteLine("Введите ключ: ");
        //    var keyboardValue = Console.ReadLine();
        //    switch (keyboardValue)
        //    {
        //        case "exit":
        //            return;

        //        case "list":
        //            await Console.Out.WriteLineAsync("Список всех элементов:");
        //            cach.GetList();
        //            break;

        //        case "life":
        //            await Console.Out.WriteLineAsync("Список всех элементов:");
        //            cach.GetTimesLifes();
        //            break;

        //        case "remove":
        //            Console.WriteLine("Укажите ключ для удаления элемента: ");
        //            string? key = Console.ReadLine();
        //            cach.Remove(key);
        //            break;

        //        default:
        //            await cach.GetOrAdd(keyboardValue, MemoryFactory, token);
        //            break;
        //    }
        //}
    }
    //static async Task<string> MemoryFactory()
    //{
    //    return new string("feefefeefefef");
    //}

    static async void FirstThread()
    {
        var cach = new Cache<string>(10);
        cach.ActionNotify += cach.Notify;

        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        //tokenSource.Cancel();

        await cach.GetOrAdd("ddddd", async () => "Hello", token);
        await cach.GetOrAdd("ddddd", async () => "Hello", token);
        await cach.GetOrAdd("ffff", async () => "World", token);
        await cach.GetOrAdd("555t5t", async () => "Сложная задача", token);
        cach.GetList();
        cach.Remove("ffff");
        cach.GetList();
        cach.GetTimesLifes();
    }

    static async void TwoThread()
    {
        //cache2
        var cach2 = new Cache<int>(15);
        cach2.ActionNotify += cach2.Notify;
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;

        await cach2.GetOrAdd("ddddd", async () => 1255255, token);
        await cach2.GetOrAdd("ddddd", async () => 90291092, token);
        await cach2.GetOrAdd("ffff", async () => 94589895, token);
        await cach2.GetOrAdd("555t5t", async () => 1101010101, token);
        cach2.GetList();
        cach2.Remove("ffff");
        cach2.GetList();
        cach2.GetTimesLifes();
    } 
}
