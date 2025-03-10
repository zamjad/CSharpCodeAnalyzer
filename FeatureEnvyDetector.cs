using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class FeatureEnvyDetector
{
    public static bool IsFeatureEnvy(MethodDeclarationSyntax methodNode, int threshold)
    {
        var externalClassUsage = methodNode.DescendantNodes()
            .OfType<MemberAccessExpressionSyntax>()
            .GroupBy(m => m.Expression.ToString())
            .OrderByDescending(g => g.Count());

        return externalClassUsage.FirstOrDefault()?.Count() > threshold; 
    }
}
