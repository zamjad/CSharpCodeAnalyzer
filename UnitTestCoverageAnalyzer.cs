using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class UnitTestCoverageAnalyzer
{
    public static bool HasUnitTest(MethodDeclarationSyntax methodNode)
    {
        return methodNode.AttributeLists.Any(attr => attr.Attributes
            .Any(a => a.Name.ToString().Contains("Test")));
    }
}
