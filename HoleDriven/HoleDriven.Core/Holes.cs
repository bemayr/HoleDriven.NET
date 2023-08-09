﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core
{
    public interface IHolesConfiguration : IHolesCanSetLogger, IHolesExtendable { }

    public class HolesConfiguration : IHolesConfiguration
    {
        public Dependencies Dependencies => Dependencies.Instance;

        public IHolesExtendable SetLogger(ILoggerFactory loggerFactory)
        {
            Dependencies.SetLoggerFactory(loggerFactory);
            loggerFactory.CreateLogger("Some Category").LogError("inside here as well?");
            return this;
        }
    }

    public interface IHolesCanSetLogger
    {
        IHolesExtendable SetLogger(ILoggerFactory loggerFactory);
    }
    public interface IHolesExtendable
    {
        Dependencies Dependencies { get; }
    }
    public static class Holes
    {
        public static void Configure(Action<IHolesConfiguration> configureHoles) => configureHoles(new HolesConfiguration());
    }
}
