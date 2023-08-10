using HoleDriven.Core.Types;
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
            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);

            Reporters.InvokeHoleEncountered(HoleType.Fake, location, description);
            //Reporters.EffectAsyncStarted(description, id, effect, location);
            await effect;
            //Reporters.EffectAsyncCompleted(description, id, effect, location);
            Reporters.InvokeFakeHappened(id, location, description);
        }

        public static async Task EffectAsync(
            string description,
            SetAsyncResultProvider taskProvider,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var id = Guid.NewGuid();
            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            var task = taskProvider(new EffectAsyncInput(id, description));

            Reporters.InvokeHoleEncountered(HoleType.Fake, location, description);
            //Reporters.EffectAsyncStarted(description, id, task, location);
            await task;
            //Reporters.EffectAsyncCompleted(description, id, task, location);
            Reporters.InvokeFakeHappened(id, location, description);

        }
    }
}
