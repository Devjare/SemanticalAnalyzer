using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class WhileSyntax
    {


        // While OpenParenthesis Condition CloseParenthesis OpenCurlyBrackets Statements CloseCurlyBrackets

        public SyntaxToken OpenParenthesisToken { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SyntaxToken OpenCurlyBracketsToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseCurlyBracketsToken { get; }
    }
}
