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
using WebAPI.StartupFilters;

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

            LoadAndValidateSettings(services);

            services.AddSingleton<IEmailService>(
                new EmailService(services.BuildServiceProvider().GetService<IOptions<MailingSettings>>()
            ));
            services.ConfigureMongoDbIdentity<User, MongoIdentityRole, Guid>(ConfigureIdentity(
                Configuration.GetSection("IdentitySettings").Get<IdentitySettings>()));
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

        private void LoadAndValidateSettings(IServiceCollection services)
        {
            services.AddTransient<IStartupFilter, SettingValidationStartupFilter>();

            var mailSettings = Configuration.GetSection("MailingSettings");
            if (mailSettings.Get<MailingSettings>() == null)
                throw new ArgumentException($"Unable to load {nameof(MailingSettings)} configuration section.");
            var identitySettings = Configuration.GetSection("IdentitySettings");
            if (identitySettings.Get<IdentitySettings>() == null)
                throw new ArgumentException($"Unable to load {nameof(IdentitySettings)} configuration section.");

            services.Configure<MailingSettings>(mailSettings);
            services.Configure<IdentitySettings>(identitySettings);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<MailingSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<IdentitySettings>>().Value);
        }

        private MongoDbIdentityConfiguration ConfigureIdentity(IdentitySettings settings)
        {
            return new MongoDbIdentityConfiguration
            {
                MongoDbSettings = new MongoDbSettings //ToDo
                {
                    ConnectionString = "mongodb://localhost:27017",
                    DatabaseName = DATABASENAME
                },
                IdentityOptionsAction = options =>
                {
                    options.Password.RequireDigit = (bool)settings.RequireDigit;
                    options.Password.RequiredLength = (int)settings.RequiredLength;
                    options.Password.RequireNonAlphanumeric = (bool)settings.RequireNonAlphanumeric;
                    options.Password.RequireUppercase = (bool)settings.RequireUppercase;
                    options.Password.RequireLowercase = (bool)settings.RequireLowercase;

                    options.SignIn.RequireConfirmedEmail = (bool)settings.RequireConfirmedEmail;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes((int)settings.DefaultLockoutTimeSpan);
                    options.Lockout.MaxFailedAccessAttempts = (int)settings.MaxFailedAccessAttempts;

                    options.User.RequireUniqueEmail = (bool)settings.RequireUniqueEmail;
                }
            };
        }
    }
}
