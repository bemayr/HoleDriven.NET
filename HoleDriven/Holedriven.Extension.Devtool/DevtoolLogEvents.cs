using Microsoft.Extensions.Logging;

namespace HoleDriven.Core.Logging
{
    internal static class DevtoolLogEvents
    {
        internal static EventId ClientConnected = new(2000, nameof(ClientConnected));
        internal static EventId ClientDisconnected = new(2001, nameof(ClientDisconnected));
    }
}
