using ExpressionEvaluator.CodeAnalysis;
using System.Collections.Generic;

namespace SemanticalAnalyzer.CodeAnalysis
{
    internal class ExpresionFuncionPrintln : Expresion
    {
        public Token ParentesisApertura { get; }
        public Expresion Expresion { get; }
        public Token ParentesisCierre { get; }

        public ExpresionFuncionPrintln(Token parentesisApertura, Expresion expresion, Token parentesisCierre)
        {
            this.ParentesisApertura = parentesisApertura;
            this.Expresion = expresion;
            this.ParentesisCierre = parentesisCierre;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionFuncionPrintln;

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return ParentesisApertura;
            yield return Expresion;
            yield return ParentesisCierre;
        }
    }
}