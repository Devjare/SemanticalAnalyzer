using SemanticalAnalyzer.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class ArbolSintactico
    {
        public ArbolSintactico(IEnumerable<string> diagnostico, Sentencias raiz, Token endOfFileToken)
        {
            Diagnostico = diagnostico.ToArray();
            Raiz = raiz;
            EndOfFileToken = endOfFileToken;
        }

        public void ImprimirLsita()
        {
            foreach (var item in Raiz.ListaExpresiones)
            {
                ImprimirArbol(item);
            }
        }
        private void ImprimirArbol(Expresion raiz)
        {
            foreach (var item in raiz.GetChildren())
            {
                System.Console.WriteLine($"E: {item}");
            }
        }

        public IReadOnlyList<string> Diagnostico { get; }
        public Sentencias Raiz { get; }
        public Token EndOfFileToken { get; }

        public static ArbolSintactico Analizar(string texto)
        {
            var analizadorSintactico = new AnalizadorSintactico(texto);
            return analizadorSintactico.Analizar();
        }
    }
}