using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionIf : ExpresionBloqueCodigo
    {
        public ExpresionIf(Token parentesisApertura, Expresion expresion, 
            Token parentesisCierre, Token llaveApertura, List<Expresion> expresiones, Token llaveCierre) : base(expresiones)
        {
            ParentesisApertura = parentesisApertura;
            Expresion = expresion;
            ParentesisCierre = parentesisCierre;
            LlaveApertura = llaveApertura;
            LlaveCierre = llaveCierre;
        }

        public Token ParentesisApertura { get; }
        public Expresion Expresion { get;  }
        public Token ParentesisCierre { get; }
        public Token LlaveApertura { get; }
        public Token LlaveCierre { get; }

        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionIf; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return ParentesisApertura;
            yield return Expresion;
            yield return ParentesisCierre;
            yield return LlaveApertura;
            yield return LlaveCierre;
        }
    }
}
