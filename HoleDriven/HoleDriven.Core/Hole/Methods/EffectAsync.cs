using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static partial class Hole
    {
        internal class SetAsyncResultCompleted : Core.IEffectAsyncResult
        {
            public Task Task => Task.CompletedTask;
        }

        public delegate Core.IEffectAsyncResult SetAsyncResultProvider(string a);

        public static Task EffectAsync(
            string description,
            SetAsyncResultProvider resultProvider = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            resultProvider = resultProvider ?? (_ => new SetAsyncResultCompleted());

            ReportHole(description, new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName));
            ReportEffectAsyncHappened(description);

            return Hole.Refactor(
                description: "🔴 check whether this is needed and what it does",
                () => resultProvider("what the fuck does this string do").Task);
        }

        internal static void ReportEffectAsyncHappened(string description) =>
            Console.WriteLine($"[⚡✳️ EFFECT.ASYNC]: {description}");
    }
}
