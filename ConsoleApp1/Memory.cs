namespace ConsoleApp1;

public class Memory
{
    internal byte VolumeOfMemory { get; set; }
    public async Task<Memory> MemoryFactory() 
    {
        var rand = new Random();
        return await Task.Run(() =>
        {
            return new Memory() ;
        });
    }
}
