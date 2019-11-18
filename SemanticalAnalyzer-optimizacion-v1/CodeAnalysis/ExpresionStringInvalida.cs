using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionStringInvalida : Expresion
    {
        private Token token;

        public ExpresionStringInvalida(Token token)
        {
            this.token = token;
        }

        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionStringInvalida; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return token;
        }
    }
}