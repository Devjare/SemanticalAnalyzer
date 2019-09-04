using SemanticalAnalyzer.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    // Start ParserClass
    public class AnalizadorSintactico
    {
        private readonly Token[] _tokens;

        private List<string> _diagnostics = new List<string>();
        private int _position;

        public Dictionary<Token, Expresion> TablaSimbolos { get; }

        public AnalizadorSintactico(string text)
        {
            var tokens = new List<Token>();

            var lexer = new AnalizadorLexico(text);
            Token token;
            do
            {
                token = lexer.SiguienteToken();

                if (token.Tipo != TipoSintaxis.TokenEspacioEnBlanco &&
                    token.Tipo != TipoSintaxis.TokenInvalido)
                {
                    tokens.Add(token);   
                }
            } while (token.Tipo != TipoSintaxis.TokenEOF);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostico);
            TablaSimbolos = new Dictionary<Token, Expresion>();
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
            if (TokenActual.Tipo == TipoSintaxis.UseKeyword)
            {
                return AnalizarExpresionUse();
            }

            if (TokenActual.Tipo == TipoSintaxis.VoidKeyword)
            {
                return AnalizarExpresionMain();
            }

            return null;
        }

        private Expresion AnalizarExpresionMain()
        {
            var tokenVoid = CoincideCon(TipoSintaxis.VoidKeyword);
            var tokenMain = CoincideCon(TipoSintaxis.TokenFuncionMain);
            var parentesisApertura = CoincideCon(TipoSintaxis.TokenParentesisApertura);
            var parentesisCierre = CoincideCon(TipoSintaxis.TokenLlavesApertura);
            var llaveApertura = CoincideCon(TipoSintaxis.TokenLlavesCierre);

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
            }

            // Main closure
            var llaveCierre = CoincideCon(TipoSintaxis.TokenLlavesCierre);
            return new ExpresionMain(parentesisApertura, parentesisCierre, llaveApertura, expresiones, llaveCierre);
        }

        private Expresion AnalizarExpresionDeDeclaracion()
        {
            var tokenTipoDato = SiguienteToken();
            var tokenIdentificador = CoincideCon(TipoSintaxis.Identificador);

            var variable = new Token(tokenTipoDato.Tipo, _position, tokenIdentificador.Value.ToString(), null);

            var equalsToken = CoincideCon(TipoSintaxis.TokenIgual);

            Expresion expresion = null;

            if (tokenTipoDato.Tipo.Equals(TipoSintaxis.IntegerKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.FloatKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.LongKeyword) ||
                    tokenTipoDato.Tipo.Equals(TipoSintaxis.DoubleKeyword))
            {
                expresion = AnalizarExpresionAritmetica();
            }
            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.BoolKeyword))
            {
                expresion = AnalizarExpresionLogica();
            }
            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.CharKeyword))
            {
                expresion = AnalizarExpresionChar();
            }
            else if (tokenTipoDato.Tipo.Equals(TipoSintaxis.StringKeyword))
            {
                expresion = AnalizarExpresionString();
            }

            TablaSimbolos[variable] = expresion;

            var semicolonToken = CoincideCon(TipoSintaxis.TokenPuntoyComa);
            return new ExplresionDeclaracion(tokenTipoDato, tokenIdentificador, equalsToken, expresion, semicolonToken);
        }

        private Expresion AnalizarExpresionString()
        {


            throw new NotImplementedException();
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

        private Expresion AnalizarExpresionLogica() 
        {
            var izquierda = AnalizarExpresionLogicaPrimaria();

            while (TokenActual.Tipo.Equals(TipoSintaxis.TokenAnd) ||
                TokenActual.Tipo.Equals(TipoSintaxis.TokenOr) ||
                TokenActual.Tipo.Equals(TipoSintaxis.TokenIgualIgual) ||
                TokenActual.Tipo.Equals(TipoSintaxis.TokenNotIgual))
            {
                var operatorToken = SiguienteToken();
                var right = AnalizarExpresionLogicaPrimaria();
                return new ExpresionBinaria(izquierda, operatorToken, right);
            }

            return izquierda;
        }


        // true
        private Expresion AnalizarExpresionLogicaPrimaria()
        {

            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var left = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                var expression = AnalizarExpresionLogica();
                var right = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(left, expression, right);
            }

            var tokenBool = CoincideCon(TipoSintaxis.TokenBool);
            return new ExpresionBool(tokenBool);
        }

        #endregion

        #region "Aritmetic Parsing Section"    
        public Expresion AnalizarExpresionAritmetica()
        {
            return AnalizarTerminos();
        }      

        private Expresion AnalizarTerminos()
        {
            var izquierda = AnalizarFactores();

            while (TokenActual.Tipo == TipoSintaxis.TokenMas ||
                   TokenActual.Tipo == TipoSintaxis.TokenMenos)
            {
                var operador = SiguienteToken();
                var derecha = AnalizarFactores();
                izquierda = new ExpresionBinaria(izquierda, operador, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarFactores()
        {
            var izquierda = AnalizarPotencias();

            while (TokenActual.Tipo == TipoSintaxis.TokenMultiplicacion ||
                   TokenActual.Tipo == TipoSintaxis.TokenDivision ||
                   TokenActual.Tipo == TipoSintaxis.TokenModulo)
            {
                var operador = SiguienteToken();
                var right = AnalizarPotencias();
                izquierda = new ExpresionBinaria(izquierda, operador, right);
            }

            return izquierda;
        }

        private Expresion AnalizarPotencias()
        {
            var izquierda = AnalizarExpresionAritmeticaPrimaria();

            while (TokenActual.Tipo == TipoSintaxis.TokenPotencia)
            {
                var operatorToken = SiguienteToken();
                var derecha = AnalizarExpresionAritmeticaPrimaria();
                izquierda = new ExpresionBinaria(izquierda, operatorToken, derecha);
            }

            return izquierda;
        }

        private Expresion AnalizarExpresionAritmeticaPrimaria()
        {
            if (TokenActual.Tipo == TipoSintaxis.TokenParentesisApertura)
            {
                var izquierda = CoincideCon(TipoSintaxis.TokenParentesisApertura);
                var expresion = AnalizarExpresionAritmetica();
                var derecha = CoincideCon(TipoSintaxis.TokenParentesisCierre);
                return new ExpresionEnParentesis(izquierda, expresion, derecha);
            }

            var tokenEntero = CoincideCon(TipoSintaxis.TokenInteger);
            return new ExpresionEntera(tokenEntero);
        }

        #endregion
    }

}