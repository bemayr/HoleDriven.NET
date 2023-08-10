using HoleDriven.Core.Types;
using System;
using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        public static void Effect(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Reporters.InvokeHoleEncountered(HoleType.Fake, location, description);
            Reporters.InvokeFakeHappened(null, location, description);
        }
    }
}
