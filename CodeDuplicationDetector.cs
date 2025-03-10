using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class CodeDuplicationDetector
{
    public static List<string> DetectDuplicatedMethods(List<MethodDeclarationSyntax> methodNodes)
    {
        var methodBodies = methodNodes.Select(m => m.Body?.ToString()).Where(b => b != null).ToList();
        return methodBodies.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
    }
}
