using Backend;
using Backend.Config;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NLog.Web;
using System.Text;

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
            if (Environment.IsDevelopment())
            {
                services.AddSingleton(new MongoClient(Mongo2Go.MongoDbRunner.StartForDebugging().ConnectionString).GetDatabase(DATABASENAME));
            }

            services.Configure<EnvironmentConfig>(Configuration);
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new ProducesAttribute("application/json"));
            });
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(GetJwtSecret()),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStudentService, StudentService>();
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
            app.UseAuthentication();
            app.UseMvc();
            app.MapWhen(x => !x.Request.Path.Value.StartsWith("/api"), builder =>
            {
                builder.UseMvc(routes =>
                {
                    routes.MapRoute("spa-fallback", "{*url}", new { controller = "Home", action = "RouteToReact" });
                });
            });
        }

        private byte[] GetJwtSecret()
        {
            var secret = System.Environment.GetEnvironmentVariable("JwtSecret");
            var bytes = Encoding.ASCII.GetBytes(secret);
            return bytes;
        }
    }
}
