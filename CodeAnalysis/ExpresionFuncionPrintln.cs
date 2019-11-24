using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionFuncionPrintln : Expresion
    {
        private Token parentesisApertura;
        private Expresion expresion;
        private Token parentesisCierre;

        public ExpresionFuncionPrintln(Token parentesisApertura, Expresion expresion, Token parentesisCierre)
        {
            this.parentesisApertura = parentesisApertura;
            this.expresion = expresion;
            this.parentesisCierre = parentesisCierre;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionFuncionPrintln;

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return parentesisApertura;
            yield return expresion;
            yield return parentesisCierre;
        }
    }
}