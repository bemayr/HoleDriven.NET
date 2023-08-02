using System;
using System.Runtime.CompilerServices;

namespace Holedriven
{
    public partial class Hole
    {
        public static void Set(
            string description,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            Console.WriteLine($"[🕳️]: {description} ({new { callerFilePath, callerLineNumber, callerMemberName }})");
        }
    }
}
