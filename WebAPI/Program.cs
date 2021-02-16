using FRITeam.Swapify.Backend.Exceptions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

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
                .UseSetting("https_port", "443")
                .Build();
        }

        //public static class KestrelServerOptionsExtensions
        //{
        //    public static void ConfigureEndpoints(this KestrelServerOptions options)
        //    {
        //        var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
        //        var environment = options.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();

        //        var endpoints = configuration.GetSection("HttpServer:Endpoints")
        //            .GetChildren()
        //            .ToDictionary(section => section.Key, section =>
        //            {
        //                var endpoint = new EndpointConfiguration();
        //                section.Bind(endpoint);
        //                return endpoint;
        //            });

        //        foreach (var endpoint in endpoints)
        //        {
        //            var config = endpoint.Value;
        //            var port = config.Port ?? (config.Scheme == "https" ? 443 : 80);

        //            var ipAddresses = new List<IPAddress>();
        //            if (config.Host == "localhost")
        //            {
        //                ipAddresses.Add(IPAddress.IPv6Loopback);
        //                ipAddresses.Add(IPAddress.Loopback);
        //            }
        //            else if (IPAddress.TryParse(config.Host, out var address))
        //            {
        //                ipAddresses.Add(address);
        //            }
        //            else
        //            {
        //                ipAddresses.Add(IPAddress.IPv6Any);
        //            }

        //            foreach (var address in ipAddresses)
        //            {
        //                options.Listen(address, port,
        //                    listenOptions =>
        //                    {
        //                        if (config.Scheme == "https")
        //                        {
        //                            var certificate = LoadCertificate();
                                    
        //                            listenOptions.UseHttps(certificate);
        //                        }
        //                    });
        //            }
        //        }
        //    }

        //    private static X509Certificate2 LoadCertificate()
        //    {
        //        var certificate = new X509Certificate2();
        //        certificate.PrivateKey = ;
        //        //certificate.PublicKey = ;
        //    }

        //}
    }
}
