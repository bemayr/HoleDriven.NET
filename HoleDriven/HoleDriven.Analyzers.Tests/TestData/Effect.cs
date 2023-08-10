using HoleDriven.Core;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class Effect
    {
        public static void Print()
        {
            /*{|ExpectedDiagnosticLocation:*/Hole.Effect("/*description*/")/*|}*/;
        }
    }
}
