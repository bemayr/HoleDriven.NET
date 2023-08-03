using System;
using System.Runtime.CompilerServices;

namespace Holedriven
{
    public static partial class Hole
    {
        public static void Effect(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            ReportHole(nameof(Effect), $"[🕳️]: {description} ({new { callerFilePath, callerLineNumber, callerMemberName }})");
        }
    }
}
