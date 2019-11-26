using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator.CodeAnalysis;

namespace SemanticalAnalyzer.CodeAnalysis.Expresiones
{
    class ExpresionPrintlnInvalida : ExpresionInvalida
    {
        public ExpresionPrintlnInvalida(Token valor) : base(valor)
        {
        }

        public override TipoSintaxis Tipo => TipoSintaxis.ExpresionPrintlnInvalida;
    }
}
