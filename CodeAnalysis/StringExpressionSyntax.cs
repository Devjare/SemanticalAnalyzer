using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class StringExpressionSyntax : Expresion
    {
        private object stringToken;

        public StringExpressionSyntax(object stringToken)
        {
            this.stringToken = stringToken;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.StringExpressionSyntax;

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            throw new System.NotImplementedException();
        }
    }
}