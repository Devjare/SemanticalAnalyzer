using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    class AnalizadorLexico
    {
        private readonly string _texto;
        private int _posicion;
        private List<string> _diagnostico = new List<string>();
        public AnalizadorLexico(string texto)
        {
            _texto = texto;
        }

        public IEnumerable<string> Diagnostico => _diagnostico;

        private char CaracterActual
        {
            get
            {
                if (_posicion >= _texto.Length)
                    return '\0';

                return _texto[_posicion];
            }
        }

        private void SiguienteCaracter()
        {
            _posicion++;
        }

        public Token SiguienteToken()
        {
            if (_posicion >= _texto.Length)
            {
                return new Token(TipoSintaxis.TokenEOF, _posicion, "\0", null);
            }

            // Implement for decimal pointing numebrs
            // And make the new Expression Syntax class for
            // that entry
            // Find if the token is a Number

            if (CaracterActual == '-' && char.IsDigit(_texto[_posicion + 1]))
            {
                var inicio = _posicion;
                SiguienteCaracter();
                if (char.IsDigit(CaracterActual))
                {

                    var isInteger = true;
                    var isValidNumber = true;
                    var dotsCount = 0;
                    var decimalsCount = 0;

                    while (char.IsDigit(CaracterActual) || CaracterActual == '.')
                    {
                        if (CaracterActual == '.')
                        {
                            dotsCount++;
                            isInteger = false;
                            SiguienteCaracter();
                            continue;
                        }

                        if (dotsCount > 1)
                        {
                            isValidNumber = false;
                            break;
                        }

                        if (char.IsDigit(CaracterActual))
                        {
                            SiguienteCaracter();
                            if (dotsCount == 1)
                            {
                                decimalsCount++;
                                if (decimalsCount == 5)
                                {
                                    break;
                                }
                            }
                        }

                    }

                    var length = _posicion - inicio;
                    var texto = _texto.Substring(inicio, length);

                    if (isValidNumber)
                    {
                        if (isInteger)
                        {
                            int.TryParse(texto, out var valor);
                            return new Token(TipoSintaxis.TokenInteger, inicio, texto, valor);
                        }
                        else
                        {
                            float.TryParse(texto, out var valor);
                            return new Token(TipoSintaxis.TokenDecimal, inicio, texto, valor);
                        }
                    }

                    _diagnostico.Add($"ERROR: Invalid Number Format <{texto}>");
                    return new Token(TipoSintaxis.TokenInvalido, _posicion++, _texto.Substring(_posicion - 1, 1), null);
                }

            }

            if (char.IsDigit(CaracterActual))
            {
                var inicio = _posicion;
                var isInteger = true;
                var isValidNumber = true;
                var dotsCount = 0;
                var decimalsCount = 0;

                while (char.IsDigit(CaracterActual) || CaracterActual == '.')
                {
                    if (CaracterActual == '.')
                    {
                        dotsCount++;
                        isInteger = false;
                        SiguienteCaracter();
                        continue;
                    }

                    if (dotsCount > 1)
                    {
                        isValidNumber = false;
                        break;
                    }

                    if (char.IsDigit(CaracterActual))
                    {
                        SiguienteCaracter();
                        if (dotsCount == 1)
                        {
                            decimalsCount++;
                            if (decimalsCount == 5)
                            {
                                break;
                            }
                        }
                    }

                }

                var length = _posicion - inicio;
                var texto = _texto.Substring(inicio, length);

                if (isValidNumber)
                {
                    if (isInteger)
                    {
                        int.TryParse(texto, out var valor);
                        return new Token(TipoSintaxis.TokenInteger, inicio, texto, valor);
                    }
                    else
                    {
                        float.TryParse(texto, out var valor);
                        return new Token(TipoSintaxis.TokenDecimal, inicio, texto, valor);
                    }
                }

                _diagnostico.Add($"ERROR: Invalid Number Format <{texto}>");
                return new Token(TipoSintaxis.TokenInvalido, _posicion++, _texto.Substring(_posicion - 1, 1), null);
            }

            if (char.IsLetter(CaracterActual))
            {
                var inicio = _posicion;

                while (char.IsLetter(CaracterActual))
                    SiguienteCaracter();

                var length = _posicion - inicio;
                var texto = _texto.Substring(inicio, length);

                if (texto == "void")
                    return new Token(TipoSintaxis.VoidKeyword, inicio, "VoidKeyword", texto);
                if (texto == "int")
                    return new Token(TipoSintaxis.IntegerKeyword, inicio, "IntegerKeyword", texto);
                if (texto == "long")
                    return new Token(TipoSintaxis.LongKeyword, inicio, "LongKeyword", texto);
                if (texto == "string")
                    return new Token(TipoSintaxis.StringKeyword, inicio, "StringKeyword", texto);
                if (texto == "double")
                    return new Token(TipoSintaxis.DoubleKeyword, inicio, "DoubleKeyword", texto);
                if (texto == "bool")
                    return new Token(TipoSintaxis.BoolKeyword, inicio, "BoolKeyword", texto);
                if (texto == "char")
                    return new Token(TipoSintaxis.CharKeyword, inicio, "CharKeyword", texto);
                if (texto == "float")
                    return new Token(TipoSintaxis.FloatKeyword, inicio, "FloatKeyword", texto);
                if (texto == "let")
                    return new Token(TipoSintaxis.LetKeyword, inicio, "LetKeyword", texto);
                if (texto == "use")
                    return new Token(TipoSintaxis.UseKeyword, inicio, "UseKeyword", texto);
                if (texto == "from")
                    return new Token(TipoSintaxis.FromKeyword, inicio, "FromKeyword", texto);
                if (texto == "true")
                    return new Token(TipoSintaxis.TokenBool, inicio, "BoolToken", texto);
                if (texto == "false")
                    return new Token(TipoSintaxis.TokenBool, inicio, "BoolToken", texto);
                if (texto == "main")
                    return new Token(TipoSintaxis.TokenFuncionMain, inicio, "TokenFuncionMain", texto);
                if (texto == "if")
                {
                    return new Token(TipoSintaxis.TokenIf, inicio, "TokenIf", texto);
                }
                if (texto == "else")
                {
                    return new Token(TipoSintaxis.TokenElse, inicio, "TokenElse", texto);
                }

                return new Token(TipoSintaxis.Identificador, inicio, "Identifier", texto);
            }


            if (CaracterActual == '\"')
            {
                SiguienteCaracter();
                var inicio = _posicion;
                while (CaracterActual != '\"')
                    SiguienteCaracter();

                var ultimaPosicion = _posicion;
                var longitud = ultimaPosicion - inicio;
                var texto = _texto.Substring(inicio, longitud);

                SiguienteCaracter();

                return new Token(TipoSintaxis.TokenString, inicio, "String", texto);
            }

            if (char.IsWhiteSpace(CaracterActual))
            {

                var inicio = _posicion;

                while (char.IsWhiteSpace(CaracterActual))
                    SiguienteCaracter();

                var length = _posicion - inicio;
                var texto = _texto.Substring(inicio, length);

                return new Token(TipoSintaxis.TokenEspacioEnBlanco, inicio, texto, null);

            }

            if (CaracterActual == '+')
            {
                SiguienteCaracter();
                if (CaracterActual == '+')
                {
                    return new Token(TipoSintaxis.TokenIncremento, _posicion++, "++", null);
                }
                return new Token(TipoSintaxis.TokenMas, _posicion, "+", null);
            }

            if (CaracterActual == '-')
            {
                SiguienteCaracter();
                if (CaracterActual == '-')
                {
                    return new Token(TipoSintaxis.TokenDecremento, _posicion++, "--", null);
                }
                return new Token(TipoSintaxis.TokenMenos, _posicion, "-", null);
            }

            if (CaracterActual == '/')
            {
                SiguienteCaracter();
                if (CaracterActual == '/')
                {
                    return new Token(TipoSintaxis.TokenComentario, _posicion++, "//", null);
                }
                return new Token(TipoSintaxis.TokenDivision, _posicion, "/", null);
            }

            //else if (Current == '-')
            //    return new Token(TokenType.MINUS, _position++, "-", null);
            else if (CaracterActual == '*')
                return new Token(TipoSintaxis.TokenMultiplicacion, _posicion++, "*", null);
            //else if (Current == '/')
            //    return new Token(TokenType.DIV, _position++, "/", null);
            else if (CaracterActual == '(')
                return new Token(TipoSintaxis.TokenParentesisApertura, _posicion++, "(", null);
            else if (CaracterActual == ')')
                return new Token(TipoSintaxis.TokenParentesisCierre, _posicion++, ")", null);
            else if (CaracterActual == '{')
                return new Token(TipoSintaxis.TokenLlavesApertura, _posicion++, "{", null);
            else if (CaracterActual == '}')
                return new Token(TipoSintaxis.TokenLlavesCierre, _posicion++, "}", null);
            else if (CaracterActual == '[')
                return new Token(TipoSintaxis.TokenCorchetesApertura, _posicion++, "[", null);
            else if (CaracterActual == ']')
                return new Token(TipoSintaxis.TokenCorchetesCierre, _posicion++, "]", null);
            else if (CaracterActual == '_')
                return new Token(TipoSintaxis.TokenGuionbajo, _posicion++, "_", null);
            else if (CaracterActual == ';')
                return new Token(TipoSintaxis.TokenPuntoyComa, _posicion++, ";", null);
            else if (CaracterActual == (char)10)
                return new Token(TipoSintaxis.EndOfLineToken, _posicion++, "EndOfLine", null);
            else if (CaracterActual == '|')
                return new Token(TipoSintaxis.TokenOr, _posicion++, "|", null);
            else if (CaracterActual == '&')
                return new Token(TipoSintaxis.TokenAnd, _posicion++, "&", null);
            else if (CaracterActual == '!')
                return new Token(TipoSintaxis.TokenNot, _posicion++, "!", null);
            else if (CaracterActual == '>')
            {
                SiguienteCaracter();
                if (CaracterActual == '=')
                {
                    return new Token(TipoSintaxis.TokenMayorIgualQue, _posicion++, ">=", null);
                }
                return new Token(TipoSintaxis.TokenMayorQue, _posicion, ">", null);
            }
            else if (CaracterActual == '<')
            {
                SiguienteCaracter();
                if (CaracterActual == '=')
                    return new Token(TipoSintaxis.TokenMenorIgualQue, _posicion++, "<=", null);
                return new Token(TipoSintaxis.TokenMenorQue, _posicion, "<", null);
            }
            else if (CaracterActual == '=')
            {
                SiguienteCaracter();
                if (CaracterActual == '=')
                    return new Token(TipoSintaxis.TokenIgualIgual, _posicion++, "==", null);
                if (char.IsWhiteSpace(CaracterActual))
                    return new Token(TipoSintaxis.TokenIgual, _posicion++, "=", null);
            }
            else if (CaracterActual == '.')
                return new Token(TipoSintaxis.TokenOperadorMiembro, _posicion++, ".", null);
            else if (CaracterActual == ',')
                return new Token(TipoSintaxis.TokenComa, _posicion++, ",", null);

            _diagnostico.Add($"ERROR: bad character input: '{CaracterActual}'");
            return new Token(TipoSintaxis.TokenInvalido, _posicion++, _texto.Substring(_posicion - 1, 1), null);
        }
    }

}