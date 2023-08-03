using Holedriven;

namespace HoleDriven.Analyzers.Tests.TestData.HD0001
{
    public class Get
    {
        public static void Print()
        {
            /*{|ExpectedDiagnosticLocation:*/Hole.Set("/*description*/")/*|}*/;
        }
    }
}
