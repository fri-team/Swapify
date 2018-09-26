using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLog.Web;
using System;

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

            services.ConfigureMongoDbIdentity<User, MongoIdentityRole, Guid>(ConfigureIdentity());
            services.Configure<EmailSettings>(Configuration.GetSection("Mailing"));
            services.AddSingleton<IEmailService>(
                new EmailService(services.BuildServiceProvider().GetService<IOptions<EmailSettings>>()
                ));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseAuthentication();
        }

        private MongoDbIdentityConfiguration ConfigureIdentity()
        {
            return new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = DATABASENAME
                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = true;

                    options.SignIn.RequireConfirmedEmail = true;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // ApplicationUser settings
                    options.User.RequireUniqueEmail = true;
                }
            };
        }
    }
}
