using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
public static class CodeCommentQualityAnalyzer
{
    public static bool HasInsufficientComments(ClassDeclarationSyntax classNode, int threshold)
    {
        int commentCount = classNode.GetLeadingTrivia()
            .Count(trivia => trivia.Kind() == SyntaxKind.SingleLineCommentTrivia ||
                             trivia.Kind() == SyntaxKind.MultiLineCommentTrivia);

        return commentCount < threshold;
    }
}
