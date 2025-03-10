using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class SingletonStaticDetector
{
    public static bool UsesExcessiveSingletons(ClassDeclarationSyntax classNode)
    {
        return classNode.Modifiers.Any(m => m.Text == "static") ||
               classNode.Members.OfType<FieldDeclarationSyntax>()
                   .Any(f => f.Declaration.Type.ToString().Contains("Singleton"));
    }
}
