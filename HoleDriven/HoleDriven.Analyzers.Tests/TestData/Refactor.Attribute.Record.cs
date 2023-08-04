using HoleDriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    [Hole.Refactor("/*description*/")]
    public record /*{|ExpectedDiagnosticLocation:*/Refactor_Attribute_Record/*|}*/
    {
    }
}
