
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class Evaluator
    {
        private readonly ExpressionSyntax _root;

        public Evaluator(ExpressionSyntax root)
        {
            this._root = root;
        }

        public Evaluator()
        {

        }

        //public double Evaluate()
        //{
            
        //}

        public List<String> Evaluate(String code)
        {
            var diagnostics = new List<String>();

            var parser = new Parser(code);
            var tree = parser.Parse();

            // Generate Symbols table
            var VariablesTable = parser.VariablesTable;

            foreach (var variable in VariablesTable)
            {
                Console.WriteLine($"Key: {variable.Key.ToString()}, Value: {variable.Value}");

                if (variable.Key.Kind == SyntaxKind.IntegerKeyword ||
                variable.Key.Kind == SyntaxKind.LongKeyword ||
                variable.Key.Kind == SyntaxKind.FloatKeyword ||
                variable.Key.Kind == SyntaxKind.DoubleKeyword)
                {
                    var result = EvaluateArithmeticExpression(variable.Value);
                    Console.WriteLine("Result arithmetic: " + result);
                }
                else
                {
                    var result = EvaluateLogicalExpression(variable.Value);
                    Console.WriteLine("Logical result: " + result);
                }

            }

            // tree.PrintList();

            diagnostics.AddRange(parser.Diagnostics);

            return diagnostics;
        }

        public double EvaluateArithmeticExpression(ExpressionSyntax node)
        {
            if (node is NumberExpressionSyntax n)
                return (int) n.NumberToken.Value;

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateArithmeticExpression(b.Left);
                var right = EvaluateArithmeticExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.SlashToken)
                    return left / right;
                else if (b.OperatorToken.Kind == SyntaxKind.PowerToken)
                    return Math.Pow(left, right);
                else if (b.OperatorToken.Kind == SyntaxKind.ModulusToken)
                    return left % right;
                else
                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }

            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateArithmeticExpression(p.Expression);

            throw new Exception($"Unexpected node {node.Kind}");
        }

        private bool EvaluateLogicalExpression(ExpressionSyntax node)
        {
            if (node is BoolExpressionSyntax n)
                return Boolean.Parse(n.BoolToken.Value.ToString());

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateLogicalExpression(b.Left);
                var right = EvaluateLogicalExpression(b.Right);

                if (b.OperatorToken.Kind == SyntaxKind.AndToken)
                    return left && right;
                else if (b.OperatorToken.Kind == SyntaxKind.OrToken)
                    return left || right;
                else if (b.OperatorToken.Kind == SyntaxKind.EqualsEqualsToken)
                    return left == right;
                else if (b.OperatorToken.Kind == SyntaxKind.NotEqualsToken)
                    return left != right;
                else
                    throw new Exception($"Unexpected binary operator {b.OperatorToken.Kind}");
            }

            if (node is ParenthesizedExpressionSyntax p)
                return EvaluateLogicalExpression(p.Expression);

            throw new Exception($"Unexpected node {node.Kind}");
        }
    }
} 