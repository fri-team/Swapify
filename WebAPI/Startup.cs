using Backend;
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
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using System;
using WebAPI.StartupFilters;

namespace WebAPI
{
    public class Startup
    {
        private const string DatabaseName = "Swapify";
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
                services.AddSingleton(new MongoClient(Mongo2Go.MongoDbRunner.StartForDebugging().ConnectionString).GetDatabase(DatabaseName));
            }

            LoadAndValidateSettings(services);
            ConfigureAuthorization(services);            

            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<IEmailService>(
                new EmailService(services.BuildServiceProvider().GetService<IOptions<MailingSettings>>()
            ));
            services.ConfigureMongoDbIdentity<User, MongoIdentityRole, Guid>(ConfigureIdentity(
                Configuration.GetSection("IdentitySettings").Get<IdentitySettings>()));
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
            services.Configure<EnvironmentSettings>(Configuration);

            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<MailingSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<IdentitySettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<EnvironmentSettings>>().Value);
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
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
        }

        private MongoDbIdentityConfiguration ConfigureIdentity(IdentitySettings settings)
        {
            MongoDbIdentityConfiguration configuration = new MongoDbIdentityConfiguration();
            if (Environment.IsDevelopment())
                configuration.MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = Mongo2Go.MongoDbRunner.StartForDebugging().ConnectionString,
                    DatabaseName = DatabaseName
                };
            else
                configuration.MongoDbSettings = new MongoDbSettings();

            configuration.IdentityOptionsAction = options =>
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
            };
            return configuration;
        }

        private byte[] GetJwtSecret()
        {
            var secret = System.Environment.GetEnvironmentVariable("JwtSecret");
            var bytes = Encoding.ASCII.GetBytes(secret);
            return bytes;
        }
    }
}
