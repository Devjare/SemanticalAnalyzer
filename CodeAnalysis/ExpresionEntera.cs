using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    sealed class ExpresionEntera : Expresion
    {
        public ExpresionEntera(Token numero)
        {
            Numero = numero;
        }

        
        public Token Numero { get; }
        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionEntera;
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Numero;
        }
    }
}