using SemanticalAnalyzer.CodeAnalysis;
using SemanticalAnalyzer.CodeAnalysis.Expresiones_Individuales;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExpressionEvaluator.CodeAnalysis
{
    // Start ParserClass
    public class AnalizadorSintactico
    {
        private readonly Token[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _position;

        public Dictionary<String, Object> TablaSimbolos { get; }
        // int suma(int a, int b);
        // functionsMap["suma"] = Pair<"int", {TokenInteger(a), TokenInteger(b)}>;
        public Dictionary<String, Pair<Object, List<Token>>> functionsMap;
        public long LexTimeTaken { get; set; }

        public AnalizadorSintactico(string text)
        {
            var tokens = new List<Token>();

            var stopwatch = new Stopwatch();
            var lexer = new AnalizadorLexico(text);
            Token token;
            stopwatch.Start();
            do
            {
                token = lexer.SiguienteToken();

                if (token.Tipo != TipoSintaxis.TokenEspacioEnBlanco &&
                    token.Tipo != TipoSintaxis.TokenInvalido)
                {
                    tokens.Add(token);
                }
            } while (token.Tipo != TipoSintaxis.TokenEOF);
            stopwatch.Stop();

            LexTimeTaken = stopwatch.ElapsedMilliseconds;

            TokenList = tokens;
            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostico);
            TablaSimbolos = new Dictionary<String, Object>();
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        public Token[] GetTokenList() { return _tokens; }

        private Token Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1];

            return _tokens[index];
        }

        private Token TokenActual => Peek(0);

        public List<Token> TokenList { get; internal set; }

        private Token SiguienteToken()
        {
            var actual = TokenActual;
            _position++;
            return actual;
        }

        private Token CoincideCon(TipoSintaxis tipo)
        {
            if (TokenActual.Tipo == tipo)
                return SiguienteToken();

            _diagnostics.Add($"ERROR: Token inesperado <{TokenActual.Tipo}>, se esperaba <{tipo}>");
            return new Token(tipo, TokenActual.Position, null, null);
        }

        public ArbolSintactico Analizar()
        {
            var sentencias = new Sentencias(new List<Expresion>());

            while (TokenActual.Tipo != TipoSintaxis.TokenEOF)
            {
                sentencias.Add(AnalizarExpresion());
            }

            var endOfFileToken = CoincideCon(TipoSintaxis.TokenEOF);
            return new ArbolSintactico(_diagnostics, sentencias, endOfFileToken);
        }

        private Expresion AnalizarExpresion()
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenInvalido)
            {
                _diagnostics.Add($"ERROR: Token encontrado en posicion invalida <{TokenActual.ToString()}>");
                SiguienteToken();
                return new ExpresionInvalida(TokenActual);
            }
            if (TokenActual.Tipo == TipoSintaxis.Identificador)
            {
                _diagnostics.Add($"ERROR: Identificador encontrado en posicion invalida <{TokenActual.Value.ToString()}>");
                SiguienteToken();
                return new ExpresionIdentificador(TokenActual);
            }

            if (TokenActual.Tipo == TipoSintaxis.UseKeyword)
            {
                return AnalizarExpresionUse();
            }

            if (TokenActual.Tipo == TipoSintaxis.VoidKeyword ||
                TokenActual.Tipo == TipoSintaxis.IntegerKeyword ||
                TokenActual.Tipo == TipoSintaxis.FloatKeyword ||
                TokenActual.Tipo == TipoSintaxis.StringKeyword)
            {
                return AnalizarFuncion();
            }

            _diagnostics.Add($"ERROR: Token encontrado en posicion invalida <{TokenActual.ToString()}>");
            SiguienteToken();
            return new ExpresionInvalida(TokenActual);
        }

        private Expresion AnalizarFuncion()
        {
            var tokenTipoDeDato = SiguienteToken();
            var tokenNombreFuncion = CoincideCon(TipoSintaxis.TokenFuncionMain);

            if (tokenNombreFuncion.Value.Equals("main"))
            {
                return AnalizarExpresionMain();
            }
            else // Analizar otros tipos de funciones y variables globales: COMING SOON.
            {
                var tokenParentesisApertura = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                // Identifier -> Value.
                var parametros = new Dictionary<TipoSintaxis, Token>();
                // functionsMap["suma"] = Pair<"int", {TokenInteger(a), TokenInteger(b)}>;
                // Mientras no sea un parentesis de cierre.
                while (CoincideCon(TipoSintaxis.TokenParentesisCierre) == null)
                {
                    var tokenTipoParam = SiguienteToken();
                    var tokenNombreParam = CoincideCon(TipoSintaxis.Identificador);


                }
            }

            return new ExpresionInvalida(TokenActual);
        }

        private Expresion AnalizarExpresionMain()
        {
            var parentesisApertura = CoincideCon(TipoSintaxis.TokenParentesisApertura);
            var parentesisCierre = CoincideCon(TipoSintaxis.TokenParentesisCierre);
            var llaveApertura = CoincideCon(TipoSintaxis.TokenLlavesApertura);

            var bloqueCodigo = AnalizarBloque();

            if (bloqueCodigo is ExpresionInvalida) return bloqueCodigo;

            var llaveCierre = CoincideCon(TipoSintaxis.TokenLlavesCierre);
            var expresion = bloqueCodigo as ExpresionBloqueCodigo;
            return new ExpresionMain(parentesisApertura, parentesisCierre, llaveApertura, expresion.Sentencias.ListaExpresiones, llaveCierre);
        }

        // Analizar bloque, analizará cualquier estructura dentro de llaves, 
        // ya sea funciones, estrucutras condicionales o ciclicas.
        private Expresion AnalizarBloque()
        {
            var expresiones = new List<Expresion>();

            while (TokenActual.Tipo != TipoSintaxis.TokenLlavesCierre)
            {
                // Parsing Declaration Statements
                if (TokenActual.Tipo == TipoSintaxis.IntegerKeyword ||
                TokenActual.Tipo == TipoSintaxis.StringKeyword ||
                TokenActual.Tipo == TipoSintaxis.FloatKeyword ||
                TokenActual.Tipo == TipoSintaxis.DoubleKeyword ||
                TokenActual.Tipo == TipoSintaxis.CharKeyword ||
                TokenActual.Tipo == TipoSintaxis.LongKeyword ||
                TokenActual.Tipo == TipoSintaxis.BoolKeyword)
                {
                    expresiones.Add(AnalizarExpresionDeDeclaracion());
                }
                else if (TokenActual.Tipo == TipoSintaxis.TokenIf)
                {
                    // Analizar If
                    // Pendiente optimizar, codigo repetido muchas veces.
                    expresiones.Add(AnalizarExpresionIf());
                    if (TokenActual.Tipo == TipoSintaxis.TokenElse)
                    {
                        // Si existe el if, entonces se puede analizar el Else
                        expresiones.Add(AnalizarExpresionElse());
                    }
                }
                // Invalid token
                else
                {
                    _diagnostics.Add($"ERROR: Identificador Invalido <{TokenActual.Value.ToString()}>");
                    return new ExpresionInvalida(TokenActual);
                }
            }

            return new ExpresionBloqueCodigo(expresiones);
        }

        private Expresion AnalizarExpresionElse()
        {
            var tokenElse = SiguienteToken();
            var llaveApertura = CoincideCon(TipoSintaxis.TokenLlavesApertura);
            if (llaveApertura == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));

            var bloqueCodigo = AnalizarBloque();

            if (bloqueCodigo is ExpresionInvalida) return bloqueCodigo;

            var llaveCierre = CoincideCon(TipoSintaxis.TokenLlavesCierre);
            if (llaveCierre == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));
            var expresion = bloqueCodigo as ExpresionBloqueCodigo;
            return new ExpresionElse(llaveApertura, expresion.Sentencias.ListaExpresiones, llaveCierre);
        }

        private Expresion AnalizarExpresionIf()
        {
            var tokenIf = SiguienteToken();
            var tokenParentesisApertura = CoincideCon(TipoSintaxis.TokenParentesisApertura);
            if (tokenParentesisApertura == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));
            var expresionCondicion = AnalizarExpresionBooleana();
            if (expresionCondicion is ExpresionInvalida)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));
            var tokenParentesisCierre = CoincideCon(TipoSintaxis.TokenParentesisCierre);
            if (tokenParentesisCierre == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));
            var llaveApertura = CoincideCon(TipoSintaxis.TokenLlavesApertura);
            if (llaveApertura == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));

            var bloqueCodigo = AnalizarBloque();

            if (bloqueCodigo is ExpresionInvalida) return bloqueCodigo;

            var llaveCierre = CoincideCon(TipoSintaxis.TokenLlavesCierre);
            if (llaveCierre == null)
                return new ExpresionIfInvalida(new Token(TipoSintaxis.TokenIfInvalido, _position, null, null));
            var expresion = bloqueCodigo as ExpresionBloqueCodigo;
            return new ExpresionIf(tokenParentesisApertura, expresionCondicion, tokenParentesisCierre, llaveApertura,
                expresion.Sentencias.ListaExpresiones, llaveCierre);
        }

        private Expresion AnalizarExpresionDeDeclaracion()
        {
            var tokenTipoDato = SiguienteToken();
            var tokenIdentificador = CoincideCon(TipoSintaxis.Identificador);
            if (TablaSimbolos.ContainsKey(tokenIdentificador.Value.ToString()))
            {
                _diagnostics.Add($"ERROR: Multiple declarations of variable <{tokenIdentificador.Value.ToString()}>");
            }
            var equalsToken = CoincideCon(TipoSintaxis.TokenIgual);

            Expresion expresion = null;

            if (tokenTipoDato.Tipo.Equals(TipoSintaxis.IntegerKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.FloatKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.LongKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.DoubleKeyword))
            {
                expresion = AnalizarExpresionAritmetica(tokenTipoDato);

                // If expresion is not an arithmetic expresion, then is invalid.
            }

            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.BoolKeyword))
            {
                expresion = AnalizarExpresionBooleana();
            }
            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.CharKeyword))
            {
                expresion = AnalizarExpresionChar();
            }
            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.StringKeyword))
            {
                expresion = AnalizarExpresionString();
            }

            if (tokenTipoDato.Tipo == TipoSintaxis.IntegerKeyword)
            {
                TablaSimbolos[tokenIdentificador.Value.ToString()] =
                    new Token(TipoSintaxis.TokenInteger, tokenIdentificador.Position, tokenIdentificador.Value.ToString(), expresion);
            }
            else if (tokenTipoDato.Tipo == TipoSintaxis.FloatKeyword)
            {
                TablaSimbolos[tokenIdentificador.Value.ToString()] =
                    new Token(TipoSintaxis.TokenDecimal, tokenIdentificador.Position, tokenIdentificador.Value.ToString(), expresion);
            }
            else if (tokenTipoDato.Tipo == TipoSintaxis.StringKeyword)
            {
                TablaSimbolos[tokenIdentificador.Value.ToString()] =
                    new Token(TipoSintaxis.TokenString, tokenIdentificador.Position, tokenIdentificador.Value.ToString(), expresion);
            }

            var semicolonToken = CoincideCon(TipoSintaxis.TokenPuntoyComa);
            return new ExplresionDeclaracion(tokenTipoDato, tokenIdentificador, equalsToken, expresion, semicolonToken);
        }

        private Expresion AnalizarExpresionBooleana()
        {
            var izquierda = AnalizarExpresionBooleanaAnd();

            while (TokenActual.Tipo == TipoSintaxis.TokenAnd)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionBooleanaAnd();
                izquierda = new ExpresionBooleana(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionBooleanaAnd()
        {
            var izquierda = AnalizarExpresionBooleanaOr();

            while (TokenActual.Tipo == TipoSintaxis.TokenOr)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionBooleanaOr();
                izquierda = new ExpresionBooleana(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionBooleanaOr()
        {
            var izquierda = AnalizarExpresionBooleanaPrimaria();

            while (TokenActual.Tipo == TipoSintaxis.TokenNotIgual)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionBooleanaPrimaria();
                izquierda = new ExpresionBooleana(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionBooleanaPrimaria()
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var izquierda = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                var expresion = AnalizarExpresionBooleana();
                var derecha = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(izquierda, expresion, derecha);
            }

            var token = SiguienteToken();

            if (token.Tipo is TipoSintaxis.TokenString ||
                token.Tipo is TipoSintaxis.TokenDecimal ||
                token.Tipo is TipoSintaxis.TokenInteger)
            {
                return AnalizarExpresionRelacional(token);
                // return new ExpresionString(token);
            }
            // si estamos analizando un token como "true" o "false"
            else if (token.Tipo is TipoSintaxis.TokenBool)
            {
                return new ExpresionLogica(token);
            }
            else if (token.Tipo is TipoSintaxis.Identificador)
            {
                var identificador = token;
                if (TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    var variable = (Token)TablaSimbolos[identificador.Value.ToString()];
                    if (variable.Tipo is TipoSintaxis.TokenString ||
                        variable.Tipo is TipoSintaxis.TokenDecimal ||
                        variable.Tipo is TipoSintaxis.TokenInteger)
                    {
                        return AnalizarExpresionRelacional(token);
                        // return new ExpresionIdentificador(token);
                    }

                    else
                    {
                        _diagnostics.Add($"ERROR: Se variable de tipo invalido, se esperaba tipo <{variable.Tipo}>");
                        return new ExpresionIdentificador(new Token(TipoSintaxis.Identificador, _position, null, null));
                    }
                }
            }

            _diagnostics.Add($"ERROR: Variable de tipo invalido, se esperaba string.");
            return new ExpresionStringInvalida(new Token(TipoSintaxis.TokenNumericoInvalido, _position, null, null));
        }

        private Expresion AnalizarExpresionRelacional(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalIgual(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenIgualIgual)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalIgual(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalIgual(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalMayorQue(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenMayorQue)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalMayorQue(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalMayorQue(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalMenorQue(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenMenorQue)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalMenorQue(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalMenorQue(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalMayorIgualQue(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenMayorIgualQue)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalMayorIgualQue(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalMayorIgualQue(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalMenorIgualQue(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenMenorIgualQue)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalMenorIgualQue(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalMenorIgualQue(Token token)
        {
            var izquierda = AnalizarExpresionRelacionalPrimaria(token);

            while (TokenActual.Tipo == TipoSintaxis.TokenMenorIgualQue)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionRelacionalPrimaria(token = SiguienteToken());
                izquierda = new ExpresionRelacional(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionRelacionalPrimaria(Token currentToken)
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var izquierda = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                var expresion = AnalizarExpresionBooleana();
                var derecha = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(izquierda, expresion, derecha);
            }

            if (currentToken.Tipo is TipoSintaxis.TokenString) return new ExpresionString(currentToken);
            else if (currentToken.Tipo is TipoSintaxis.TokenDecimal) return new ExpresionDecimal(currentToken);
            else if (currentToken.Tipo is TipoSintaxis.TokenInteger) return new ExpresionEntera(currentToken);
            else if (currentToken.Tipo is TipoSintaxis.Identificador)
            {
                var identificador = currentToken;
                if (TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    var variable = (Token)TablaSimbolos[identificador.Value.ToString()];
                    if (variable.Tipo is TipoSintaxis.TokenString ||
                        variable.Tipo is TipoSintaxis.TokenDecimal ||
                        variable.Tipo is TipoSintaxis.TokenInteger)
                    {
                        return new ExpresionIdentificador(currentToken);
                        // return new ExpresionIdentificador(token);
                    }
                    else
                    {
                        _diagnostics.Add($"ERROR: Se variable de tipo invalido, se esperaba tipo <{variable.Tipo}>");
                        return new ExpresionIdentificador(new Token(TipoSintaxis.Identificador, _position, null, null));
                    }
                }
            }
            _diagnostics.Add($"ERROR: Variable de tipo invalido, se esperaba valor.");
            return new ExpresionInvalida(new Token(TipoSintaxis.TokenInvalido, _position, null, null));
        }

        private Expresion AnalizarExpresionString()
        {
            var izquierda = AnalizarExpresionStringPrimaria();

            while (TokenActual.Tipo == TipoSintaxis.TokenMas)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarExpresionStringPrimaria();
                izquierda = new ExpresionBinaria(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionStringPrimaria()
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var izquierda = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                var expresion = AnalizarExpresionString();
                var derecha = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(izquierda, expresion, derecha);
            }

            var tokenString = SiguienteToken();

            if (tokenString.Tipo is TipoSintaxis.TokenString)
            {
                return new ExpresionString(tokenString);
            }
            else if (tokenString.Tipo is TipoSintaxis.Identificador)
            {
                var identificador = tokenString;
                if (TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    var variable = (Token)TablaSimbolos[identificador.Value.ToString()];
                    if (variable.Tipo is TipoSintaxis.TokenString)
                    {
                        return new ExpresionIdentificador(tokenString);
                    }
                    else
                    {
                        _diagnostics.Add($"ERROR: Se variable de tipo invalido, se esperaba tipo <{variable.Tipo}>");
                        return new ExpresionIdentificador(new Token(TipoSintaxis.Identificador, _position, null, null));
                    }
                }
            }

            _diagnostics.Add($"ERROR: Variable de tipo invalido, se esperaba string.");
            return new ExpresionStringInvalida(new Token(TipoSintaxis.TokenNumericoInvalido, _position, null, null));
        }

        private Expresion AnalizarExpresionChar()
        {
            throw new NotImplementedException();
        }

        #region "Use packages Parsing Section"

        private Expresion AnalizarExpresionUse()
        {

            // use class name from namespace
            // Eg. use List from System.Collection.Generic; -> using System.Collections.Generic.List;
            // Eg. use * from System.Collection.Generic; -> using System.Collections.Generic;

            // if the current Token doesnt match the expected token (Use keyword)
            // Then Error will be added to diagnostics
            // and BadToken would be returned.
            // else the Right UseToken Will be returned.
            // Same for all the other tokens

            var useToken = CoincideCon(TipoSintaxis.UseKeyword);
            var classIdentifierToken = CoincideCon(TipoSintaxis.Identificador);
            var fromToken = CoincideCon(TipoSintaxis.FromKeyword);
            var namespaceIdentifierToken = CoincideCon(TipoSintaxis.Identificador);
            var semicolonToken = CoincideCon(TipoSintaxis.TokenPuntoyComa);

            return new ExpresionUse(useToken, classIdentifierToken, fromToken, namespaceIdentifierToken, semicolonToken);
        }

        #endregion  

        #region "Flow Control Structures Parsing Section

        #endregion

        #region "Aritmetic Parsing Section"    
        public Expresion AnalizarExpresionAritmetica(Token tipoDeDato)
        {
            return AnalizarTerminos(tipoDeDato);
        }

        private Expresion AnalizarTerminos(Token tipoDeDato)
        {
            var izquierda = AnalizarFactores(tipoDeDato);

            while (TokenActual.Tipo == TipoSintaxis.TokenMas ||
                   TokenActual.Tipo == TipoSintaxis.TokenMenos)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarFactores(tipoDeDato);
                izquierda = new ExpresionBinaria(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarFactores(Token tipoDeDato)
        {
            var izquierda = AnalizarPotencias(tipoDeDato);

            while (TokenActual.Tipo == TipoSintaxis.TokenMultiplicacion ||
                   TokenActual.Tipo == TipoSintaxis.TokenDivision ||
                   TokenActual.Tipo == TipoSintaxis.TokenModulo)
            {
                var operador = SiguienteToken();
                var right = AnalizarPotencias(tipoDeDato);
                izquierda = new ExpresionBinaria(izquierda, operador, right);
            }

            return izquierda;
        }

        private Expresion AnalizarPotencias(Token tipoDeDato)
        {
            var izquierda = AnalizarExpresionAritmeticaPrimaria(tipoDeDato);

            while (TokenActual.Tipo == TipoSintaxis.TokenPotencia)
            {
                var operatorToken = SiguienteToken();
                var derecha = AnalizarExpresionAritmeticaPrimaria(tipoDeDato);
                izquierda = new ExpresionBinaria(izquierda, operatorToken, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionAritmeticaPrimaria(Token tipoDeDato)
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var izquierda = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                var expresion = AnalizarExpresionAritmetica(tipoDeDato);
                var derecha = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(izquierda, expresion, derecha);
            }

            var tokenNumero = SiguienteToken();

            if (tokenNumero.Tipo is TipoSintaxis.TokenInteger && tipoDeDato.Tipo == TipoSintaxis.IntegerKeyword)
            {
                return new ExpresionEntera(tokenNumero);
            }
            else if (tokenNumero.Tipo is TipoSintaxis.TokenDecimal && tipoDeDato.Tipo == TipoSintaxis.IntegerKeyword)
            {
                var token = new Token(tokenNumero.Tipo, tokenNumero.Position, tokenNumero.Text, (int)float.Parse(tokenNumero.Value.ToString()));
                return new ExpresionEntera(token);
            }
            else if (tokenNumero.Tipo is TipoSintaxis.TokenDecimal && tipoDeDato.Tipo == TipoSintaxis.FloatKeyword)
            {
                return new ExpresionDecimal(tokenNumero);
            }
            else if (tokenNumero.Tipo is TipoSintaxis.TokenInteger && tipoDeDato.Tipo == TipoSintaxis.FloatKeyword)
            {
                var token = new Token(tokenNumero.Tipo, tokenNumero.Position, tokenNumero.Text, (float)int.Parse(tokenNumero.Value.ToString()));
                return new ExpresionDecimal(token);
            }
            else if (tokenNumero.Tipo is TipoSintaxis.Identificador)
            {
                var identificador = tokenNumero;
                if (TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    var variable = (Token)TablaSimbolos[identificador.Value.ToString()];
                    if (variable.Tipo is TipoSintaxis.TokenInteger ||
                        variable.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        return new ExpresionIdentificador(tokenNumero);
                    }
                    else
                    {
                        _diagnostics.Add($"ERROR: Se variable de tipo invalido, se esperaba tipo <{variable.Tipo}>");
                        return new ExpresionIdentificador(new Token(TipoSintaxis.Identificador, _position, null, null));
                    }
                }
                else
                {
                    _diagnostics.Add($"ERROR: Identificador <{identificador}> no existe en el ambito actual.");
                    return new ExpresionNumericaInvalida(new Token(TipoSintaxis.TokenNumericoInvalido, _position, null, null));
                }
            }

            _diagnostics.Add($"ERROR: Variable de tipo invalido, se esperaba valor numerico.");
            return new ExpresionNumericaInvalida(new Token(TipoSintaxis.TokenNumericoInvalido, _position, null, null));
        }

        #endregion
    }

}