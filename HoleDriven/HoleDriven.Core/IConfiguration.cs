using HoleDriven.Core.Reporters;
using Microsoft.Extensions.Logging;

namespace HoleDriven.Core
{
    public interface IConfiguration
    {
        ILoggerFactory LoggerFactory { get; }
        IReporters Reporters { get; }
    }
}