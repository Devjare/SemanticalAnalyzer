using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
        class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();

        public Lexer(string text)
        {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';

                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }


            // Find if the token is a Number
            if (char.IsDigit(Current))
            {

                var start = _position;

                while (char.IsDigit(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                int.TryParse(text, out var value);

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);

            }

            if (char.IsLetter(Current))
            {
                var start = _position;

                while (char.IsLetter(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                if (text == "void")
                    return new SyntaxToken(SyntaxKind.VoidKeyword, start, "VoidKeyword", text);
                if (text == "int")
                    return new SyntaxToken(SyntaxKind.IntegerKeyword, start, "IntegerKeyword", text);
                if (text == "long")
                    return new SyntaxToken(SyntaxKind.LongKeyword, start, "LongKeyword", text);
                if (text == "string")
                    return new SyntaxToken(SyntaxKind.StringKeyword, start, "StringKeyword", text);
                if (text == "double")
                    return new SyntaxToken(SyntaxKind.DoubleKeyword, start, "DoubleKeyword", text);
                if (text == "bool")
                    return new SyntaxToken(SyntaxKind.BoolKeyword, start, "BoolKeyword", text);
                if (text == "char")
                    return new SyntaxToken(SyntaxKind.CharKeyword, start, "CharKeyword", text);
                if (text == "float")
                    return new SyntaxToken(SyntaxKind.FloatKeyword, start, "FloatKeyword", text);
                if (text == "let")
                    return new SyntaxToken(SyntaxKind.LetKeyword, start, "LetKeyword", text);
                if (text == "use")
                    return new SyntaxToken(SyntaxKind.UseKeyword, start, "UseKeyword", text);
                if (text == "from")
                    return new SyntaxToken(SyntaxKind.FromKeyword, start, "FromKeyword", text);
                if (text == "true")
                    return new SyntaxToken(SyntaxKind.BoolToken, start, "BoolToken", text);
                if (text == "false")
                    return new SyntaxToken(SyntaxKind.BoolToken, start, "BoolToken", text);

                return new SyntaxToken(SyntaxKind.Identifier, start, "Identifier", text);
            }


            if (Current == '\"')
            {
                Next();
                var start = _position;
                while (Current != '\"')
                    Next();

                var lastpos = _position;
                var len = lastpos - start;
                var text = _text.Substring(start, len);

                Next();

                return new SyntaxToken(SyntaxKind.StringToken, start, "String", text);
            }

            if (char.IsWhiteSpace(Current))
            {

                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();

                var length = _position - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);

            }

            if (Current == '+')
            {
                Next();
                if (Current == '+')
                {
                    return new SyntaxToken(SyntaxKind.IncrementToken, _position++, "++", null);
                }
                return new SyntaxToken(SyntaxKind.PlusToken, _position, "+", null);
            }

            if (Current == '-')
            {
                Next();
                if (Current == '-')
                {
                    return new SyntaxToken(SyntaxKind.DecrementToken, _position++, "--", null);
                }
                return new SyntaxToken(SyntaxKind.MinusToken, _position, "-", null);
            }

            if (Current == '/')
            {
                Next();
                if (Current == '/')
                {
                    return new SyntaxToken(SyntaxKind.CommentToken, _position++, "//", null);
                }
                return new SyntaxToken(SyntaxKind.SlashToken, _position, "/", null);
            }

            //else if (Current == '-')
            //    return new Token(TokenType.MINUS, _position++, "-", null);
            else if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            //else if (Current == '/')
            //    return new Token(TokenType.DIV, _position++, "/", null);
            else if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null);
            else if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null);
            else if (Current == '{')
                return new SyntaxToken(SyntaxKind.OpenCurlyBracketsToken, _position++, "{", null);
            else if (Current == '}')
                return new SyntaxToken(SyntaxKind.CloseCurlyBracketsToken, _position++, "}", null);
            else if (Current == '[')
                return new SyntaxToken(SyntaxKind.OpenBracketsToken, _position++, "[", null);
            else if (Current == ']')
                return new SyntaxToken(SyntaxKind.CloseBracketsToken, _position++, "]", null);
            else if (Current == '_')
                return new SyntaxToken(SyntaxKind.UnderScoreToken, _position++, "_", null);
            else if (Current == ';')
                return new SyntaxToken(SyntaxKind.SemicolonToken, _position++, ";", null);
            else if (Current == (char)10)
                return new SyntaxToken(SyntaxKind.EndOfLineToken, _position++, "EndOfLine", null);
            else if (Current == '|')
                return new SyntaxToken(SyntaxKind.OrToken, _position++, "|", null);
            else if (Current == '&')
                return new SyntaxToken(SyntaxKind.AndToken, _position++, "&", null);
            else if (Current == '!')
                return new SyntaxToken(SyntaxKind.NotToken, _position++, "!", null);
            else if (Current == '>')
            {
                Next();
                if (Current == '=')
                {
                    return new SyntaxToken(SyntaxKind.GreaterEqualsThanToken, _position++, ">=", null);
                }
                return new SyntaxToken(SyntaxKind.GreaterThanToken, _position, ">", null);
            }
            else if (Current == '<')
            {
                Next();
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.LessEqualsThanToken, _position++, "<=", null);
                return new SyntaxToken(SyntaxKind.LessThanToken, _position, "<", null);
            }
            else if (Current == '=')
            {
                Next();
                if (Current == '=')
                    return new SyntaxToken(SyntaxKind.EqualsEqualsToken, _position++, "==", null);
                if (char.IsWhiteSpace(Current))
                    return new SyntaxToken(SyntaxKind.EqualsToken, _position++, "=", null);
            }
            else if (Current == '.')
                return new SyntaxToken(SyntaxKind.MemberReferenceOperatorToken, _position++, ".", null);
            else if (Current == ',')
                return new SyntaxToken(SyntaxKind.CommaToken, _position++, ",", null);

            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);
        }
    }

}