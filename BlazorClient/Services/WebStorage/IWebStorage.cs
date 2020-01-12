using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public interface IWebStorage
    {
        Task SetAsync<T>(string key, T data);
        Task<T> GetAsync<T>(string key);
        Task RemoveAsync(string key);
    }
}
