using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionNumericaInvalida : Expresion
    {
        public ExpresionNumericaInvalida(Token valor)
        {
            Valor = valor;
        }

        public Token Valor { get; }
        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionNumericaInvalida;
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Valor;
        }
    }
}
