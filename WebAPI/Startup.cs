using Backend;
using FRITeam.Swapify.APIWrapper;
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
        public const string DATABASENAME = "Swapify";
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            DbRegistration.Init();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            if (Environment.IsDevelopment())
            {
                services.AddSingleton(new MongoClient(Mongo2Go.MongoDbRunner.StartForDebugging().ConnectionString).GetDatabase(DATABASENAME));
            }

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStudyGroupService, StudyGroupService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<ISchoolScheduleProxy, SchoolScheduleProxy>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyOrigin()
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()
                                              .AllowCredentials());
            }

            env.ConfigureNLog($"nlog.{env.EnvironmentName}.config");

            // Serve index.html and static resources from wwwroot/
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapRoute("spa-fallback", "{*url}", new { controller = "Home", action = "RouteToReact" });
                });
            });
        }
    }
}
