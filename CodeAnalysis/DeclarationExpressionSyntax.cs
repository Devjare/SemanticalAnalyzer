using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class DeclarationExpressionSyntax : ExpressionSyntax
    {
        private SyntaxToken DatatypeToken;
        private SyntaxToken IdentifierToken;
        private SyntaxToken EqualsToken;
        private ExpressionSyntax Expression;
        private SyntaxToken SemicolonToken;

        public DeclarationExpressionSyntax(SyntaxToken datatypeToken, SyntaxToken identifierToken, SyntaxToken equalsToken, ExpressionSyntax expression, SyntaxToken semicolonToken)
        {
            this.DatatypeToken = datatypeToken;
            this.IdentifierToken = identifierToken;
            this.EqualsToken = equalsToken;
            this.Expression = expression;
            this.SemicolonToken = semicolonToken;
        }

        public override SyntaxKind Kind => SyntaxKind.DeclarationExpressionSyntax;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return DatatypeToken;
            yield return IdentifierToken;
            yield return EqualsToken;
            yield return Expression;
            yield return SemicolonToken;
        }
    }
}