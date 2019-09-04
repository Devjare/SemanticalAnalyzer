using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    sealed class ExpresionEnParentesis : Expresion
    {
        public ExpresionEnParentesis(Token parentesisApertura, Expresion expresion, Token parentesisCierre)
        {
            ParentesisApertura = parentesisApertura;
            Expresion = expresion;
            ParentesisCierre = parentesisCierre;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionEnParentesis;
        public Token ParentesisApertura { get; }
        public Expresion Expresion { get; }
        public Token ParentesisCierre { get; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return ParentesisApertura;
            yield return Expresion;
            yield return ParentesisCierre;
        }
    }
}