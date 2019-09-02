using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class UseExpressionSyntax : ExpressionSyntax
    {
        public UseExpressionSyntax(SyntaxToken useToken, SyntaxToken classIdentifierToken, SyntaxToken fromToken, SyntaxToken namespaceIdentifierToken, SyntaxToken semicolonToken)
        {
            UseToken = useToken;
            ClassIdentifierToken = classIdentifierToken;
            FromToken = fromToken;
            NamespaceIdentifierToken = namespaceIdentifierToken;
            SemicolonToken = semicolonToken;
        }

        public override SyntaxKind Kind => SyntaxKind.UseExpression;

        public SyntaxToken UseToken { get; }
        // Could be an asterists or a common identifier
        public SyntaxToken ClassIdentifierToken { get; }
        public SyntaxToken FromToken { get; }
        public SyntaxToken NamespaceIdentifierToken { get; }

        public SyntaxToken SemicolonToken { get; }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return UseToken;
            yield return ClassIdentifierToken;
            yield return FromToken;
            yield return NamespaceIdentifierToken;
            yield return SemicolonToken;
        }

    }
}
