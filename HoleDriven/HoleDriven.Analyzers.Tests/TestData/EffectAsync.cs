using HoleDriven;
using System.Threading.Tasks;

namespace HoleDriven.Analyzers.Tests.TestData
{
    public class EffectAsync
    {
        public static async Task PrintAsync()
        {
            /*{|ExpectedDiagnosticLocation:*/await Hole.EffectAsync("/*description*/")/*|}*/;
        }
    }
}
