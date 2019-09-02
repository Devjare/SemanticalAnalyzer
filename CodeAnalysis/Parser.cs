using SemanticalAnalyzer.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    // Start ParserClass
    public class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _position;

        public Dictionary<SyntaxToken, ExpressionSyntax> VariablesTable { get; }

        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;
            do
            {
                token = lexer.NextToken();

                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);   
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            foreach (var item in tokens)
            {
                System.Console.WriteLine($"Token: {item.Text}, Value: {item.Value}");
            }

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
            VariablesTable = new Dictionary<SyntaxToken, ExpressionSyntax>();
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        public SyntaxToken[] GetTokenList() { return _tokens; }

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {

            var statements = new Statements(new List<ExpressionSyntax>());

            while (Current.Kind != SyntaxKind.EndOfFileToken)
            {
                statements.Add(ParseExpression());
            }

            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, statements, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            if (Current.Kind == SyntaxKind.UseKeyword)
            {
                return ParseUseExpression();
            }

            if (Current.Kind == SyntaxKind.VoidKeyword)
            {
                return ParseMainExpression();
            }

            return null;
        }

        private ExpressionSyntax ParseMainExpression()
        {
            var voidToken = NextToken();
            var mainToken = NextToken();
            var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
            var closeParenthesisToken = NextToken();
            var openCurlyBracketsToken = NextToken();

            var expressionsList = new List<ExpressionSyntax>();

            while (Current.Kind != SyntaxKind.CloseCurlyBracketsToken)
            {
                // Parsing Declaration Statements
                if (Current.Kind == SyntaxKind.IntegerKeyword ||
                Current.Kind == SyntaxKind.StringKeyword ||
                Current.Kind == SyntaxKind.FloatKeyword ||
                Current.Kind == SyntaxKind.DoubleKeyword ||
                Current.Kind == SyntaxKind.CharKeyword ||
                Current.Kind == SyntaxKind.LongKeyword ||
                Current.Kind == SyntaxKind.BoolKeyword)
                {
                    expressionsList.Add(ParseDeclarationExpression());
                }
            }

            // Main closure
            var closeCurlyBracketsToken = NextToken();
            return new MainExpressionSyntax(openParenthesisToken, closeParenthesisToken, openCurlyBracketsToken, expressionsList, closeCurlyBracketsToken);
        }

        private ExpressionSyntax ParseDeclarationExpression()
        {
            var datatypeToken = NextToken();
            var identifierToken = NextToken();

            var variableToken = new SyntaxToken(datatypeToken.Kind, _position, identifierToken.Value.ToString(), null);

            var equalsToken = NextToken();

            ExpressionSyntax expression = null;

            if (datatypeToken.Kind.Equals(SyntaxKind.IntegerKeyword) ||
                    datatypeToken.Kind.Equals(SyntaxKind.FloatKeyword) ||
                    datatypeToken.Kind.Equals(SyntaxKind.LongKeyword) ||
                    datatypeToken.Kind.Equals(SyntaxKind.DoubleKeyword))
            {
                expression = ParseTerm();
            }
            else if (datatypeToken.Kind.Equals(SyntaxKind.BoolKeyword))
            {
                expression = ParseLogicalExpression();
            }
            else if (datatypeToken.Kind.Equals(SyntaxKind.CharKeyword))
            {
                expression = ParseCharExpression();
            }
            else if (datatypeToken.Kind.Equals(SyntaxKind.StringKeyword))
            {
                expression = ParseStringExpression();
            }

            VariablesTable[variableToken] = expression;

            var semicolonToken = NextToken();
            return new DeclarationExpressionSyntax(datatypeToken, identifierToken, equalsToken, expression, semicolonToken);
        }

        private ExpressionSyntax ParseStringExpression()
        {
            throw new NotImplementedException();
        }

        private ExpressionSyntax ParseCharExpression()
        {
            throw new NotImplementedException();
        }

        #region "Use packages Parsing Section"

        private ExpressionSyntax ParseUseExpression()
        {

            // use class name from namespace
            // Eg. use List from System.Collection.Generic; -> using System.Collections.Generic.List;
            // Eg. use * from System.Collection.Generic; -> using System.Collections.Generic;

            // if the current Token doesnt match the expected token (Use keyword)
            // Then Error will be added to diagnostics
            // and BadToken would be returned.
            // else the Right UseToken Will be returned.
            // Same for all the other tokens

            var useToken = Match(SyntaxKind.UseKeyword);
            var classIdentifierToken = Match(SyntaxKind.Identifier);
            var fromToken = Match(SyntaxKind.FromKeyword);
            var namespaceIdentifierToken = Match(SyntaxKind.Identifier);
            var semicolonToken = Match(SyntaxKind.SemicolonToken);

            return new UseExpressionSyntax(useToken, classIdentifierToken, fromToken, namespaceIdentifierToken, semicolonToken);
        }

        #endregion  

        #region "Flow Control Structures Parsing Section

        private ExpressionSyntax ParseLogicalExpression() 
        {
            var left = ParsePrimaryLogicalExpression();

            while (Current.Kind.Equals(SyntaxKind.AndToken) ||
                Current.Kind.Equals(SyntaxKind.OrToken) ||
                Current.Kind.Equals(SyntaxKind.EqualsEqualsToken) ||
                Current.Kind.Equals(SyntaxKind.NotEqualsToken))
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryLogicalExpression();
                return new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryLogicalExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseLogicalExpression();
                var right = Match(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var boolToken = Match(SyntaxKind.BoolToken);
            return new BoolExpressionSyntax(boolToken);
        }

        #endregion

        #region "Aritmetic Parsing Section"    
        public ExpressionSyntax ParseArithmeticExpression()
        {
            return ParseTerm();
        }      

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken ||
                   Current.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePower();

            while (Current.Kind == SyntaxKind.StarToken ||
                   Current.Kind == SyntaxKind.SlashToken ||
                   Current.Kind == SyntaxKind.ModulusToken)
            {
                var operatorToken = NextToken();
                var right = ParsePower();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePower()
        {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.PowerToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseArithmeticExpression();
                var right = Match(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }

        #endregion
    }

}