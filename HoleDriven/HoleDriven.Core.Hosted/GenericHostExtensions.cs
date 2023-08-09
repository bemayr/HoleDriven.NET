using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HoleDriven.Core.Hosted
{
    public static class GenericHostExtensions
    {
        public static void UseHoles(this IHost host)
        {
            var loggerFactory = host.Services.GetService<ILoggerFactory>();
            
        }
    }
}