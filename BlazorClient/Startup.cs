using Blazor.Extensions.Storage;
using BlazorClient.Services;
using BlazorClient.Services.API;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();
            services.AddSingleton<IWebStorage, Services.LocalStorage>();
            services.AddSingleton<ISwapifyAPI, SwapifyApi>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
