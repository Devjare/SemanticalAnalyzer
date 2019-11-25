using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionBloqueCodigo : Expresion
    {
        public ExpresionBloqueCodigo(List<Expresion> expresiones)
        {
            Sentencias = new Sentencias(expresiones);
        }
        public override TipoSintaxis Tipo => throw new NotImplementedException();
        public Sentencias Sentencias { get; set; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            foreach (var item in Sentencias.ListaExpresiones)
            {
                yield return item;
            }
        }
    }
}
