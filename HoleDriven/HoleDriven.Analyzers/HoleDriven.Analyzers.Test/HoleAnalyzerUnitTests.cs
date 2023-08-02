using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Threading.Tasks;
using VerifyCS = HoleDriven.Analyzers.Test.CSharpCodeFixVerifier<
    HoleDriven.Analyzers.HoleAnalyzer,
    HoleDriven.Analyzers.HoleDrivenAnalyzersCodeFixProvider>;

namespace HoleDriven.Analyzers.Test
{
    [TestClass]
    public class HoleAnalyzerUnitTests
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [TestMethod]
        public async Task HoleGet_TriggersWarning()
        {
            var source = @"
    using System;
    using Holedriven;

    internal class Program
    {
        static void Main(string[] args)
        {
            [|HD0001:Hole.Set(""adfasdf"")|];
        }
    }";
            // https://github.com/shuebner/OneOfDiagnosticSuppressor/blob/main/OneOfDiagnosticSuppressor.Tests/CompilationHelper.cs#L23
            var referenceAssemblies = ReferenceAssemblies.Default
                .AddAssemblies(ImmutableArray.Create(typeof(global::Holedriven.Hole).Assembly.Location));

            var test = new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { source },
                    AdditionalReferences =
                    {
                        MetadataReference.CreateFromFile(typeof(Holedriven.Hole).Assembly.Location)
                    },
                }
            };
            await test.RunAsync();
        }

        //Diagnostic and CodeFix both triggered and checked for
        //    [TestMethod]
        //    public async Task TestMethod2()
        //    {
        //        var test = @"
        //using System;
        //using System.Collections.Generic;
        //using System.Linq;
        //using System.Text;
        //using System.Threading.Tasks;
        //using System.Diagnostics;

        //namespace ConsoleApplication1
        //{
        //    class {|#0:TypeName|}
        //    {   
        //    }
        //}";

        //        var fixtest = @"
        //using System;
        //using System.Collections.Generic;
        //using System.Linq;
        //using System.Text;
        //using System.Threading.Tasks;
        //using System.Diagnostics;

        //namespace ConsoleApplication1
        //{
        //    class TYPENAME
        //    {   
        //    }
        //}";

        //        var expected = VerifyCS.Diagnostic("HoleDrivenAnalyzers").WithLocation(0).WithArguments("TypeName");
        //        await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        //    }
    }
}
