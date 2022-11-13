using AspNetCore.Identity.MongoDbCore.Extensions;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using AspNetCore.Identity.MongoDbCore.Models;
using Backend;
using FRITeam.Swapify.APIWrapper;
using FRITeam.Swapify.Backend;
using FRITeam.Swapify.Backend.DbSeed;
using FRITeam.Swapify.Backend.Interfaces;
using FRITeam.Swapify.SwapifyBase.Entities;
using FRITeam.Swapify.SwapifyBase.Exceptions;
using FRITeam.Swapify.SwapifyBase.Settings;
using FRITeam.Swapify.SwapifyBase.Settings.ProxySettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Filters;
using WebAPI.Models.DatabaseModels;

namespace WebAPI
{
    public class Startup
    {
        private const string EnviromentDevVS = "DevelopmentVS";
        private const string ErrorHandlingPath = "/Error";

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private readonly ILogger<Startup> _logger;
        private Mongo2Go.MongoDbRunner _runner;
        private PathSettings _pathSettings;
        private SwapifyDatabaseSettings _swapifyDbSettings;
        private IdentitySettings _identitySettings;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            Environment = environment;
            DbRegistration.Init();
            _logger = loggerFactory.CreateLogger<Startup>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Configuring services");
            LoadAndValidateSettings(services);
            if (Environment.IsEnvironment(EnviromentDevVS))
            {
                _logger.LogInformation("Starting Mongo2Go");
                _runner = Mongo2Go.MongoDbRunner.StartForDebugging();
                MongoClientSettings settings = new MongoClientSettings
                {
                    GuidRepresentation = GuidRepresentation.Standard
                };
                services.AddSingleton(new MongoClient(settings).GetDatabase(_swapifyDbSettings.DatabaseName));
            }
            else
            {
                try
                {
                    Server = new MongoServerAddress("mongodb-stg", 27017),
                    GuidRepresentation = GuidRepresentation.Standard
                };
                services.AddSingleton(new MongoClient(settings).GetDatabase(_swapifyDbSettings.DatabaseName));
                services.AddSingleton<ISwapifyDatabaseSettings>(sp => sp.GetRequiredService<IOptions<SwapifyDatabaseSettings>>().Value);
            }
            if (Environment.IsProduction())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("SwapifyCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("https://swapify.fri.uniza.sk",
                                            "http://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                    options.AddPolicy("AllowCredentials",
                    builder =>
                    {
                        builder.WithOrigins("https://swapify.fri.uniza.sk",
                                            "http://swapify.fri.uniza.sk",
                                            "https://*swapify.fri.uniza.sk",
                                            "http://*swapify.fri.uniza.sk")
                               .AllowCredentials();
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("SwapifyCorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost",
                                            "https://localhost",
                                            "http://localhost:3000",
                                            "https://localhost:3000")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                    options.AddPolicy("AllowCredentials",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost",
                                            "https://localhost",
                                            "http://localhost:3000",
                                            "https://localhost:3000")
                               .AllowCredentials();
                    });
                });
            }
            services.ConfigureMongoDbIdentity<User, MongoIdentityRole, Guid>(ConfigureIdentity());
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<ICourseService, CourseService>();
            services.AddSingleton<ISchoolScheduleProxy, SchoolScheduleProxy>();
            services.AddSingleton<ISchoolCourseProxy, SchoolCourseProxy>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IBlockChangesService, BlockChangesService>();
            services.AddSingleton<IStudentService, StudentService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<ICalendarService, CalendarService>();
            services.AddControllersWithViews();
            if (!Environment.IsEnvironment(EnviromentDevVS))
            {
                services.AddSpaStaticFiles(configuration =>
                {
                    configuration.RootPath = _pathSettings.WwwRootPath;
                });
            }

            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swapify API", Version = "v1" });
            });
            ConfigureAuthorization(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "v1");
            });
            if (!env.IsEnvironment(EnviromentDevVS))
            {
                app.UseExceptionHandler(ErrorHandlingPath);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseSpaStaticFiles();
                if (env.IsDevelopment())
                {
                    CreateDbSeedAsync(app.ApplicationServices);
                }
                else if (env.IsProduction())
                {
                    CreateDbSeedAsyncProduction(app.ApplicationServices);
                }
            }
            else
            {
                app.UseDeveloperExceptionPage();
                CreateDbSeedAsync(app.ApplicationServices);
            }
            app.UseCors("SwapifyCorsPolicy");
            app.UseCors("AllowCredentials");
            // Serve index.html and static resources from wwwroot/
            app.UseDefaultFiles();
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
            app.UseSpa(spa =>
            {
                //spa.Options.SourcePath = "/";
                spa.Options.SourcePath = _pathSettings.WwwRootPath;
                if (env.IsEnvironment(EnviromentDevVS))
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
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
            _identitySettings = identitySettings.Get<IdentitySettings>();
            if (_identitySettings == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(IdentitySettings)} configuration section.");
            var pathSettings = Configuration.GetSection("PathSettings");
            _pathSettings = pathSettings.Get<PathSettings>();
            if (_pathSettings == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(PathSettings)} configuration section.");
            var swapifyDbSettings = Configuration.GetSection(nameof(SwapifyDatabaseSettings));
            _swapifyDbSettings = swapifyDbSettings.Get<SwapifyDatabaseSettings>();
            if (_swapifyDbSettings == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(SwapifyDatabaseSettings)} configuration section.");
            var recaptchaSettings = Configuration.GetSection("RecaptchaSettings");
            if (recaptchaSettings.Get<RecaptchaSettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(RecaptchaSettings)} configuration section.");
            var ldapSettings = Configuration.GetSection("LdapSettings");
            if (ldapSettings.Get<LdapSettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(LdapSettings)} configuration section.");
            var proxySettings = Configuration.GetSection(nameof(ProxySettings));
            if (proxySettings.Get<ProxySettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(ProxySettings)} configuration section.");
            var calendarSettings = Configuration.GetSection("CalendarSettings");
            if (calendarSettings.Get<CalendarSettings>() == null)
                throw new SettingException("appsettings.json", $"Unable to load {nameof(CalendarSettings)} configuration section.");
            services.Configure<MailingSettings>(mailSettings);
            services.Configure<IdentitySettings>(identitySettings);
            services.Configure<PathSettings>(pathSettings);
            services.Configure<EnvironmentSettings>(Configuration);
            services.Configure<RecaptchaSettings>(recaptchaSettings);
            services.Configure<LdapSettings>(ldapSettings);
            services.Configure<ProxySettings>(proxySettings);
            services.Configure<CalendarSettings>(calendarSettings);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<MailingSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<IdentitySettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<EnvironmentSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<PathSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<RecaptchaSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<LdapSettings>>().Value);
            services.AddSingleton<IValidatable>(resolver =>
                resolver.GetRequiredService<IOptions<CalendarSettings>>().Value);
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

        private MongoDbIdentityConfiguration ConfigureIdentity()
        {
            _logger.LogInformation("Configuring identity");
            MongoDbIdentityConfiguration configuration = new MongoDbIdentityConfiguration();
            if (Environment.IsEnvironment(EnviromentDevVS))
            {
                configuration.MongoDbSettings = new MongoDbSettings
                {
                    ConnectionString = _runner.ConnectionString + _swapifyDbSettings.DatabaseName,
                    DatabaseName = _swapifyDbSettings.DatabaseName
                };
            }
            else
            {
                configuration.MongoDbSettings = new MongoDbSettings
                {

                    ConnectionString = _swapifyDbSettings.ConnectionString + _swapifyDbSettings.DatabaseName,
                    DatabaseName = _swapifyDbSettings.DatabaseName
                };
                _logger.LogInformation($"Database connection string: {configuration.MongoDbSettings.ConnectionString} ");
                _logger.LogInformation($"Database name: {configuration.MongoDbSettings.DatabaseName} ");
            }
            configuration.IdentityOptionsAction = options =>
            {
                options.Password.RequireDigit = (bool)_identitySettings.RequireDigit;
                options.Password.RequiredLength = (int)_identitySettings.RequiredLength;
                options.Password.RequireNonAlphanumeric = (bool)_identitySettings.RequireNonAlphanumeric;
                options.Password.RequireUppercase = (bool)_identitySettings.RequireUppercase;
                options.Password.RequireLowercase = (bool)_identitySettings.RequireLowercase;
                options.SignIn.RequireConfirmedEmail = (bool)_identitySettings.RequireConfirmedEmail;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes((int)_identitySettings.DefaultLockoutTimeSpan);
                options.Lockout.MaxFailedAccessAttempts = (int)_identitySettings.MaxFailedAccessAttempts;
                options.User.RequireUniqueEmail = (bool)_identitySettings.RequireUniqueEmail;
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
                _logger.LogInformation("Creating users");
                await DbSeed.CreateTestingUserAsync(serviceProvider);
                _logger.LogInformation("Users created");
                _logger.LogInformation("Creating courses");
                await DbSeed.LoadTestingCoursesAsync(serviceProvider);
                _logger.LogInformation("Courses created");
                _logger.LogInformation("Creating exchanges");
                await DbSeed.CreateTestingExchangesAsync(serviceProvider);
                _logger.LogInformation("Exchanges created.");
                _logger.LogInformation("Creating notifications");
                await DbSeed.CreateTestingNotifications(serviceProvider);
                _logger.LogInformation("Notifications created.");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception during creating DB seed :\n{e.Message}");
            }
        }

        private async Task CreateDbSeedAsyncProduction(IServiceProvider serviceProvider)
        {
            _logger.LogInformation("Creating DB seed");
            try
            {
                _logger.LogInformation("Creating courses");
                await DbSeed.LoadTestingCoursesAsync(serviceProvider); //its not necessary to call this when database is already loaded
                _logger.LogInformation("Courses created");
            }
            catch (Exception e)
            {
                _logger.LogError($"Exception during creating DB seed :\n{e.Message}");
            }
        }
    }
}
