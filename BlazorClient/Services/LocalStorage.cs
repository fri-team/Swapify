using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazor.Extensions.Storage.Interfaces;

namespace BlazorClient.Services
{
    public class LocalStorage: IWebStorage
    {
        private readonly Blazor.Extensions.Storage.LocalStorage _localStorage;

        public LocalStorage(Blazor.Extensions.Storage.LocalStorage localStorage)
        {
            _localStorage = localStorage;
        }
        public async Task SetAsync<T>(string key, T data)
        {
            await _localStorage.SetItem(key, data);
        }
    }
}
