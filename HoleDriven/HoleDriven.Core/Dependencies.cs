using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;

namespace HoleDriven.Core
{
    public class Dependencies
    {
        #region Defaults
        private static readonly ILoggerFactory DefaultLoggerFactory = NullLoggerFactory.Instance;
        #endregion

        private static readonly Lazy<Dependencies> lazy = new Lazy<Dependencies>(
            () => new Dependencies());
        public static Dependencies Instance => lazy.Value;

        private Dependencies(ILoggerFactory loggerFactory = null)
        {
            LoggerFactory = loggerFactory ?? DefaultLoggerFactory;
        }

        public ILoggerFactory LoggerFactory { get; private set; }

        internal static void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (Instance.LoggerFactory != DefaultLoggerFactory)
                throw new InvalidOperationException($"{nameof(LoggerFactory)} has already been set");
            Instance.LoggerFactory = loggerFactory;
        }
    }
}
