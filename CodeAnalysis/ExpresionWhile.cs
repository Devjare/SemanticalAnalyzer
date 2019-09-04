using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionWhile
    {


        // While OpenParenthesis Condition CloseParenthesis OpenCurlyBrackets Statements CloseCurlyBrackets

        public Token OpenParenthesisToken { get; }
        public Token CloseParenthesisToken { get; }
        public Token OpenCurlyBracketsToken { get; }
        public Expresion Expression { get; }
        public Token CloseCurlyBracketsToken { get; }
    }
}
