using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class DIComplianceChecker
{
    public static bool UsesManualInstantiation(ClassDeclarationSyntax classNode)
    {
        return classNode.DescendantNodes().OfType<ObjectCreationExpressionSyntax>().Any();
    }
}
