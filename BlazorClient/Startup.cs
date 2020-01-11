using Blazor.Extensions.Storage;
using BlazorClient.Services;
using BlazorClient.Services.API;
using BlazorClient.Services.IdentityManagement;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddStorage();
            services.AddScoped<ILocalStorage, Services.LocalStorage>();
            services.AddScoped<ISwapifyAPI, SwapifyApi>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITimetableService, TimetableService>();
            services.AddScoped<TimetableBlocksConverter>();
            services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            services.AddAuthorizationCore();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
