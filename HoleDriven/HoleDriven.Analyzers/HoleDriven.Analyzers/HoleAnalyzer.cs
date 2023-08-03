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
        private static DiagnosticDescriptor CreateHoleDignosticDescriptor(int id, string holeName) => new(
            id: $"HD{id.ToString().PadLeft(4, '0')}",
            title: new LocalizableResourceString($"{holeName}HoleAnalyzerTitle", Resources.ResourceManager, typeof(Resources)),
            messageFormat: "🧩 {0}",
            category: "Hole",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: $"https://holedriven.net/hole/{holeName.ToLowerInvariant()}");
        public static class Diagnostic
        {
            public static readonly DiagnosticDescriptor Refactor = CreateHoleDignosticDescriptor(1, nameof(Refactor));
            public static readonly DiagnosticDescriptor Idea = CreateHoleDignosticDescriptor(2, nameof(Idea));
            public static readonly DiagnosticDescriptor Effect = CreateHoleDignosticDescriptor(3, nameof(Effect));
            public static readonly DiagnosticDescriptor EffectAsync = CreateHoleDignosticDescriptor(4, nameof(EffectAsync));
            public static readonly DiagnosticDescriptor Provide = CreateHoleDignosticDescriptor(5, nameof(Provide));
            public static readonly DiagnosticDescriptor ProvideAsync = CreateHoleDignosticDescriptor(6, nameof(ProvideAsync));
            public static readonly DiagnosticDescriptor Throw = CreateHoleDignosticDescriptor(7, nameof(Throw));
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(
            Diagnostic.Refactor,
            Diagnostic.Idea,
            Diagnostic.Effect,
            Diagnostic.EffectAsync,
            Diagnostic.Provide,
            Diagnostic.ProvideAsync,
            Diagnostic.Throw);

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
                nameof(Diagnostic.Refactor) => Diagnostic.Refactor,
                nameof(Diagnostic.Idea) => Diagnostic.Idea,
                nameof(Diagnostic.Effect) => Diagnostic.Effect,
                nameof(Diagnostic.EffectAsync) => Diagnostic.EffectAsync,
                nameof(Diagnostic.Provide) => Diagnostic.Provide,
                nameof(Diagnostic.ProvideAsync) => Diagnostic.Provide,
                nameof(Diagnostic.Throw) => Diagnostic.Throw,
                _ => null,
            };
            var severity = optimizationLevel switch
            {
                OptimizationLevel.Release => DiagnosticSeverity.Error,
                OptimizationLevel.Debug => DiagnosticSeverity.Info,
                _ => throw new System.Exception($"Unknown OptimizationLevel: {optimizationLevel}"),
            };

            if (diagnosticDescriptor.Id == Diagnostic.Refactor.Id)
            {
                var diagnostic = Microsoft.CodeAnalysis.Diagnostic.Create(
                    id: Diagnostic.Refactor.Id,
                    category: Diagnostic.Refactor.Category,
                    message: string.Format(Diagnostic.Refactor.MessageFormat.ToString(), description),
                    severity,
                    defaultSeverity: Diagnostic.Refactor.DefaultSeverity,
                    isEnabledByDefault: Diagnostic.Refactor.IsEnabledByDefault,
                    warningLevel: severity == DiagnosticSeverity.Error ? 0 : 1, /// <see cref="Diagnostic.WarningLevel"/> Gets the warning level. This is 0 for diagnostics with severity Microsoft.CodeAnalysis.DiagnosticSeverity.Error, otherwise an integer between 1 and 4.
                    title: Diagnostic.Refactor.Title,
                    description: "this is some description that would otherwise not be available",
                    helpLink: Diagnostic.Refactor.HelpLinkUri,
                    location: invocationExpression.GetLocation());

                context.ReportDiagnostic(diagnostic);
            }
            else
            {
                var diagnostic = Microsoft.CodeAnalysis.Diagnostic.Create(
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
}
