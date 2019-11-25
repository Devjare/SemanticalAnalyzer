using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExplresionDeclaracion : Expresion
    {
        public Token TipoDato;
        public Token Identificador;
        public Token TokenIgual;
        public Expresion Expresion;
        public Token PuntoyComa;

        public ExplresionDeclaracion(Token tipoDato, Token identificador
            , Token tokenIgual, Expresion expresion, Token puntoyComa)
        {
            this.TipoDato = tipoDato;
            this.Identificador = identificador;
            this.TokenIgual = tokenIgual;
            this.Expresion = expresion;
            this.PuntoyComa = puntoyComa;
        }

        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionDeclaracion; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return TipoDato;
            yield return Identificador;
            yield return TokenIgual;
            yield return Expresion;
            yield return PuntoyComa;
        }
    }
}