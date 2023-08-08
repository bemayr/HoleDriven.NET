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

namespace Holedriven.Extension.Devtool
{
    public static class Devtool
    {
        internal static IHubContext<PromptHub> PromptHubContext { get; private set; }

        public static void ActivateDevtool(
            this IExtensionMarker _,
            string devtoolUrl = "https://devtool.holedriven.net")
        {
            devtoolUrl = Hole.Provide("deploy devtool", "http://localhost:5173");

            IHostBuilder hostBuilder = Host.CreateDefaultBuilder();
            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.ConfigureKestrel(opts =>
                {
                    //opts.ListenAnyIP(0); // bind web server to random free port
                });
                webHostBuilder.ConfigureLogging(logging =>
                {
                    logging.AddFilter("Microsoft.AspNetCore.SignalR", LogLevel.Debug);
                    logging.AddFilter("Microsoft.AspNetCore.Http.Connections", LogLevel.Debug);
                });
                webHostBuilder.UseStartup<Startup>();
            });
            IHost host = hostBuilder.Build();

            PromptHubContext = (IHubContext<PromptHub>)host.Services.GetService(typeof(IHubContext<PromptHub>));

            host.Start();


            //Console.WriteLine("Checking addresses...");
            var server = host.Services.GetRequiredService<IServer>();
            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            foreach (var address in addressFeature.Addresses)
            {
                var uri = new Uri(address);
                var port = uri.Port;
                //Console.WriteLine($"Listing on [{address}]");
                //Console.WriteLine($"The port is [{port}]");

                var param = new Dictionary<string, string>() { { "connection", $"http://localhost:{port}" } };
                var newUrl = new Uri(QueryHelpers.AddQueryString(new Uri(devtoolUrl).AbsoluteUri, param));

                //Console.WriteLine(newUrl);
            }
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
                // remove custom logging
                services.AddLogging(c => c.ClearProviders());

                // add SignalR capabilities
                services.AddSignalR();
                services.AddCors();
            }

            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app)
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
