using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        [DebuggerHidden]
        public static void Throw(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Configuration.ReportHoleEncountered(description, location);
            
            throw new Core.HoleNotFilledException(description);
        }
    }
}
