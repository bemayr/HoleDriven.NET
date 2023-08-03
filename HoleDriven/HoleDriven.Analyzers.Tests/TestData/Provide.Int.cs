using HoleDriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class Provide_Int
    {
        public static int Number => /*{|ExpectedDiagnosticLocation:*/Hole.Provide("/*description*/", 42)/*|}*/;
    }
}
