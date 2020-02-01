using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace WebAPI
{
    public class StartupForTest : Startup
    {
        private const string DatabaseName = "SwapifyTest";

        public StartupForTest(IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory)
            : base(configuration, environment, loggerFactory)
        {
        }

        protected override ILogger<Startup> GetLogger(ILoggerFactory loggerFactory)
        {
            return loggerFactory.CreateLogger<StartupForTest>();
        }

        protected override string GetDatabaseName()
        {
            return DatabaseName;
        }
    }
}
