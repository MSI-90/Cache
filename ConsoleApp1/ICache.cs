namespace ConsoleApp1;

public interface ICache<TValue>
{
    Task<TValue?> GetOrAdd(string key, Func<Task<TValue>> valueFactory, CancellationToken token);
    bool TryGet(string key, out TValue? value);
    bool Remove(string key);
}
