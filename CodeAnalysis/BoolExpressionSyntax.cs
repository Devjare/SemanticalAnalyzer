using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class BoolExpressionSyntax : ExpressionSyntax
    {
        public BoolExpressionSyntax(SyntaxToken boolToken)
        {
            BoolToken = boolToken;
        }

        public override SyntaxKind Kind => SyntaxKind.BoolExpressionSyntax;

        public SyntaxToken BoolToken { get; }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return BoolToken;
        }
    }
}