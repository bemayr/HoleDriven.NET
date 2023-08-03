using HoleDriven.Core;
using System;
using System.Diagnostics;

namespace Holedriven
{
    public static partial class Hole
    {
        [DebuggerHidden]
        public static void Throw(string description)
        {
            ReportHole(nameof(Throw), description);
            throw new HoleNotFilledException(description);
        }
    }
}
