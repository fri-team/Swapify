using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorClient.Services
{
    public interface IWebStorage
    {
        Task SetAsync<T>(string key, T data);
    }
}
