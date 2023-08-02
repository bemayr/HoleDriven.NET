using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace HoleDriven.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class HoleAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "HD0001";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.HoleAnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.HoleAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Holes";

        private static LocalizableString GetResourceString(string name) => new LocalizableResourceString(name, Resources.ResourceManager, typeof(Resources));

        private static class Rules
        {
            internal static readonly DiagnosticDescriptor Get = new DiagnosticDescriptor(
                id: DiagnosticId,
                title: GetResourceString(nameof(Resources.HoleAnalyzerTitle)),
                messageFormat: MessageFormat,
                category: Category,
                defaultSeverity: DiagnosticSeverity.Warning,
                isEnabledByDefault: true,
                description: Description);
        }

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rules.Get); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // register the analyzer only if Holes are in use
            context.RegisterCompilationStartAction(compilationContext =>
            {
                if (compilationContext.Compilation.GetTypeByMetadataName("Holedriven.Hole") is null)
                    return;

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

            //var methodSymbol = context.SemanticModel.GetSymbolInfo(memberAccessExpression).Symbol;
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

            switch (methodSymbol.Name)
            {
                case "Set":
                    var location1 = Location.Create(invocationExpression.SyntaxTree, invocationExpression.FullSpan);
                    var location2 = invocationExpression.GetLocation();
                    var diagnostic = Diagnostic.Create(Rules.Get, location1, description);
                    context.ReportDiagnostic(diagnostic);
                    break;
            }


            //// TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            //var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;

            //// Find just those named type symbols with names containing lowercase letters.
            //if (namedTypeSymbol.Name.ToCharArray().Any(char.IsLower))
            //{
            //    // For all such symbols, produce a diagnostic.
            //    var diagnostic = Diagnostic.Create(GetRule, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);

            //    context.ReportDiagnostic(diagnostic);
            //}
        }
    }
}
