using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Immutable;

namespace HoleDriven.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class HoleAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Hole.Get";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.HoleAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.HoleAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Holes";

        private static LocalizableString GetResourceString(string name) => new LocalizableResourceString(name, Resources.ResourceManager, typeof(Resources));

        public static class Rules
        {
            public static readonly DiagnosticDescriptor Get = new DiagnosticDescriptor(
                id: "HD0001",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Refactor = new DiagnosticDescriptor(
                id: "HD0002",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Set = new DiagnosticDescriptor(
                id: "HD0003",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor SetAsync = new DiagnosticDescriptor(
                id: "HD0004",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Throw = new DiagnosticDescriptor(
                id: "HD0005",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Idea = new DiagnosticDescriptor(
                id: "HD0006",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rules.Get, Rules.Set, Rules.SetAsync, Rules.Throw, Rules.Idea, Rules.Refactor); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // register the analyzer only if Holes are in use
            context.RegisterCompilationStartAction(compilationContext =>
            {
                if (compilationContext.Compilation.GetTypeByMetadataName("Holedriven.Hole") is null)
                    return;

                var compilation = compilationContext.Compilation;
                Console.WriteLine(compilation.Options.Platform);
                Console.WriteLine(compilation.Options.WarningLevel);
                Console.WriteLine(compilation.Options.OptimizationLevel);

                compilationContext.RegisterSyntaxNodeAction(AnalyzeHole, SyntaxKind.InvocationExpression);
            });
        }

        private static void AnalyzeHole(SyntaxNodeAnalysisContext context)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node; // Hole.<...>(<...>)
            var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax; // Hole.< ...>

            var holeExpression = memberAccessExpression?.Expression as IdentifierNameSyntax; // Hole
            if (holeExpression.Identifier.Text != "Hole")
                return; // we are not dealing with a Hole expression

            var methodSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpression).Symbol as IMethodSymbol;
            if (!methodSymbol?.ToString().StartsWith("Holedriven.Hole") ?? true)
                return; // the Hole we detected did not originate from the Holedriven library

            var argumentList = invocationExpression.ArgumentList as ArgumentListSyntax;
            if ((argumentList?.Arguments.Count ?? 0) < 1)
                return; // Description is always the first argument, so we have to have at least one

            var descriptionLiteral = argumentList.Arguments[0].Expression as LiteralExpressionSyntax;
            if (descriptionLiteral is null)
                return; /// Description is not a literal, although this should be caught by <see cref="HoleMustHaveDescriptionAnalyzer"/>

            var descriptionOptional = context.SemanticModel.GetConstantValue(descriptionLiteral);
            if (!descriptionOptional.HasValue)
                return;

            var description = descriptionOptional.Value as string;
            if (description is null)
                return;

            var diagnosticDescriptor = methodSymbol.Name switch
            {
                nameof(Rules.Get) => Rules.Get,
                nameof(Rules.Set) => Rules.Set,
                nameof(Rules.SetAsync) => Rules.Set,
                nameof(Rules.Throw) => Rules.Throw,
                nameof(Rules.Idea) => Rules.Idea,
                nameof(Rules.Refactor) => Rules.Refactor,
                _ => null,
            };

            var mode = "";
            if (diagnosticDescriptor is not null)
            {
#if DEBUG
                Console.WriteLine("Mode = Debug");
                mode = "Debug";
#else
                Console.WriteLine("Mode = Release"); 
                mode = "Release";
#endif

#if HELLO
                Console.WriteLine("HELLO = yes :)");
#else
                Console.WriteLine("HELLO = no :(");
#endif


                var location = invocationExpression.GetLocation();
                var diagnostic = Diagnostic.Create(diagnosticDescriptor, location, description + $" ({mode})");
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}
