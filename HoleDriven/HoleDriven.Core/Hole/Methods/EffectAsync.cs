using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static partial class Hole
    {
        public delegate Task SetAsyncResultProvider(EffectAsyncInput task);

        public class EffectAsyncInput
        {
            public Guid Id { get; }
            public string Description { get; }
            public EffectAsyncInput(Guid id, string description)
            {
                Id = id;
                Description = description;
            }
        }

        public static async Task EffectAsync(
            string description,
            Task effect = null,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            effect = effect ?? Task.CompletedTask;

            var id = Guid.NewGuid();
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);

            Report.HoleEncountered(description, location);
            Report.EffectAsyncStarted(description, id, effect, location);
            await effect;
            Report.EffectAsyncCompleted(description, id, effect, location);
        }

        public static async Task EffectAsync(
            string description,
            SetAsyncResultProvider taskProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var id = Guid.NewGuid();
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            var task = taskProvider(new EffectAsyncInput(id, description));

            Report.HoleEncountered(description, location);
            Report.EffectAsyncStarted(description, id, task, location);
            await task;
            Report.EffectAsyncCompleted(description, id, task, location);
        }
    }
}
