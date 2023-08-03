using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Holedriven
{
    public static partial class Hole
    {
        public static void ReportHole(
            string holeType,
            string description,
            [CallerMemberName] string callerMemberName = null)
        {
            Console.WriteLine($"[🧩 {holeType}, {callerMemberName}]: {description}");
        }
    }
}
