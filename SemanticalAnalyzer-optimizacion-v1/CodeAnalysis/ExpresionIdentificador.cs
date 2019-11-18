using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionIdentificador : Expresion
    {
        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionIdentificador; }

        public Token Identificador;
        public ExpresionIdentificador(Token identificador)
        {
            Identificador = identificador;
        }
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Identificador;
        }
    }
}