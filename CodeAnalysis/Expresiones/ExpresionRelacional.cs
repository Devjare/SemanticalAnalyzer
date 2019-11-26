using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    internal class ExpresionRelacional : Expresion
    {
        public ExpresionRelacional(Expresion izquierda, Token operador, Expresion derecha)
        {
            this.Izquierda = izquierda;
            this.Operador = operador;
            this.Derecha = derecha;
        }
        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionRelacional; }
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
