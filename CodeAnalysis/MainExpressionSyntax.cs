using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class MainExpressionSyntax : ExpressionSyntax
    {


        public MainExpressionSyntax()
        {
        }

        public MainExpressionSyntax(SyntaxToken openParenthesisToken, SyntaxToken closeParenthesisToken, SyntaxToken openCurlyBracketsToken, List<ExpressionSyntax> expressions, SyntaxToken closeCurlyBracketsToken)
        {
            OpenParenthesisToken = openParenthesisToken;
            CloseParenthesisToken = closeParenthesisToken;
            OpenCurlyBracketsToken = openCurlyBracketsToken;
            Expressions = expressions;
            CloseCurlyBracketsToken = closeCurlyBracketsToken;
        }

        public SyntaxToken OpenParenthesisToken { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SyntaxToken OpenCurlyBracketsToken { get; }
        public List<ExpressionSyntax> Expressions { get; }
        public SyntaxToken CloseCurlyBracketsToken { get; }

        public override SyntaxKind Kind => SyntaxKind.MainExpressionSyntax;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthesisToken;
            yield return CloseParenthesisToken;
            yield return OpenCurlyBracketsToken;
            foreach (var item in Expressions)
            {
                yield return item;
            }
            yield return CloseCurlyBracketsToken;
        }
    }
}
