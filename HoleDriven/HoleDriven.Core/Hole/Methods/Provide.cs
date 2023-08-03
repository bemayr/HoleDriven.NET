using System;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static TValue Provide<TValue>(
            string description,
            TValue value,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            ReportHole(description, location);
            ReportProvideHappened(value, location);
            return value;
        }

        internal static void ReportProvideHappened(object value, Core.HoleLocation location) =>
            Hole.Refactor(
                "🔴 make this configurable",
                () => Console.WriteLine($"[✨🎁🧧📦✉️📤🪄🔮💉 PROVIDE]: Value provided: '{value}'"));
    }
}
