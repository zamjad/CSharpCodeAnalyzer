using Microsoft.CodeAnalysis.CSharp.Syntax;

public static class DuplicatedCodeDetector
{
    public static List<string> DetectDuplicatedMethods(List<MethodDeclarationSyntax> methodNodes)
    {
        var methodBodies = methodNodes
            .Where(m => m.Body != null)  // Ensure method has a body
            .Select(m => m.Body.ToString())
            .ToList();

        return methodBodies.GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
    }
}
