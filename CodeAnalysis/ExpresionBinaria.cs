
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    sealed class ExpresionBinaria : Expresion
    {
        public ExpresionBinaria(Expresion left, Token operatorToken, Expresion right)
        {
            Izquierda = left;
            Operador = operatorToken;
            Derecha = right;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionBinaria;
        public Expresion Izquierda { get; }
        public Token Operador { get; }
        public Expresion Derecha { get; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Izquierda;
            yield return Operador;
            yield return Derecha;
        }
    }
} 