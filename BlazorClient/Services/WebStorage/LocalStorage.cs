using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public class LocalStorage : ILocalStorage
    {
        private readonly Blazor.Extensions.Storage.LocalStorage _localStorage;

        public LocalStorage(Blazor.Extensions.Storage.LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            return await _localStorage.GetItem<T>(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _localStorage.RemoveItem(key);
        }

        public async Task SetAsync<T>(string key, T data)
        {
            await _localStorage.SetItem(key, data);
        }
    }
}
