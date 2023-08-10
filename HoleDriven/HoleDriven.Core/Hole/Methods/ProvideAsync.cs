using HoleDriven.Core.Types;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static async Task<TValue> ProvideAsync<TValue>(
            string description,
            Task<TValue> task,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var id = Guid.NewGuid();
            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);

            Reporters.InvokeHoleEncountered(HoleType.Fake, location, description);
            //Reporters.ProvideAsyncStarted(description, id, task, location);
            var value = await task;
            //Reporters.ProvideAsyncCompleted(description, value, id, task, location);
            Reporters.InvokeFakeHappened(new { value, id }, location, description);

            return value;
        }
    }
}
