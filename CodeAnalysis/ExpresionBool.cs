using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionBool : Expresion
    {
        public ExpresionBool(Token tokenBool)
        {
            TokenBool = tokenBool;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionBool;

        public Token TokenBool { get; }
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return TokenBool;
        }
    }
}