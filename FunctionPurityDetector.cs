using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class FunctionPurityDetector
{
    public static bool IsPureFunction(MethodDeclarationSyntax methodNode)
    {
        return !methodNode.DescendantNodes().OfType<AssignmentExpressionSyntax>().Any() &&
               !methodNode.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().Any();
    }
}
