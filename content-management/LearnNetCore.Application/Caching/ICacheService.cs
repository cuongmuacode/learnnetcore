namespace LearnNetCore.Application.Cache;

public interface ICacheService
{
    public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> findAsync);
    public void Remove(string key);
}
