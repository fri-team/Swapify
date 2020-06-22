using FRITeam.Swapify.Backend.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;

namespace WebAPI
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // envinronement was '' (empty) and app was searching for nlog..config -> fix this...
            if (environment == "" || environment == null)
            {
                environment = "Production";
            }
            NLog.Logger logger = NLog.LogManager.LoadConfiguration($"nlog.{environment}.config").GetCurrentClassLogger();

            logger.Info($"Application starting in {environment} environment");
            try
            {
                BuildWebHost(args).Run();
                return 0;
            }
            catch (SettingException se)
            {
                logger.Error("Error when loading application settings:\n"
                    + $"{se.ConfigFileName} \n{se.Message}");
                return 1;
            }
            catch (Exception e)
            {
                logger.Fatal(e, $"Application terminated unexpectedly:\n{e}");
                return 1;
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .Build();
        }
    }
}
