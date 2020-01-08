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
            services.AddSingleton<ILocalStorage, Services.LocalStorage>();
            services.AddSingleton<ISwapifyAPI, SwapifyApi>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITimetableService, TimetableService>();
            services.AddSingleton<TimetableBlocksConverter>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
