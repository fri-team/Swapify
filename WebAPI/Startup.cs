using Backend;
using FRITeam.Swapify.APIWrapper;
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
using System.Text;
using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using FRITeam.Swapify.Backend.Settings;
using FRITeam.Swapify.Entities;
using Microsoft.Extensions.Options;
using System;
using WebAPI.Filters;
using FRITeam.Swapify.Backend.DbSeed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using FRITeam.Swapify.Backend.Exceptions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace WebAPI
{
    public class Startup
    {
        private const string DatabaseName = "Swapify";
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            DbRegistration.Init();
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");
            if (Environment.IsDevelopment())
            {
                _logger.LogInformation("Starting Mongo2Go");
                Mongo2Go.MongoDbRunner.StartForDebugging();
                MongoClientSettings settings = new MongoClientSettings();
                settings.GuidRepresentation = GuidRepresentation.Standard;

                services.AddSingleton(new MongoClient(settings).GetDatabase(DatabaseName));
            }

            LoadAndValidateSettings(services);
            ConfigureAuthorization(services);

            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IStudyGroupService, StudyGroupService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<ISchoolScheduleProxy, SchoolScheduleProxy>();
            services.AddSingleton<IEmailService>(
                new EmailService(services.BuildServiceProvider().GetService<IOptions<MailingSettings>>(),
                                 services.BuildServiceProvider().GetService<IOptions<UrlSettings>>()
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

                CreateDbSeed(app.ApplicationServices);
            }

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
            _logger.LogInformation("Validating settings");
            services.AddTransient<IStartupFilter, SettingValidationFilter>();

            var mailSettings = Configuration.GetSection("MailingSettings");
            if (mailSettings.Get<MailingSettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(MailingSettings)} configuration section.");
            var identitySettings = Configuration.GetSection("IdentitySettings");
            if (identitySettings.Get<IdentitySettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(IdentitySettings)} configuration section.");
            var pathSettings = Configuration.GetSection("PathSettings");
            if (pathSettings.Get<PathSettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(PathSettings)} configuration section.");

            services.Configure<MailingSettings>(mailSettings);
            services.Configure<IdentitySettings>(identitySettings);
            services.Configure<PathSettings>(pathSettings);
            services.Configure<EnvironmentSettings>(Configuration);
            //services.Configure<UrlSettings>(Configuration);

            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<MailingSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<IdentitySettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<EnvironmentSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<PathSettings>>().Value);
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            _logger.LogInformation("Configuring authorization");
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(typeof(ValidationFilter));
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
            _logger.LogInformation("Configuring identity");
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
            _logger.LogInformation("Generating JwtSecret");
            var secret = System.Environment.GetEnvironmentVariable("JwtSecret");
            var bytes = Encoding.ASCII.GetBytes(secret);
            return bytes;
        }

        private void CreateDbSeed(IServiceProvider serviceProvider)
        {
            _logger.LogInformation("Creating DB seed");
            try
            {
                _logger.LogInformation("Creating testing user");
                DbSeed.CreateTestingUser(serviceProvider);
                _logger.LogInformation("Oleg created");
                _logger.LogInformation("Creating courses");
                DbSeed.CreateTestingCourses(serviceProvider);
                _logger.LogInformation("Courses created");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception during creating DB seed :\n{e.Message}");
            }
        }
    }
}
