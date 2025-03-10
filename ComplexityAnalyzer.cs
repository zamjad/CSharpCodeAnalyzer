using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
public static class ComplexityAnalyzer
{
    public static int CalculateComplexity(SyntaxNode node)
    {
        return node.DescendantNodes().OfType<IfStatementSyntax>().Count() +
               node.DescendantNodes().OfType<ForStatementSyntax>().Count() +
               node.DescendantNodes().OfType<WhileStatementSyntax>().Count() + 1;
    }
}
