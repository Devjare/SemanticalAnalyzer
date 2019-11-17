using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    class ExpresionElseInvalida : Expresion
    {
        private Token token;

        public ExpresionElseInvalida(Token token)
        {
            this.token = token;
        }

        public override TipoSintaxis Tipo => throw new NotImplementedException();

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            throw new NotImplementedException();
        }
    }
}
