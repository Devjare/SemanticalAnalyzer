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
    public class Statements 
    {
        public List<ExpressionSyntax> ExpressionsList { get; }
        public Statements()
        {

        }

        public Statements(List<ExpressionSyntax> expressionsList)
        {
            ExpressionsList = expressionsList;
        }

        internal void Add(ExpressionSyntax statement)
        {
            ExpressionsList.Add(statement);
        }
    }
}
