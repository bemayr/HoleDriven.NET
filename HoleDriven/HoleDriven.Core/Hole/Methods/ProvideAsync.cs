using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static Task<TValue> ProvideAsync<TValue>(
            string description,
            Task<TValue> value,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            Hole.Idea("🔴 the async variants might also provide start AND end events");
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            ReportHole(description, location);
            ReportProvideAsyncHappened(value, location);
            return value;
        }

        internal static void ReportProvideAsyncHappened(object value, Core.HoleLocation location) =>
            Hole.Refactor(
                "🔴 make this configurable and include the location",
                () => Console.WriteLine($"[✨🎁🧧📦✉️📤🪄🔮💉 PROVIDE]: Value provided asynchronously: '{value}'"));
    }
}
