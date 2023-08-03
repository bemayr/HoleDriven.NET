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

        [TestMethod]
        public async Task Hole_Triggers_Analyzer_Debug()
        {
            var description = "Some Testing Dummy Description";
            var source = $@"
        using System;
        using Holedriven;

        public class Program {{
            public static void Main() {{
                // Hole.Set
                {{|#1:Hole.Set(""{description}"")|}};

                // Hole.Get
                var three = {{|#2:Hole.Get(""adfasdf"", 3)|}};
            }}
        }}

        ";

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
                            .WithSeverity(DiagnosticSeverity.Info)
                            .WithArguments(description),
                        new DiagnosticResult(HoleAnalyzer.Rules.Get)
                            .WithLocation(2)
                            .WithSeverity(DiagnosticSeverity.Info)
                            .WithArguments("adfasdf")
                    },
                },
            };

            analyzerTest.SolutionTransforms.Add((solution, projectId) =>
            {
                return solution.WithProjectParseOptions(projectId,
                    new CSharpParseOptions()
                        .WithPreprocessorSymbols("DEBUG"));
            });

            analyzerTest.SolutionTransforms.Add((solution, projectId) =>
                solution.WithProjectCompilationOptions(
                    projectId,
                    new CSharpCompilationOptions(OutputKind.ConsoleApplication).WithOptimizationLevel(OptimizationLevel.Debug))
            );

            await analyzerTest.RunAsync();
        }

        [TestMethod]
        public async Task Hole_Triggers_Analyzer_Release()
        {
            var description = "Some Testing Dummy Description";
            var source = $@"
        using System;
        using Holedriven;

        public class Program {{
            public static void Main() {{
                // Hole.Set
                {{|#1:Hole.Set(""{description}"")|}};

                // Hole.Get
                var three = {{|#2:Hole.Get(""adfasdf"", 3)|}};
            }}
        }}

        ";

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
                            .WithSeverity(DiagnosticSeverity.Error)
                            .WithArguments(description),
                        new DiagnosticResult(HoleAnalyzer.Rules.Get)
                            .WithLocation(2)
                            .WithSeverity(DiagnosticSeverity.Error)
                            .WithArguments("adfasdf")
                    }
                },

            };

            analyzerTest.SolutionTransforms.Add((solution, projectId) =>
            {
                //return solution.WithProjectParseOptions(projectId,
                //    new CSharpParseOptions()
                //        .WithPreprocessorSymbols("RELEASE")
                //        .WithPreprocessorSymbols("HELLO"));

                return solution.WithProjectCompilationOptions(
                    projectId,
                    new CSharpCompilationOptions(OutputKind.ConsoleApplication).WithOptimizationLevel(OptimizationLevel.Release));
            });

            await analyzerTest.RunAsync();
        }
    }
}
