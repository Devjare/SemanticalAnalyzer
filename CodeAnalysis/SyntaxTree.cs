using SemanticalAnalyzer.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, Statements root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public void PrintList()
        {
            foreach (var item in Root.ExpressionsList)
            {
                PrintTree(item);
            }
        }
        private void PrintTree(ExpressionSyntax root)
        {
            foreach (var item in root.GetChildren())
            {
                System.Console.WriteLine($"E: {item}");
            }
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public Statements Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();
        }
    }
}