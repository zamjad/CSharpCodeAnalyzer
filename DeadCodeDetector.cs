using Microsoft.CodeAnalysis;
public static class DeadCodeDetector
{
    public static List<string> FindUnusedMethods(SemanticModel semanticModel, SyntaxNode root)
    {
        var declaredMethods = root.DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>()
            .Select(m => m.Identifier.Text)
            .ToList();

        var calledMethods = root.DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax>()
            .Select(i => i.Expression.ToString())
            .ToList();

        return declaredMethods.Except(calledMethods).ToList();
    }
}
