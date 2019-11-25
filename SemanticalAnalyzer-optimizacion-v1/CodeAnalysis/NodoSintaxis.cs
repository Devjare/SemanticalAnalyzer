using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    public abstract class NodoSintaxis
    {
        public abstract TipoSintaxis Tipo { get; }

        public abstract IEnumerable<NodoSintaxis> GetChildren();
    }
}