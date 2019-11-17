using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator.CodeAnalysis;

namespace SemanticalAnalyzer.CodeAnalysis.Expresiones_Individuales
{
    internal class ExpresionLogica : ExpresionIndividual
    {
        public ExpresionLogica(Token token) : base(token)
        {
        }
        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionLogica;

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            return base.GetChildren();
        }
    }
}
