using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    internal class ExpresionBooleana : Expresion
    {
        public ExpresionBooleana(Expresion izquierda, Token operador, Expresion derecha)
        {
            this.Izquierda = izquierda;
            this.Operador = operador;
            this.Derecha = derecha;
        }
        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionBooleana; }
        public Expresion Izquierda { get; }
        public Token Operador { get; }
        public Expresion Derecha { get; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            yield return Izquierda;
            yield return Operador;
            yield return Derecha;
        }
    }
}