using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class LongMethodDetector
{
    public static bool IsLongMethod(MethodDeclarationSyntax methodNode, int threshold)
    {
        return methodNode.Body?.Statements.Count > threshold;
    }
}
