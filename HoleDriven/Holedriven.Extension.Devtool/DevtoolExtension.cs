using HoleDriven;
using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.WebUtilities;
using Holedriven.Extension.Devtool.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Net;
using System.Linq;

namespace Holedriven.Extension.Devtool
{
    internal class Devtool
    {
        private Devtool(
            Uri frontendUri,
            Uri proxyUri,
            IHubContext<PromptHub> promptsHub)
        {
            FrontendUri = frontendUri;
            ProxyUri = proxyUri;
            PromptsHub = promptsHub;
            Uri = new Uri(QueryHelpers.AddQueryString(
                FrontendUri.AbsoluteUri,
                new Dictionary<string, string>() { { "connection", ProxyUri.AbsoluteUri } }));
        }

        public Uri FrontendUri { get; }
        public Uri ProxyUri { get; }
        public Uri Uri { get; }
        public IHubContext<PromptHub> PromptsHub { get; }
        internal static Devtool Instance { get; private set; }

        internal static void Create(
            Uri frontendUri,
            Uri proxyUri,
            IHubContext<PromptHub> promptsHub)
        {
            Debug.Assert(Instance == null, "we can not create the Devtool instance twice");
            Instance = new Devtool(frontendUri, proxyUri, promptsHub);
        }

        public void OpenFrontend() => new Process()
        {
            StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Uri.AbsoluteUri
            }
        }.Start();
    }

    public static class DevtoolExtension
    {
        public static void UseDevtool(
            this IHolesExtendable _,
            string frontendUri = "https://devtool.holedriven.net")
        {
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();
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

            using var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder
                .SetMinimumLevel(LogLevel.Trace)
                .AddConsole());


            host.Start();

            var server = host.Services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var proxyUri = new Uri(addressFeature.Addresses.First());
            var promptsHub = (IHubContext<PromptHub>)host.Services.GetService(typeof(IHubContext<PromptHub>));

            Debug.Assert(addressFeature.Addresses.Count > 0, "we expect to only bind to one address");
            Devtool.Create(new Uri(frontendUri), proxyUri, promptsHub);

            Console.WriteLine(Devtool.Instance.Uri);
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
