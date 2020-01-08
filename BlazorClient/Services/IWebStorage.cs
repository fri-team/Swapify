using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public interface IWebStorage
    {
        Task SetAsync<T>(string key, T data);
    }
}
