using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using WebAPI;

namespace IntegrationTest
{
    public class TestFixture : IDisposable
    {        
        public HttpClient Client { get; private set; }
        public Uri BaseUrl { get => new Uri("http://localhost:5000/api/"); }
        private readonly TestServer Server;

        public TestFixture()
        {
            const string relativePathToWebProject = @"..\..\..\..\..\WebAPI";
            string configPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, @"WebAPI\appsettings.json");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true)
                .Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(relativePathToWebProject)
                .UseConfiguration(config)
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
    }
}
