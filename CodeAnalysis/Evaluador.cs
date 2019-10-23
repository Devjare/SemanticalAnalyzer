
using SemanticalAnalyzer.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class Evaluador
    {
        private readonly Expresion _raiz;
        public Dictionary<String, Object> TablaSimbolos { get; set; }
        public Dictionary<String, Object> TablaSintaxis { get; set; }
        public List<String> Diagnostico;
        public List<Token> TokenList;
        public Evaluador(Expresion raiz, Dictionary<String, Object> tablaSimbolos) : this(raiz)
        {
            TablaSimbolos = tablaSimbolos;
        }
        public Evaluador(Expresion raiz)
        {
            this._raiz = raiz;
        }

        public Evaluador()
        {
            TablaSimbolos = new Dictionary<String, Object>();
            Diagnostico = new List<String>();
            TablaSintaxis = new Dictionary<string, object>();
        }
        public List<Token> ListaExpresiones;
        public List<String> Evaluar(String codigo)
        {
            var parser = new AnalizadorSintactico(codigo);
            var arbol = parser.Analizar();
            TokenList = parser.TokenList;

            ListaExpresiones = new List<Token>();

            //for (int i = 0; i < TokenList.Count; i++)
            //{
            //    if (TokenList[i].Tipo == TipoSintaxis.IntegerKeyword || 
            //        TokenList[i].Tipo == TipoSintaxis.FloatKeyword ||
            //        TokenList[i].Tipo == TipoSintaxis.StringKeyword)
            //    {
            //        // Current int
            //        // i++ -> Current =
            //        // i++ -> Current Identifier/Number.
            //        // i++ -> Current ;
            //        var id = TokenList[i++];
            //        i++;// Equals token.
            //        var actual = TokenList[++i];
            //        while (actual.Tipo != TipoSintaxis.TokenPuntoyComa)
            //        {
            //            ListaExpresiones.Add(actual);
            //            actual = TokenList[++i];
            //        }
            //    }
            //}


            // Agregamos los errores sintacticos.
            Diagnostico.AddRange(arbol.Diagnostico);

            // Generate Symbols table
            var tablaSimbolosSintactica = TablaSintaxis = parser.TablaSimbolos;

            foreach (var registro in tablaSimbolosSintactica)
            {
                var token = (Token)registro.Value;
                if (token.Tipo == TipoSintaxis.TokenInteger ||
                token.Tipo == TipoSintaxis.LongKeyword ||
                token.Tipo == TipoSintaxis.TokenDecimal ||
                token.Tipo == TipoSintaxis.DoubleKeyword)
                {
                    if (token.Value is ExpresionNumericaInvalida)
                    {
                        continue;
                    }

                    try
                    {
                        var result = EvaluarExpresionAritmetica((Expresion)token.Value, token.Tipo);
                        if ((result as Token).Tipo == TipoSintaxis.TokenInvalido)
                        {
                            continue;
                        }
                        // Si existe ya ese identificador, marcar error en cualquier caso invalido.
                        this.TablaSimbolos[registro.Key] = result;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                else if(token.Tipo == TipoSintaxis.BoolKeyword)
                {
                    var result = EvaluarExpresionLogica((Expresion)token.Value);
                    this.TablaSimbolos[registro.Key] = result;
                }
                else if (token.Tipo == TipoSintaxis.StringKeyword)
                {
                    if (token.Value is ExpresionStringInvalida)
                    {
                        continue;
                    }
                    var result = EvaluarExpresionString(token.Value as Expresion);
                    this.TablaSimbolos[registro.Key] = result;
                }

            }

            return Diagnostico;
        }


        // Este metodo revisara que los tipos de las declaraciones sean correctos.
        private void TableLookup()
        {
            foreach (var item in TablaSimbolos)
            {

            }
        }

        public Token EvaluarExpresionString(Expresion nodo)
        {
            if (nodo is ExpresionString n)
            {
                string valor = n.Valor.Value.ToString();
                var tokenString = new Token(TipoSintaxis.TokenInteger, 0, valor, valor);
                return tokenString;
            }
                        
            if (nodo is ExpresionIdentificador id)
            {
                var identificador = id.Identificador;
                var tokenString = (TablaSimbolos[identificador.Value.ToString()] as Token).Value;

                if (!TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    Diagnostico.Add($"ERROR: Variable no declarada <{identificador}>");
                }
                else if (!(tokenString is null))
                {
                    return new Token(TipoSintaxis.TokenString, 0, tokenString.ToString(), tokenString.ToString());
                }

                // Invalid string
                return null;
            }


            if (nodo is ExpresionBinaria b)
            {
                var izquierda = EvaluarExpresionString(b.Izquierda);
                var derecha = EvaluarExpresionString(b.Derecha);

                if (b.Operador.Tipo == TipoSintaxis.TokenMas)
                {
                    string result = izquierda.Value.ToString() + derecha.Value.ToString();
                    return new Token(TipoSintaxis.TokenString, 0, result, result);
                }
                                
                else
                    throw new Exception($"Operador binario inesperado: {b.Operador.Tipo}");
            }

            if (nodo is ExpresionEnParentesis p)
                return EvaluarExpresionString(p.Expresion);

            throw new Exception($"Nodo inesperado {nodo.Tipo}");
        }

        public Token EvaluarExpresionAritmetica(Expresion nodo, TipoSintaxis tipo)
        {
            if (nodo is ExpresionNumericaInvalida invalidExpression)
            {
                return new Token(TipoSintaxis.TokenInvalido, 0, null, null);
            }

            if (nodo is ExpresionEntera n)
            {
                int valor = Convert.ToInt32((Convert.ToDouble(n.Numero.Value)));
                var tokenEntero = new Token(TipoSintaxis.TokenInteger, 0, valor.ToString(), valor);
                return tokenEntero;
            }

            //if (nodo is ExpresionEntera n)
            //{
            //    int valor = Convert.ToInt32((Convert.ToDouble(n.Numero.Value)));
            //    var tokenEntero = new Token(TipoSintaxis.TokenInteger, 0, valor.ToString(), valor);
            //    return tokenEntero;
            //}


            if (nodo is ExpresionDecimal nd)
            {
                float valor = (float) nd.Numero.Value;
                var tokenDecimal = new Token(TipoSintaxis.TokenDecimal, 0, valor.ToString(), valor);
                return tokenDecimal;
            }

            if (nodo is ExpresionIdentificador id)
            {
                var identificador = id.Identificador;
                var tokenNumero = (TablaSimbolos[identificador.Value.ToString()] as Token).Value;

                if (!TablaSimbolos.ContainsKey(identificador.Value.ToString()))
                {
                    Console.WriteLine($"ERROR: Variable no declarada <{identificador}>");
                    Diagnostico.Add($"ERROR: Variable no declarada <{identificador}>");
                }
                else if (!(tokenNumero is null))
                {
                    var stringData = tokenNumero.ToString();
                    // if contains a dot, then is decimal.
                    if (stringData.Contains("."))
                    {
                        float.TryParse(stringData, out var val);
                        return new Token(TipoSintaxis.TokenDecimal, 0, val.ToString(), val);
                    }

                    int.TryParse(stringData, out var value);
                    return new Token(TipoSintaxis.TokenInteger, 0, value.ToString(), value);
                }

                // Invalid number
                return null;
            }


            if (nodo is ExpresionBinaria b)
            {
                var izquierda = EvaluarExpresionAritmetica(b.Izquierda, tipo);
                var derecha = EvaluarExpresionAritmetica(b.Derecha, tipo);

                if (b.Operador.Tipo == TipoSintaxis.TokenMas)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenInteger &&
                        derecha.Tipo is TipoSintaxis.TokenInteger)
                    {
                        int result = int.Parse(izquierda.Value.ToString()) + int.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) + float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }

                else if (b.Operador.Tipo == TipoSintaxis.TokenMenos)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenInteger &&
                        derecha.Tipo is TipoSintaxis.TokenInteger)
                    {
                        int result = int.Parse(izquierda.Value.ToString()) - int.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) - float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }

                else if (b.Operador.Tipo == TipoSintaxis.TokenMultiplicacion)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenInteger &&
                        derecha.Tipo is TipoSintaxis.TokenInteger)
                    {
                        int result = int.Parse(izquierda.Value.ToString()) * int.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) * float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }
                else if (b.Operador.Tipo == TipoSintaxis.TokenDivision)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) / float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }
                else if (b.Operador.Tipo == TipoSintaxis.TokenPotencia)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) + float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }
                else if (b.Operador.Tipo == TipoSintaxis.TokenModulo)
                {
                    if (izquierda.Tipo is TipoSintaxis.TokenInteger &&
                        derecha.Tipo is TipoSintaxis.TokenInteger)
                    {
                        int result = int.Parse(izquierda.Value.ToString()) % int.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                    if (izquierda.Tipo is TipoSintaxis.TokenDecimal ||
                        derecha.Tipo is TipoSintaxis.TokenDecimal)
                    {
                        float result = float.Parse(izquierda.Value.ToString()) % float.Parse(derecha.Value.ToString());
                        return new Token(TipoSintaxis.TokenInteger, 0, result.ToString(), result);
                    }
                }
                else
                    throw new Exception($"Operador binario inesperado: {b.Operador.Tipo}");
            }

            if (nodo is ExpresionEnParentesis p)
                return EvaluarExpresionAritmetica(p.Expresion, tipo);

            throw new Exception($"Nodo inesperado {nodo.Tipo}");
        }

        private bool EvaluarExpresionLogica(Expresion nodo)
        {
            if (nodo is ExpresionBool n)
                return Boolean.Parse(n.TokenBool.Value.ToString());

            if (nodo is ExpresionBinaria b)
            {
                var izquierda = EvaluarExpresionLogica(b.Izquierda);
                var derecha = EvaluarExpresionLogica(b.Derecha);

                if (b.Operador.Tipo == TipoSintaxis.TokenAnd)
                    return izquierda && derecha;
                else if (b.Operador.Tipo == TipoSintaxis.TokenOr)
                    return izquierda || derecha;
                else if (b.Operador.Tipo == TipoSintaxis.TokenIgualIgual)
                    return izquierda == derecha;
                else if (b.Operador.Tipo == TipoSintaxis.TokenNotIgual)
                    return izquierda != derecha;
                else
                    throw new Exception($"Operador binario inesperado: {b.Operador.Tipo}");
            }

            if (nodo is ExpresionEnParentesis p)
                return EvaluarExpresionLogica(p.Expresion);

            throw new Exception($"Nodo inesperado {nodo.Tipo}");
        }
    }
}