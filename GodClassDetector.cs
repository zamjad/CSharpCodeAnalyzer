using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class GodClassDetector
{
    public static bool IsGodClass(ClassDeclarationSyntax classNode, int methodThreshold, int fieldThreshold)
    {
        int methodCount = classNode.Members.OfType<MethodDeclarationSyntax>().Count();
        int fieldCount = classNode.Members.OfType<FieldDeclarationSyntax>().Count();
        return methodCount > methodThreshold && fieldCount > fieldThreshold;
    }
}
