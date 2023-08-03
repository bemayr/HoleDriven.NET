using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Xunit.Abstractions;
using static HoleDriven.Analyzers.Tests.HoleAnalyzerTestsHelpers;

namespace HoleDriven.Analyzers.Tests
{
    public class HoleAnalyzerTests : HoleAnalyzerTestsBase
    {
        public HoleAnalyzerTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void Sourcecode_Preparation_Works()
        {
            // Arrange
            var source = @"
using Holedriven;

namespace HoleDriven.Analyzers.Tests
{
    public class Get
    {
        public static int Number => /*{|ExpectedDiagnosticLocation:*/Hole.Get(""/*description*/"", 42)/*|}*/;
    }
}";
            var expected = @"
using Holedriven;

namespace HoleDriven.Analyzers.Tests
{
    public class Get
    {
        public static int Number => {|#1:Hole.Get(""What is the answer to everything?"", 42)|};
    }
}";

            // Act
            var prepared = HoleAnalyzerTestsHelpers.PrepareSourcecode(source, "What is the answer to everything?");

            // Assert
            Log(source);
            Log(prepared);
            Assert.Equal(expected, prepared);
        }

        [Theory]
        [InlineData("TestData/Get.cs")]
        [InlineData("TestData/Set.cs")]
        public async Task Holes_are_Reported_as_Info_in_Debug_Mode(string sourcePath)
        {
            // Arrange
            var expectedDiagnosticDescriptor = HoleAnalyzer.Rules.Get;
            var holeDescription = "Some Test Description";
            var source = PrepareSourcecode(LoadSourceFile(sourcePath), holeDescription);

            // Act
            var test = CreateTest(
                source,
                expectedDiagnosticDescriptor,
                holeDescription,
                DiagnosticSeverity.Info,
                OptimizationLevel.Debug);

            // Assert
            await test.RunAsync();
        }

        [Theory]
        [InlineData("TestData/Get.cs")]
        [InlineData("TestData/Set.cs")]
        public async Task Holes_are_Reported_as_Error_in_Release_Mode(string sourcePath)
        {
            // Arrange
            var expectedDiagnosticDescriptor = HoleAnalyzer.Rules.Get;
            var holeDescription = "Some Test Description";
            var source = PrepareSourcecode(LoadSourceFile(sourcePath), holeDescription);

            // Act
            var test = CreateTest(
                source,
                expectedDiagnosticDescriptor,
                holeDescription,
                DiagnosticSeverity.Error,
                OptimizationLevel.Release);

            // Assert
            await test.RunAsync();
        }
    }

    internal static class HoleAnalyzerTestsHelpers
    {
        private const int DIAGNOSTIC_LOCATION = 1;
        public static string PrepareSourcecode(string source, string description) => source
                .Replace("/*{|ExpectedDiagnosticLocation:*/", $"{{|#{DIAGNOSTIC_LOCATION}:")
                .Replace("/*description*/", description)
                .Replace("/*|}*/", "|}");

        public static string LoadSourceFile(string path) => File.ReadAllText(path);

        public static CSharpAnalyzerTest<HoleAnalyzer, XUnitVerifier> CreateTest(
            string source,
            DiagnosticDescriptor expectedDiagnosticDescriptor,
            string expectedDescription,
            DiagnosticSeverity expectedDiagnosticSeverity,
            OptimizationLevel optimizationLevel)
        {
            var analyzerTest = new CSharpAnalyzerTest<HoleAnalyzer, XUnitVerifier>
            {
                TestState =
                {
                    Sources = { source },
                    AdditionalReferences =
                    {
                        MetadataReference.CreateFromFile(typeof(Holedriven.Hole).Assembly.Location)
                    },
                    ExpectedDiagnostics =
                    {
                        new DiagnosticResult(expectedDiagnosticDescriptor)
                            .WithLocation(DIAGNOSTIC_LOCATION)
                            .WithSeverity(expectedDiagnosticSeverity)
                            .WithArguments(expectedDescription),
                    }
                },
            };

            analyzerTest.SolutionTransforms.Add((solution, projectId) =>
            {
                var compilationOptions = solution.GetProject(projectId)!.CompilationOptions!;

                return solution.WithProjectCompilationOptions(
                    projectId,
                    compilationOptions.WithOptimizationLevel(optimizationLevel));
            });

            return analyzerTest;
        }
    }
}