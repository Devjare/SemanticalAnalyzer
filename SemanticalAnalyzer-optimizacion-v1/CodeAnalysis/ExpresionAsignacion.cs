using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionAsignacion : Expresion
    {
        public override TipoSintaxis Tipo { get => TipoSintaxis.ExpresionAsignacion; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
