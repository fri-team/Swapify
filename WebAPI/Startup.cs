using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NLog.Web;

namespace WebAPI
{
    public class Startup
    {
        private const string DATABASENAME = "Swapify";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            // just for development, for production environment variables has to be used
            services.AddSingleton<IMongoDatabase>(
                new MongoClient(Mongo2Go.MongoDbRunner.StartForDebugging().ConnectionString)
                    .GetDatabase(DATABASENAME));
            services.AddSingleton<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            env.ConfigureNLog($"nlog.{env.EnvironmentName}.config");

            // Serve index.html and static resources from wwwroot/
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
