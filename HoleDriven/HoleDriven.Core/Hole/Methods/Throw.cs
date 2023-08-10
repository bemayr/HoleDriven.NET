using HoleDriven.Core.Types;
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
            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            var exception = new Core.HoleNotFilledException(description);

            Reporters.InvokeHoleEncountered(HoleType.NotImplemented, location, description);            
            throw exception;
        }
    }
}
