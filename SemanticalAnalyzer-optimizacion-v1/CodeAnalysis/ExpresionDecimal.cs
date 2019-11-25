using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionDecimal : Expresion
    {

        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionDecimal; }

        public Token Numero;
        public ExpresionDecimal(Token numero)
        {
            Numero = numero;
        }
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Numero;
        }
    }
}
