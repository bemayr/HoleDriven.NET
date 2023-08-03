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
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);

            Configuration.ReportHoleEncountered(description, location);
            Configuration.ReportProvideAsyncStarted(description, id, task, location);
            var value = await task;
            Configuration.ReportProvideAsyncCompleted(description, value, id, task, location);

            return value;
        }
    }
}
