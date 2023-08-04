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

        public delegate Core.IEffectAsyncResult SetAsyncResultProvider(IEffectAsyncInput task);

        public interface IEffectAsyncInput
        {
            Guid Id { get; }
        }

        internal class EffectAsyncInputTask : IEffectAsyncInput
        {
            public Guid Id { get; }
            public EffectAsyncInputTask(Guid id) => Id = id;
        }

        public static async Task EffectAsync(
            string description,
            SetAsyncResultProvider resultProvider = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            resultProvider = resultProvider ?? (_ => new SetAsyncResultCompleted());

            var id = Guid.NewGuid();
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            var task = resultProvider(new EffectAsyncInputTask(id)).Task;

            Configuration.ReportHoleEncountered(description, location);
            Configuration.ReportEffectAsyncStarted(description, id, task, location);
            await task;
            Configuration.ReportEffectAsyncCompleted(description, id, task, location);
        }
    }
}
