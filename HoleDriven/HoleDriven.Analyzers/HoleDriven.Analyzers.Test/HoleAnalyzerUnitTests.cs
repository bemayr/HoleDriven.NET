using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.IO;
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
            [|Hole.Get:Hole.Set(""adfasdf"")|];
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

        private static DiagnosticResult GetRS1018ExpectedDiagnostic(int markupKey, string diagnosticId, string category, string format, string additionalFile) =>
            new DiagnosticResult(HoleAnalyzer.Rules.Get)
                .WithLocation(markupKey)
                .WithArguments(diagnosticId, category, format, additionalFile);

        [TestMethod]
        public async Task HoleGet_TriggersWarning_NoCSVerify()
        {
            var source = @"
        using System;
        using Holedriven;

        internal class Program
        {
            static void Main(string[] args)
            {
                {|#1:Hole.Set(""adfasdf"")|};
            }
        }";

            var analyzerTest = new CSharpAnalyzerTest<HoleAnalyzer, MSTestVerifier>
            {
                TestState =
                {
                    Sources = { source },
                    ReferenceAssemblies = new ReferenceAssemblies(
                        "net6.0",
                        new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"),
                        Path.Combine("ref", "net6.0")),
                    AdditionalReferences =
                    {
                        MetadataReference.CreateFromFile(typeof(Holedriven.Hole).Assembly.Location)
                    },
                    ExpectedDiagnostics =
                    {
                        new DiagnosticResult(HoleAnalyzer.Rules.Set)
                            .WithLocation(1)
                            .WithSeverity(DiagnosticSeverity.Warning)
                            .WithArguments("adfasdf")
                    }
                },
            };

            analyzerTest.SolutionTransforms.Add((solution, projectId) =>
            {
                return solution.WithProjectParseOptions(projectId,
                    new CSharpParseOptions()
                        .WithPreprocessorSymbols("DEBUG"));
            });

            await analyzerTest.RunAsync();
        }

    }
}
