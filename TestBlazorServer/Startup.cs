using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;

namespace BlazorTestClient.Server
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
            }

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<BlazorClient.Startup>();

            // application acts as proxy server for calls to WebAPI api endpoints
            app.MapWhen(IsApiCall, builder => builder.RunProxy(new ProxyOptions
            {
                Scheme = "http",
                Host = "localhost",
                Port = "5000"
            }));

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {                
                endpoints.MapFallbackToClientSideBlazor<BlazorClient.Startup>("index.html");
            });
        }

        private bool IsApiCall(HttpContext httpContext)
        {
            return httpContext.Request.Path.Value.StartsWith(@"/api", StringComparison.OrdinalIgnoreCase);
        }
    }
}
