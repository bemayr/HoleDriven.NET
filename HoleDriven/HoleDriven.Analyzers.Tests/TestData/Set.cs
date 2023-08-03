using Holedriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class Set
    {
        public static void Print()
        {
            /*{|ExpectedDiagnosticLocation:*/Hole.Effect("/*description*/")/*|}*/;
        }
    }
}
