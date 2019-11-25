using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionMain : ExpresionBloqueCodigo
    {

        public ExpresionMain(Token parentesisApertura, Token parentesisCierre, Token llaveApertura, List<Expresion> expresiones, Token llaveCierre) : base(expresiones)
        {
            ParentesisApertura = parentesisApertura;
            ParentesisCierre = parentesisCierre;
            Expresiones = expresiones;
            LlaveApertura = llaveApertura;
            LlaveCierre = llaveCierre;
        }

        public Token ParentesisApertura { get; }
        public Token ParentesisCierre { get; }
        public Token LlaveApertura { get; }
        public List<Expresion> Expresiones { get; }
        public Token LlaveCierre { get; }

        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionMain; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return ParentesisApertura;
            yield return ParentesisCierre;
            yield return LlaveApertura;
            foreach (var item in Expresiones)
            {
                yield return item;
            }
            yield return LlaveCierre;
        }
    }
}
