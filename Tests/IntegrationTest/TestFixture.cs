using FRITeam.Swapify.Backend.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using WebAPI;

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
#pragma warning disable S4142 // Duplicate values should not be passed as arguments
namespace IntegrationTest
{
    public class TestFixture : IDisposable
    {
        private const string EnviromentType = "DevelopmentVS";
        private const string AppsettingsFile = "appsettings.integrationtest.json";

        public Uri BaseUrl { get => new Uri("http://localhost:5000/api/"); }
        private readonly TestServer Server;

        public TestFixture()
        {
            Server = new TestServer(this.settupEnviroment());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public HttpClient CreateClient()
        {
            return Server.CreateClient();
        }

        protected virtual void Dispose(bool disposing)
        {
            Server.Dispose();
        }

        private IWebHostBuilder settupEnviroment() {
            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())
                                                         .Parent.Parent.Parent.Parent
                                                         .FullName, "WebAPI");
            LoadLaunchSettings(basePath);

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(AppsettingsFile, optional: true)
                .AddEnvironmentVariables()
                .Build();

            return new WebHostBuilder()
                .UseContentRoot(basePath)
                .UseConfiguration(config)
                .UseEnvironment(EnviromentType)
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    MockServices(services);
                });
        }

        private void LoadLaunchSettings(string basePath)
        {
            var settings = Path.Combine(basePath, "Properties", "launchSettings.json");
            using (var file = File.OpenText(settings))
            {
                var jObject = JObject.Load(new JsonTextReader(file));

                var environmentVariables = jObject["profiles"]["WebAPI"]["environmentVariables"]
                    .Children<JProperty>()
                    .ToList();

                foreach (var variable in environmentVariables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }

        private void MockServices(IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(IEmailService));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }
            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(x => x.SendConfirmationEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            services.AddSingleton(emailServiceMock.Object);
        }
    }
}
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
#pragma warning restore S4142 // Duplicate values should not be passed as arguments
