using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class HighCouplingDetector
{
    public static bool HasHighCoupling(ClassDeclarationSyntax classNode, int threshold)
    {
        int dependencyCount = classNode.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Select(id => id.Identifier.Text)
            .Distinct().Count();

        return dependencyCount > threshold; 
    }
}
