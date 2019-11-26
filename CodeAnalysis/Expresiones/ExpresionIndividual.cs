using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis.Expresiones_Individuales
{
    class ExpresionIndividual : Expresion
    {
        public ExpresionIndividual(Token value)
        {
            this.Value = value;
        }
        public Token Value { get; set; }
        public override TipoSintaxis Tipo => throw new NotImplementedException();

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
