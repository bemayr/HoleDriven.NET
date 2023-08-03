using Holedriven;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class SetAsync
    {
        public static async Task PrintAsync()
        {
            /*{|ExpectedDiagnosticLocation:*/await Hole.EffectAsync("/*description*/")/*|}*/;
        }
    }
}
