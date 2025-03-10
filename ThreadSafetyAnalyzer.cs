using Microsoft.CodeAnalysis.CSharp.Syntax;
public static class ThreadSafetyAnalyzer
{
    public static bool HasSharedState(ClassDeclarationSyntax classNode)
    {
        return classNode.DescendantNodes().OfType<FieldDeclarationSyntax>()
            .Any(field => !field.Modifiers.Any(m => m.Text == "readonly") && !field.Modifiers.Any(m => m.Text == "volatile"));
    }
}
