using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionString : Expresion
    {
        public Token Valor { get; set; }

        public ExpresionString(Token tokenString)
        {
            this.Valor = tokenString;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionStirng;

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Valor;
        }
    }
}