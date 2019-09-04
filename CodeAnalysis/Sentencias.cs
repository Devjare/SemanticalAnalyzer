using ExpressionEvaluator.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemanticalAnalyzer.CodeAnalysis
{
    /// <summary>
    /// This class represents the main root of the complete source coe
    /// it is a class containing only the list of the rest of statements. ie. the root.
    /// </summary>
    public class Sentencias 
    {
        public List<Expresion> ListaExpresiones { get; }
        public Sentencias()
        {

        }

        public Sentencias(List<Expresion> expresiones)
        {
            ListaExpresiones = expresiones;
        }

        internal void Add(Expresion statement)
        {
            ListaExpresiones.Add(statement);
        }
    }
}
