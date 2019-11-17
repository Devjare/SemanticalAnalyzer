using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionRelacional : ExpresionBooleana
    {
        public ExpresionRelacional(Expresion izquierda, Token operador, Expresion derecha)
            : base(izquierda, operador, derecha)
        {
        }
    }
}
