using System;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static void Effect(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            ReportHole(description, new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName));
            ReportEffectHappened(description);
        }

        internal static void ReportEffectHappened(string description) =>
            Console.WriteLine($"[⚡✳️ EFFECT]: {description}");
    }
}
