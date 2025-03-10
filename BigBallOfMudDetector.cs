using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class BigBallOfMudDetector
{
    public static bool IsBigBallOfMud(ClassDeclarationSyntax classNode, int threshold)
    {
        int dependencyCount = classNode.DescendantNodes()
            .OfType<IdentifierNameSyntax>()
            .Select(id => id.Identifier.Text)
            .Distinct().Count();

        return dependencyCount > threshold;
    }
}
