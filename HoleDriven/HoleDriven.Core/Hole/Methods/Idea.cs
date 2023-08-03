using System;

namespace Holedriven
{
    public static partial class Hole
    {
        public static void Idea(string description, Scope scope = Scope.Nearest)
        {
            ReportHole(nameof(Idea), description);
        }
    }

    public enum Scope
    {
        Nearest,
        NextLine
    }
}
