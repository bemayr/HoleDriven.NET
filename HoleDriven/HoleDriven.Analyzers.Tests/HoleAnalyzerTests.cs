using HoleDriven.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static HoleDriven.Analyzers.Tests.HoleAnalyzerTestsHelpers;

namespace HoleDriven.Analyzers.Tests
{
    public class HoleAnalyzerTests : HoleAnalyzerTestsBase
    {
        private const string DUMMY_HOLE_DESCRIPTION = "Testing the Hole Description 🧪";

        public HoleAnalyzerTests(ITestOutputHelper output) : base(output) { }

        public static IEnumerable<object[]> GetSourceFiles() =>
            from file in Data.SourceFiles
            select new[] { new HoleAnalyzerTestInput
            {
                Name = Path.GetFileNameWithoutExtension(file),
                Source = File.ReadAllText(file),
                ExpectedDiagnostic = Path.GetFileName(file).Split(".")[0]
            } };

        [Fact]
        public void Sourcecode_Preparation_Works()
        {
            // Arrange
            var source = @"
using HoleDriven;

namespace HoleDriven.Analyzers.Tests
{
    public class Get
    {
        public static int Number => /*{|ExpectedDiagnosticLocation:*/Hole.Get(""/*description*/"", 42)/*|}*/;
    }
}";
            var expected = @"
using HoleDriven;

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
        [Trait("Category", "Engine")]
        [MemberData(nameof(GetSourceFiles))]
        public async Task Holes_gets_Reported_DEBUG(HoleAnalyzerTestInput input)
        {
            // Arrange
            var expectedDiagnosticDescriptor = Data.GetDiagnosticDescriptor(input.ExpectedDiagnostic);
            var source = PrepareSourcecode(input.Source, DUMMY_HOLE_DESCRIPTION);

            // Act
            var test = CreateTest(
                source,
                expectedDiagnosticDescriptor,
                DUMMY_HOLE_DESCRIPTION,
                DiagnosticSeverity.Info,
                OptimizationLevel.Debug);

            // Assert
            await test.RunAsync();
        }

        [Theory]
        [Trait("Category", "Engine")]
        [MemberData(nameof(GetSourceFiles))]
        public async Task Holes_gets_Reported_RELEASE(HoleAnalyzerTestInput input)
        {
            // Arrange
            var expectedDiagnosticDescriptor = Data.GetDiagnosticDescriptor(input.ExpectedDiagnostic);
            var source = PrepareSourcecode(input.Source, DUMMY_HOLE_DESCRIPTION);

            // Act
            var test = CreateTest(
                source,
                expectedDiagnosticDescriptor,
                DUMMY_HOLE_DESCRIPTION,
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
                        MetadataReference.CreateFromFile(typeof(Hole).Assembly.Location)
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

    internal static class Data
    {
        public static IEnumerable<string> SourceFiles
        {
            get
            {
                var holeSourcesPath = Path.Combine(Environment.CurrentDirectory, "TestData");
                var holeSourcesFiles = Directory.EnumerateFiles(holeSourcesPath, "*.cs", SearchOption.AllDirectories);
                return holeSourcesFiles;
            }
        }

        public static DiagnosticDescriptor GetDiagnosticDescriptor(string methodName) => methodName switch
        {
            nameof(Hole.Refactor) => HoleAnalyzer.Diagnostic.Refactor,
            nameof(Hole.Idea) => HoleAnalyzer.Diagnostic.Idea,
            nameof(Hole.Effect) => HoleAnalyzer.Diagnostic.Effect,
            nameof(Hole.EffectAsync) => HoleAnalyzer.Diagnostic.EffectAsync,
            nameof(Hole.Provide) => HoleAnalyzer.Diagnostic.Provide,
            nameof(Hole.ProvideAsync) => HoleAnalyzer.Diagnostic.ProvideAsync,
            nameof(Hole.Throw) => HoleAnalyzer.Diagnostic.Throw,
            _ => throw new NotImplementedException($"no Analyzer available for this {methodName}")
        };
    }

    // used for NCrunch: https://forum.ncrunch.net/yaf_postst3132_IXunitSerializable-and-serializable.aspx
    [Serializable]
    public record HoleAnalyzerTestInput : IXunitSerializable
    {
        private string? name;
        public string Name
        {
            get => name ?? throw new NullReferenceException($"{nameof(Name)} can't be null.");
            init => name = value;
        }
        private string? source;
        public string Source
        {
            get => source ?? throw new NullReferenceException($"{nameof(Source)} can't be null.");
            init => source = value;
        }

        private string? expectedDiagnostic;
        public string ExpectedDiagnostic
        {
            get => expectedDiagnostic ?? throw new NullReferenceException($"{nameof(ExpectedDiagnostic)} can't be null.");
            init => expectedDiagnostic = value;
        }

        // needed for IXUnitSerializable
        public HoleAnalyzerTestInput() { }

        // needed for test case distinction: https://github.com/xunit/xunit/issues/1473
        public void Deserialize(IXunitSerializationInfo info)
        {
            name = info.GetValue<string>("name");
            source = info.GetValue<string>("source");
            expectedDiagnostic = info.GetValue<string>("expectedDiagnostic");
        }
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("name", Name, typeof(string));
            info.AddValue("source", Source, typeof(string));
            info.AddValue("expectedDiagnostic", ExpectedDiagnostic, typeof(string));
        }
        public override string ToString() => Name;
    }
}