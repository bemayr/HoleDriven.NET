using System.Runtime.CompilerServices;

namespace HoleDriven
{
    public static partial class Hole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Scope is only needed for the Source Analyzer")]
        public static void Idea(
            string description,
            Scope scope = Scope.Nearest,
            [CallerFilePath] string callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = int.MinValue,
            [CallerMemberName] string callerMemberName = null)
        {
            var location = new Core.HoleLocation(callerFilePath, callerLineNumber, callerMemberName);
            Configuration.ReportHoleEncountered(description, location);
        }
    }

    public enum Scope
    {
        Nearest,
        NextLine
    }
}
