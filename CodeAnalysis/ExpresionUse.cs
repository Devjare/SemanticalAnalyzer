using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionUse : Expresion
    {
        public ExpresionUse(Token tokenUse, Token IdentificadorDeClase, Token tokenFrom, Token identificadorNamespace, Token tokenPuntoyComa)
        {
            TokenUse = tokenUse;
            this.IdentificadorDeClase = IdentificadorDeClase;
            TokenFrom = tokenFrom;
            IdentificadorNamespace = identificadorNamespace;
            TokenPuntoyComa = tokenPuntoyComa;
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionUse;

        public Token TokenUse { get; }
        // Could be an asterists or a common identifier
        public Token IdentificadorDeClase { get; }
        public Token TokenFrom { get; }
        public Token IdentificadorNamespace { get; }

        public Token TokenPuntoyComa { get; }
        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return TokenUse;
            yield return IdentificadorDeClase;
            yield return TokenFrom;
            yield return IdentificadorNamespace;
            yield return TokenPuntoyComa;
        }

    }
}
