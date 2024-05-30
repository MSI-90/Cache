using System.Reflection.Metadata;

namespace ConsoleApp1;

public class Program
{
    static async Task Main(string[] args)
    {
        var cach = new Cache();
        cach.ActionNotify += cach.Notify;

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
                    Console.WriteLine(cach.GetList());
                    break;

                case "remove":
                    Console.WriteLine("Укажите ключ для удаления элемента: ");
                    string? key = Console.ReadLine();
                    cach.Remove(key);
                    break;

                default:
                    await cach.GetOrAdd(keyboardValue, MemoryFactory);
                    break;
            }
        }
    }

    static async Task<Memory> MemoryFactory()
    {
        return await new Memory().MemoryFactory();
    }
}
