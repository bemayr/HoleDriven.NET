using Holedriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class Get
    {
        public static int Number => /*{|ExpectedDiagnosticLocation:*/Hole.Provide("/*description*/", 42)/*|}*/;
    }
}
