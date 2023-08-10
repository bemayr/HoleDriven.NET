using HoleDriven.Core;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class Refactor
    {
        public static double Pi =>
            /*{|ExpectedDiagnosticLocation:*/Hole.Refactor(
                "/*description*/",
                () => 3.1415)/*|}*/;
    }
}
