using HoleDriven.Core.Reporters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core
{
    public interface IHolesConfiguration : IHolesCanSetLogger, IHolesExtendable { }

    public class HolesConfiguration : IHolesConfiguration
    {
        public IConfiguration Configuration => Core.Configuration.Instance;

        public IHolesExtendable SetLogger(ILoggerFactory loggerFactory)
        {
            Dependencies.SetLoggerFactory(loggerFactory);
            return this;
        }

        public IHolesExtendable SetReporters(ILoggerFactory loggerFactory)
        {
            Dependencies.SetLoggerFactory(loggerFactory);
            return this;
        }
    }

    public interface IHolesCanSetLogger
    {
        IHolesExtendable SetLogger(ILoggerFactory loggerFactory);
    }
    public interface IHolesExtendable
    {
        IConfiguration Configuration { get; }
    }
    public static class Holes
    {
        public static void Configure(Action<IHolesConfiguration> configureHoles) => configureHoles(new HolesConfiguration());

        public static IReporters Reporters => Core.Reporters.Reporters.Instance;
    }
}
