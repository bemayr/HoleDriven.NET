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

        private const string HoleMessageFormat = "🧩 {0}";

        public static class Rules
        {
            public static readonly DiagnosticDescriptor Refactor = new DiagnosticDescriptor(
                id: "HD0001",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Idea = new DiagnosticDescriptor(
                id: "HD0002",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Effect = new(
                id: "HD0003",
                title: "Effect Analyzer Title (where does this show?)",
                messageFormat: HoleMessageFormat,
                category: "Effect Category (where does this show?)",
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: "Effect Description (where does this show?)",
                helpLinkUri: "https://github.com/bemayr/HoleDriven.NET");

            public static readonly DiagnosticDescriptor EffectAsync = new DiagnosticDescriptor(
                id: "HD0004",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Provide = new DiagnosticDescriptor(
                id: "HD0005",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor ProvideAsync = new DiagnosticDescriptor(
                id: "HD0006",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);

            public static readonly DiagnosticDescriptor Throw = new DiagnosticDescriptor(
                id: "HD0007",
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            Rules.Refactor,
            Rules.Idea,
            Rules.Effect,
            Rules.EffectAsync,
            Rules.Provide,
            Rules.ProvideAsync,
            Rules.Throw);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            // register the analyzer only if Holes are in use
            context.RegisterCompilationStartAction(compilationContext =>
            {
                if (compilationContext.Compilation.GetTypeByMetadataName("HoleDriven.Hole") is null)
                    return;

                // get the current optimization level to properly report the Holes
                var optimizationLevel = compilationContext.Compilation.Options.OptimizationLevel;

                Console.WriteLine(optimizationLevel);

                compilationContext.RegisterSyntaxNodeAction(
                    context => AnalyzeHoles(context, optimizationLevel),
                    SyntaxKind.InvocationExpression);
            });
        }

        private static void AnalyzeHoles(SyntaxNodeAnalysisContext context, OptimizationLevel optimizationLevel)
        {
            var invocationExpression = (InvocationExpressionSyntax)context.Node; // Hole.<...>(<...>)
            var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax; // Hole.< ...>

            var holeExpression = memberAccessExpression?.Expression as IdentifierNameSyntax; // Hole
            if (holeExpression.Identifier.Text != "Hole")
                return; // we are not dealing with a Hole expression

            var methodSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpression).Symbol as IMethodSymbol;
            if (!methodSymbol?.ToString().StartsWith("HoleDriven.Hole") ?? true)
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
                nameof(Rules.Refactor) => Rules.Refactor,
                nameof(Rules.Idea) => Rules.Idea,
                nameof(Rules.Effect) => Rules.Effect,
                nameof(Rules.EffectAsync) => Rules.EffectAsync,
                nameof(Rules.Provide) => Rules.Provide,
                nameof(Rules.ProvideAsync) => Rules.Provide,
                nameof(Rules.Throw) => Rules.Throw,
                _ => null,
            };
            var severity = optimizationLevel switch
            {
                OptimizationLevel.Release => DiagnosticSeverity.Error,
                OptimizationLevel.Debug => DiagnosticSeverity.Info,
                _ => throw new System.Exception($"Unknown OptimizationLevel: {optimizationLevel}"),
            };

            var diagnostic = Diagnostic.Create(
                descriptor: diagnosticDescriptor,
                location: invocationExpression.GetLocation(),
                effectiveSeverity: severity,
                additionalLocations: null,
                properties: null,
                description);
            context.ReportDiagnostic(diagnostic);
        }
    }
}
