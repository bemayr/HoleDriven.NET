using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoleDriven.Core
{
    internal static class HoleLogEvents
    {
        internal static EventId HoleEncountered = new EventId(1000, nameof(HoleEncountered));
    }
}
