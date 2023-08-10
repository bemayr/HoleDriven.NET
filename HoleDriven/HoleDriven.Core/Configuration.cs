using HoleDriven.Core.Reporters;
using Microsoft.Extensions.Logging;
using System;

namespace HoleDriven.Core
{
    public class Configuration : IConfiguration
    {
        private static readonly Lazy<Configuration> lazy = new Lazy<Configuration>(() => new Configuration());
        public static Configuration Instance => lazy.Value;

        public ILoggerFactory LoggerFactory => Dependencies.Instance.LoggerFactory;
        public IReporters Reporters => Core.Reporters.Reporters.Instance;
    }
}
