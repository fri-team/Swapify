using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using WebAPI;

namespace IntegrationTest
{
    public class TestFixture : IDisposable
    {
        public static readonly Uri BaseUrl = new Uri("http://localhost:5000/api/");
        public HttpClient Client { get; private set; }
        private readonly TestServer Server;

        public TestFixture()
        {
            var basePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "WebAPI");

            LoadLaunchSettings(basePath);

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(basePath)
                .UseConfiguration(config)
                .UseEnvironment("Development")
                .UseStartup<Startup>();

            Server = new TestServer(builder);
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Client.Dispose();
            Server.Dispose();
        }

        private static void LoadLaunchSettings(string basePath)
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
    }
}
