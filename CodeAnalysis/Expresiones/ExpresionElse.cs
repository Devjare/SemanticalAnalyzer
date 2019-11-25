using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator.CodeAnalysis;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionElse : ExpresionBloqueCodigo
    {
        public ExpresionElse(Token llaveApertura, List<Expresion> expresiones, Token llaveCierre) 
            : base(expresiones)
        {
            this.TokenLlaveApertura = llaveApertura;
            this.TokenLlaveCierre = llaveCierre;
        }

        public override TipoSintaxis Tipo => base.Tipo;

        public Token TokenLlaveApertura { get; }
        public Token TokenLlaveCierre { get; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return TokenLlaveApertura;
            yield return TokenLlaveCierre;
        }
    }
}
