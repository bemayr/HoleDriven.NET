using HoleDriven;
using System;
using System.Text;
using HoleDriven.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Holedriven.Extension.Devtool.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Net;
using System.Linq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Holedriven.Extension.Devtool
{

    public static class DevtoolExtension
    {
        public static void UseDevtool(
            this IHolesExtendable extendable,
            string frontendUri = "https://devtool.holedriven.net")
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();

            // TODO: this is how dependencies can be used in extensions
            var loggerFactory = extendable.Configuration.LoggerFactory;
            hostBuilder.ConfigureServices(services =>
            {
                // todo: validate that this works
                services.Replace(new ServiceDescriptor(typeof(ILoggerFactory), extendable.Configuration.LoggerFactory));
            });

            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.ConfigureKestrel(opts =>
                {
                    //opts.Listen(IPAddress.Loopback, 0); // bind web server to random free port
                });
                webHostBuilder.ConfigureLogging(logging =>
                {
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.None);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Trace);
                });
                webHostBuilder.UseStartup<Startup>();
            });
            IHost host = hostBuilder.Build();

            host.Start();

            var server = host.Services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var proxyUri = new Uri(addressFeature.Addresses.First());
            var promptsHub = (IHubContext<PromptHub>)host.Services.GetService(typeof(IHubContext<PromptHub>));

            Debug.Assert(addressFeature.Addresses.Count > 0, "we expect to only bind to one address");
            Devtool.Create(new Uri(frontendUri), proxyUri, promptsHub);

            Dependencies.Instance.LoggerFactory.CreateLogger(typeof(DevtoolExtension).FullName).LogInformation("Devtool started at {ProxyUri}", proxyUri);
        }

        public class Startup
        {
            public static void ConfigureServices(IServiceCollection services)
            {
                // remove custom logging
                services.AddLogging(c => c.ClearProviders());

                // add SignalR capabilities
                services.AddSignalR();
                services.AddCors();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public static void Configure(IApplicationBuilder app)
            {
                app.UseRouting();
                app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true) // allow any origin
                    .AllowCredentials()); // allow credentials
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<PromptHub>("/hub/prompts");
                });
            }
        }
    }
}
