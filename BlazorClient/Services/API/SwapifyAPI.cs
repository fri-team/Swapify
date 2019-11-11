using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorClient.Services.API
{
    public class SwapifyApi: ISwapifyAPI
    {
        public SwapifyApi(HttpClient httpClient)
        {
            User = new UserEndpoints(httpClient);
        }

        public IUserEndpoints User { get; }
    }
}
