namespace ConsoleApp1;

public class Memory
{
    public byte VolumeOfMemory { get; set; }
    public async Task<Memory> MemoryFactory() 
    {
        return await Task.Run(() =>
        {
            return new Memory();
        });
    }
}
