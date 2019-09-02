using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }
}