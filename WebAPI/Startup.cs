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
using System.IO;
using WebAPI.Filters;
using FRITeam.Swapify.Backend.DbSeed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using FRITeam.Swapify.Backend.Exceptions;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using WebAPI.Models.DatabaseModels;

namespace WebAPI
{
    public class Startup
    {
        private const string DatabaseName = "SwapifyDB";
        private const string DatabaseNameDev = "Swapify";
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private readonly ILogger<Startup> _logger;
        private Mongo2Go.MongoDbRunner _runner;
        private String _absPathForFixedDB;
        private readonly string SwapifyCorsOrigins = "_swapifyCorsOrigins";

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            DbRegistration.Init();
            _logger = loggerFactory.CreateLogger<Startup>();
            _absPathForFixedDB = Path.GetFullPath("..\\Tests\\FixedDB");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");

            if (Environment.IsDevelopment())
            {
                _logger.LogInformation("Starting Mongo2Go");
                _runner = Mongo2Go.MongoDbRunner.StartForDebugging();
                MongoClientSettings settings = new MongoClientSettings();
                settings.GuidRepresentation = GuidRepresentation.Standard;

                services.AddSingleton(new MongoClient(settings).GetDatabase(DatabaseNameDev));

                services.AddCors(options =>
                {
                    options.AddPolicy("SwapifyCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://18.193.17.141",
                                            "http://swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk",
                                            "http://localhost:3000",
                                            "https://18.193.17.141",
                                            "https://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk",
                                            "https://localhost:3000")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });

                    options.AddPolicy("AllowCredentials",
                    builder =>
                    {
                        builder.WithOrigins("http://18.193.17.141",
                                            "http://swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk",
                                            "http://localhost:3000",
                                            "https://18.193.17.141",
                                            "https://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk",
                                            "https://localhost:3000")
                               .AllowCredentials();
                    });
                });
            }
            else
            {
                services.Configure<SwapifyDatabaseSettings>(Configuration.GetSection(nameof(SwapifyDatabaseSettings)));
                var settings = new MongoClientSettings
                {
                    Server = new MongoServerAddress("mongodb", 27017),
                    GuidRepresentation = GuidRepresentation.Standard
                };

                services.AddSingleton(new MongoClient(settings).GetDatabase(DatabaseName));
                services.AddSingleton<ISwapifyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<SwapifyDatabaseSettings>>().Value);

                services.AddCors(options =>
                {
                    options.AddPolicy("SwapifyCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://18.193.17.141",
                                            "http://swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk",
                                            "https://18.193.17.141",
                                            "https://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });

                    options.AddPolicy("AllowCredentials",
                    builder =>
                    {
                        builder.WithOrigins("http://18.193.17.141",
                                            "http://swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk",
                                            "https://18.193.17.141",
                                            "https://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk")
                               .AllowCredentials();
                    });
                });
            }


            services.ConfigureMongoDbIdentity<User, MongoIdentityRole, Guid>(ConfigureIdentity(
                Configuration.GetSection("IdentitySettings").Get<IdentitySettings>()));

            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<ISchoolScheduleProxy, SchoolScheduleProxy>();
            services.AddSingleton<ISchoolCourseProxy, SchoolCourseProxy>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IBlockChangesService, BlockChangesService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<INotificationService, NotificationService>();

            

            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            if (Environment.IsProduction())
            {
                services.AddSpaStaticFiles(configuration =>
                {
                    //configuration.RootPath = "/";
                    //configuration.RootPath = "WebApp/build";
                    configuration.RootPath = "wwwroot";
                });
            }

            LoadAndValidateSettings(services);
            ConfigureAuthorization(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                CreateDbSeedAsync(app.ApplicationServices);
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSpaStaticFiles();
            }

            app.UseCors("SwapifyCorsPolicy");
            app.UseCors("AllowCredentials");

            // Serve index.html and static resources from wwwroot/
            app.UseDefaultFiles();
            //app.UseHttpsRedirection(); // redirects to https when user puts url in browser, we don't have this now
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });


            // Do we even use this???
            /*
            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = "/";
                spa.Options.SourcePath = "wwwroot";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
            */
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

            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<MailingSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<IdentitySettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<EnvironmentSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<PathSettings>>().Value);

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            _logger.LogInformation("Configuring authorization");
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
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
            {
                configuration.MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = _runner.ConnectionString + DatabaseNameDev,
                    DatabaseName = DatabaseNameDev
                };
            }
            else
            {
                configuration.MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = "mongodb://mongodb:27017/" + DatabaseName,
                    DatabaseName = DatabaseName
                };
            }
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

        private async Task CreateDbSeedAsync(IServiceProvider serviceProvider)
        {
            _logger.LogInformation("Creating DB seed");
            try
            {
                _logger.LogInformation("Creating testing user");
                await DbSeed.CreateTestingUserAsync(serviceProvider);
                _logger.LogInformation("Oleg created");
                _logger.LogInformation("Creating courses");
                DbSeed.CreateTestingCourses(serviceProvider);
                _logger.LogInformation("Courses created");
                await DbSeed.CreateTestingExchangesAsync(serviceProvider);
                _logger.LogInformation("Testing exchanges created.");
                await DbSeed.CreateTestingNotifications(serviceProvider);
                _logger.LogInformation("Testing notifications created.");
                DbSeed.ImportTestDb(_runner, _absPathForFixedDB);
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception during creating DB seed :\n{e.Message}");
            }
        }
    }
}
