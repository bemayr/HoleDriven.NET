using HoleDriven.Core.Reporters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core
{
    public class HolesConfiguration : IHolesCanSetLogger, IHolesCanConfigureReporters
    {
        public IConfiguration Configuration => Core.Configuration.Instance;

        public IHolesCanConfigureReporters SetLogger(ILoggerFactory loggerFactory)
        {
            Dependencies.SetLoggerFactory(loggerFactory);
            return this;
        }

        public IHolesExtendable SetReporters(Action<IReporters> setReporters)
        {
            setReporters(Holes.Reporters);
            return this;
        }
    }

    public interface IHolesCanSetLogger : IHolesExtendable // entry interface
    {
        IHolesCanConfigureReporters SetLogger(ILoggerFactory loggerFactory);
    }
    public interface IHolesCanConfigureReporters : IHolesExtendable
    {
        IHolesExtendable SetReporters(Action<IReporters> reporters);
    }
    public interface IHolesExtendable
    {
        IConfiguration Configuration { get; }
    }
    public static class Holes
    {
        public static void Configure(Action<IHolesCanSetLogger> configureHoles) => configureHoles(new HolesConfiguration());

        public static IReporters Reporters => Core.Reporters.Reporters.Instance;
    }
}
