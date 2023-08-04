using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace HoleDriven.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class HoleAnalyzer : DiagnosticAnalyzer
    {
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
                    context => AnalyzeHoleMethods(context, optimizationLevel),
                    SyntaxKind.InvocationExpression);

                compilationContext.RegisterSyntaxNodeAction(
                    context => AnalyzeHoleAttributes(context, optimizationLevel),
                    SyntaxKind.Attribute);
            });
        }

        private static void AnalyzeHoleMethods(SyntaxNodeAnalysisContext context, OptimizationLevel optimizationLevel)
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

            var holeDescription = descriptionOptional.Value as string;
            if (holeDescription is null)
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
            var location = invocationExpression.GetLocation();
            var additionalDescription = GetAdditionalDescription();
            var additionalLocations = GetAdditionalLocations();

            var diagnostic = CreateHoleDiagnostic(
                diagnosticDescriptor,
                holeDescription,
                severity,
                location,
                additionalDescription,
                additionalLocations);

            context.ReportDiagnostic(diagnostic);

            string GetAdditionalDescription()
            {
                // We only care about additional Description if we are handling a Refactor Hole
                if (diagnosticDescriptor.Id == Diagnostic.Refactor.Id)
                {
                    var argumentList = invocationExpression.ArgumentList as ArgumentListSyntax;
                    if ((argumentList?.Arguments.Count ?? 0) < 2)
                        return null; // There is no Invocation Expression that we can use

                    var wrappedToRefactorExpression = argumentList.Arguments[1].Expression as ParenthesizedLambdaExpressionSyntax;
                    if (wrappedToRefactorExpression is null)
                        return null; // The second parameter is no parenthesized lambda expression

                    return $"⚒️ Refactoring needed for:{Environment.NewLine}{wrappedToRefactorExpression.Body}";
                }
                else return null;
            }

            IEnumerable<Location> GetAdditionalLocations()
            {
                // We only care about additional Description if we are handling an Idea Hole
                if (diagnosticDescriptor.Id == Diagnostic.Idea.Id)
                {
                    // TODO: query Scope of Idea and show it here
                    return null;
                }
                else return null;
            }
        }

        private static void AnalyzeHoleAttributes(SyntaxNodeAnalysisContext context, OptimizationLevel optimizationLevel)
        {
            var syntax = (AttributeSyntax)context.Node; // Hole.<...>(<...>)
            var nameSyntax = syntax.Name as QualifiedNameSyntax;

            var namePrefixSyntax = nameSyntax?.Left as IdentifierNameSyntax; // Hole
            if (namePrefixSyntax.Identifier.Text != "Hole")
                return; // we are not dealing with a Hole attribute

            var constructorSymbol = context.SemanticModel.GetSymbolInfo(nameSyntax).Symbol as IMethodSymbol;
            if (!constructorSymbol?.ToString().StartsWith("HoleDriven.Hole") ?? true)
                return; // the Hole we detected did not originate from the Holedriven library

            var argumentList = syntax.ArgumentList as AttributeArgumentListSyntax;
            if ((argumentList?.Arguments.Count ?? 0) < 1)
                return; // Description is always the first argument, so we have to have at least one

            var descriptionLiteral = argumentList.Arguments[0].Expression as LiteralExpressionSyntax;
            if (descriptionLiteral is null)
                return; /// Description is not a literal, although this should be caught by <see cref="HoleMustHaveDescriptionAnalyzer"/>

            var descriptionOptional = context.SemanticModel.GetConstantValue(descriptionLiteral);
            if (!descriptionOptional.HasValue)
                return;

            var holeDescription = descriptionOptional.Value as string;
            if (holeDescription is null)
                return;

            var attributeName = constructorSymbol.ContainingType.Name;
            var diagnosticDescriptor = attributeName.Replace("Attribute", string.Empty) switch
            {
                nameof(Diagnostic.Refactor) => Diagnostic.Refactor,
                nameof(Diagnostic.Idea) => Diagnostic.Idea,
                _ => null,
            };

            var severity = optimizationLevel switch
            {
                OptimizationLevel.Release => DiagnosticSeverity.Error,
                OptimizationLevel.Debug => DiagnosticSeverity.Info,
                _ => throw new System.Exception($"Unknown OptimizationLevel: {optimizationLevel}"),
            };
            var location = syntax.GetLocation(); // TODO: apply to the location of target

            var diagnostic = CreateHoleDiagnostic(
                diagnosticDescriptor,
                holeDescription,
                severity,
                location,
                additionalDescription: null,
                additionalLocations: null);

            context.ReportDiagnostic(diagnostic);
        }

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
        
        private static DiagnosticDescriptor CreateHoleDignosticDescriptor(int id, string holeName) => new(
            id: $"HD{id.ToString().PadLeft(4, '0')}",
            title: new LocalizableResourceString($"{holeName}HoleAnalyzerTitle", Resources.ResourceManager, typeof(Resources)),
            messageFormat: "🧩 {0}",
            category: "Hole",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true,
            helpLinkUri: $"https://holedriven.net/hole/{holeName.ToLowerInvariant()}");

        private static Microsoft.CodeAnalysis.Diagnostic CreateHoleDiagnostic(
            DiagnosticDescriptor diagnosticDescriptor,
            string holeDescription,
            DiagnosticSeverity severity,
            Location location,
            string additionalDescription,
            IEnumerable<Location> additionalLocations) =>
            Microsoft.CodeAnalysis.Diagnostic.Create(
                id: diagnosticDescriptor.Id,
                category: diagnosticDescriptor.Category,
                message: string.Format(diagnosticDescriptor.MessageFormat.ToString(), holeDescription),
                severity,
                defaultSeverity: diagnosticDescriptor.DefaultSeverity,
                isEnabledByDefault: diagnosticDescriptor.IsEnabledByDefault,
                warningLevel: severity == DiagnosticSeverity.Error ? 0 : 1, /// <see cref="Diagnostic.WarningLevel"/> Gets the warning level. This is 0 for diagnostics with severity Microsoft.CodeAnalysis.DiagnosticSeverity.Error, otherwise an integer between 1 and 4.
                title: diagnosticDescriptor.Title,
                description: additionalDescription,
                helpLink: diagnosticDescriptor.HelpLinkUri,
                location: location,
                additionalLocations);
    }
}
