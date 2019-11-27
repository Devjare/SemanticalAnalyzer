
using SemanticalAnalyzer.CodeAnalysis;
using SemanticalAnalyzer.CodeAnalysis.Expresiones_Individuales;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class Evaluador
    {
        private readonly Expresion _raiz;
        public Dictionary<String, Object> TablaSintaxis { get; set; }
        public List<String> Diagnostico;
        public List<String> Salida;
        public List<Token> TokenList;
        public long SintaxTimeTaken, LexTimeTaken;

        public Dictionary<String, Object> TablaResultados;
        public int ContadorIfs;

        public Evaluador(Expresion raiz)
        {
            this._raiz = raiz;
        }

        public Evaluador()
        {
            Diagnostico = new List<String>();
            Salida = new List<string>();
            TablaSintaxis = new Dictionary<string, object>();
            TablaResultados = new Dictionary<string, object>();
            ContadorIfs = 0;
        }
        public List<Token> ListaExpresiones;
        public List<String> Evaluar(String codigo)
        {
            var parser = new AnalizadorSintactico(codigo);
            LexTimeTaken = parser.LexTimeTaken;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var arbol = parser.Analizar();

            TokenList = parser.TokenList;

            ListaExpresiones = new List<Token>();

            // Agregamos los errores sintacticos.
            Diagnostico.AddRange(arbol.Diagnostico);

            // PRUEBA EVALUACION DIRECTA DEL ARBOL.
            foreach (var expresion in arbol.Raiz.ListaExpresiones)
            {

                if (expresion is ExpresionMain expMain)
                {
                    var resultado = EvaluarBloqueCodigo(expMain.Sentencias.ListaExpresiones);
                    if (resultado == false)
                    {
                        return Diagnostico;
                    }
                }
            }

            stopwatch.Stop();
            SintaxTimeTaken = stopwatch.ElapsedMilliseconds;

            return Diagnostico;
        }

        private bool EvaluarBloqueCodigo(List<Expresion> expresiones)
        {
            bool tieneIf = false;
            foreach (var expresion in expresiones)
            {
                if (expresion is ExplresionDeclaracion declaracion)
                {
                    var identificador = declaracion.Identificador.Value.ToString();
                    var exp = declaracion.Expresion;

                    Token resultadoExpresion = null;

                    switch (exp.Tipo)
                    {
                        case TipoSintaxis.ExpresionEntera:

                            resultadoExpresion = EvaluarExpresionAritmetica(exp as ExpresionEntera);
                            TablaResultados[identificador] = new Token(TipoSintaxis.TokenInteger,
                                resultadoExpresion.Position, resultadoExpresion.Value.ToString(), resultadoExpresion.Value.ToString());

                            break;
                        case TipoSintaxis.ExpresionDecimal:

                            resultadoExpresion = EvaluarExpresionAritmetica(exp as ExpresionDecimal);
                            TablaResultados[identificador] = new Token(TipoSintaxis.TokenDecimal,
                            resultadoExpresion.Position, resultadoExpresion.Value.ToString(), resultadoExpresion.Value.ToString());

                            break;
                        case TipoSintaxis.ExpresionStirng:

                            resultadoExpresion = EvaluarExpresionString(exp as ExpresionString);
                            TablaResultados[identificador] = new Token(TipoSintaxis.TokenString,
                            resultadoExpresion.Position, resultadoExpresion.Value.ToString(), resultadoExpresion.Value.ToString());

                            break;
                        case TipoSintaxis.ExpresionBinaria:

                            resultadoExpresion = EvaluarExpresionBinaria(exp);
                            TablaResultados[identificador] = resultadoExpresion;

                            break;
                    }
                }
                else if (expresion is ExpresionIf ifExpr)
                {
                    tieneIf = true;
                    var exp = ifExpr.Expresion;
                    Token result = null;
                    if (exp is ExpresionBooleana expBool)
                    {
                        result = EvaluarExpresionBoolean(expBool);
                        TablaResultados[$"if({ContadorIfs++})"] = result;
                    }
                    else if (exp is ExpresionRelacional expRel)
                    {
                        result = EvaluarExpresionRelacional(expRel);
                        TablaResultados[$"if({ContadorIfs++})"] = result;
                    }
                    else if (exp is ExpresionLogica exprLogica)
                    {
                        result = new Token(TipoSintaxis.ExpresionLogica, -1, exprLogica.Value.ToString(), exprLogica.Value.ToString());
                        TablaResultados[$"if({ContadorIfs++})"] = result;
                    }

                    var resultado = Boolean.Parse(result.Value.ToString());

                    if (resultado == true)
                    {
                        bool ejecuccionExitosa = EvaluarBloqueCodigo(ifExpr.Sentencias.ListaExpresiones);
                        if (!ejecuccionExitosa) return false;
                        tieneIf = false;
                    }

                }
                else if (expresion is ExpresionElse elseExpr && tieneIf)
                {
                    bool ejecuccionExitosa = EvaluarBloqueCodigo(elseExpr.Sentencias.ListaExpresiones);
                    if (!ejecuccionExitosa) return false;
                }
                else if (expresion is ExpresionFuncionPrintln printlnExpr)
                {
                    var exp = printlnExpr.Expresion;

                    Token resultadoExp = null;

                    switch (exp.Tipo)
                    {
                        case TipoSintaxis.ExpresionDecimal:
                        case TipoSintaxis.ExpresionEntera:

                            resultadoExp = EvaluarExpresionAritmetica(exp);

                            break;
                        case TipoSintaxis.ExpresionStirng:

                            resultadoExp = EvaluarExpresionString(exp);

                            break;
                        case TipoSintaxis.ExpresionBinaria:

                            resultadoExp = EvaluarExpresionBinaria(exp);

                            //var izq = (exp as ExpresionBinaria).Izquierda;

                            //if (izq is ExpresionIdentificador)
                            //{
                            //    var exprIdent = izq as ExpresionIdentificador;
                            //    if (!TablaResultados.ContainsKey(exprIdent.Identificador.Value.ToString()))
                            //    {
                            //        Diagnostico.Add($"Variable <{exprIdent.Identificador.Value.ToString()}> no declarada.");
                            //        return false;
                            //    }
                            //    else
                            //    {
                            //        var valor = TablaResultados[exprIdent.Identificador.Value.ToString()] as Token;

                            //        switch (valor.Tipo)
                            //        {
                            //            case TipoSintaxis.TokenDecimal:
                            //            case TipoSintaxis.TokenInteger:
                            //                resultadoExp = EvaluarExpresionAritmetica(exp);
                            //                break;
                            //            case TipoSintaxis.TokenString:
                            //                resultadoExp = EvaluarExpresionString(exp);
                            //                break;
                            //        }
                            //    }
                            //}
                            //else if (izq is ExpresionString) resultadoExp = EvaluarExpresionString(exp);
                            //else resultadoExp = EvaluarExpresionAritmetica(exp);
                            break;
                        case TipoSintaxis.ExpresionIdentificador:

                            var expIdent = exp as ExpresionIdentificador;

                            var llave = expIdent.Identificador.Value.ToString();

                            if (!TablaResultados.ContainsKey(llave))
                            {
                                Diagnostico.Add($"Error variable <{llave}> no encontrada.");
                            }
                            else
                            {
                                var token = TablaResultados[llave] as Token;
                                switch (token.Tipo)
                                {
                                    case TipoSintaxis.TokenInteger:
                                    case TipoSintaxis.TokenDecimal:

                                        resultadoExp = EvaluarExpresionAritmetica(exp);

                                        break;
                                    case TipoSintaxis.TokenString:

                                        resultadoExp = EvaluarExpresionString(exp);
                                        break;
                                }
                            }

                            break;
                    }

                    Salida.Add(resultadoExp.Value.ToString());
                }
            }

            return true;
        }

        private Token EvaluarExpresionBoolean(Expresion expresion)
        {
            var expresionBooleana = expresion as ExpresionBooleana;

            Token resultado = null;

            var izq = expresionBooleana.Izquierda;
            Token resIzq = null;

            switch (izq.Tipo)
            {
                case TipoSintaxis.ExpresionLogica:
                    var resToken = (izq as ExpresionLogica).Value;
                    resIzq = new Token(TipoSintaxis.ExpresionLogica, -1, resToken.Value.ToString(), resToken.Value.ToString());
                    break;
                case TipoSintaxis.ExpresionRelacional:
                    resIzq = EvaluarExpresionRelacional(izq);
                    break;
                case TipoSintaxis.ExpresionBooleana:
                    resIzq = EvaluarExpresionBoolean(izq);
                    break;
            }

            var der = expresionBooleana.Derecha;
            Token resDer = null;

            switch (der.Tipo)
            {
                case TipoSintaxis.ExpresionLogica:
                    var resToken = (der as ExpresionLogica).Value;
                    resDer = new Token(TipoSintaxis.TokenBool, -1, resToken.Value.ToString(), resToken.Value.ToString());
                    break;
                case TipoSintaxis.ExpresionRelacional:
                    resDer = EvaluarExpresionRelacional(der);
                    break;
                case TipoSintaxis.ExpresionBooleana:
                    resDer = EvaluarExpresionBoolean(der);
                    break;
            }

            var operador = expresionBooleana.Operador;

            bool resIzqBool, resDerBool, boolRes;

            switch (operador.Tipo)
            {
                case TipoSintaxis.TokenAnd:

                    resIzqBool = Boolean.Parse(resIzq.Value.ToString());
                    resDerBool = Boolean.Parse(resDer.Value.ToString());


                    boolRes = resIzqBool && resDerBool;

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenOr:

                    resIzqBool = Boolean.Parse(resIzq.Value.ToString());
                    resDerBool = Boolean.Parse(resDer.Value.ToString());

                    boolRes = resIzqBool || resDerBool;

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
            }

            return resultado;
        }

        private Token EvaluarExpresionRelacional(Expresion expresion)
        {
            var expresionRelacional = expresion as ExpresionRelacional;

            Token resultado = null;

            var izq = expresionRelacional.Izquierda;
            Token resIzq = null;

            switch (izq.Tipo)
            {
                case TipoSintaxis.ExpresionEntera:
                    resIzq = (izq as ExpresionEntera).Numero;
                    break;
                case TipoSintaxis.ExpresionDecimal:
                    resIzq = (izq as ExpresionDecimal).Numero;
                    break;
                case TipoSintaxis.ExpresionStirng:
                    resIzq = (izq as ExpresionString).Valor;
                    break;
                case TipoSintaxis.ExpresionIdentificador:
                    var identificador = (izq as ExpresionIdentificador).Identificador;
                    if (TablaResultados.ContainsKey(identificador.Value.ToString()))
                    {
                        var val = TablaResultados[identificador.Value.ToString()] as Token;
                        switch (val.Tipo)
                        {
                            case TipoSintaxis.TokenInteger:
                                resIzq = new Token(TipoSintaxis.TokenInteger, val.Position, val.Text, Int32.Parse(val.Value.ToString()));
                                break;
                            case TipoSintaxis.TokenString:
                                resIzq = new Token(TipoSintaxis.TokenString, val.Position, val.Text, val.Value.ToString());
                                break;
                            case TipoSintaxis.TokenDecimal:
                                resIzq = new Token(TipoSintaxis.TokenDecimal, val.Position, val.Text, Double.Parse(val.Value.ToString()));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case TipoSintaxis.ExpresionLogica:
                    resIzq = (izq as ExpresionLogica).Value;
                    // resIzq = new Token(TipoSintaxis.ExpresionLogica, -1, resToken.Value.ToString(), resToken.Value.ToString());
                    break;
                case TipoSintaxis.ExpresionRelacional:
                    resIzq = EvaluarExpresionRelacional(izq);
                    break;
                case TipoSintaxis.ExpresionBooleana:
                    resIzq = EvaluarExpresionBoolean(izq);
                    break;
            }

            var der = expresionRelacional.Derecha;
            Token resDer = null;

            switch (der.Tipo)
            {
                case TipoSintaxis.ExpresionEntera:
                    resDer = (der as ExpresionEntera).Numero;
                    break;
                case TipoSintaxis.ExpresionDecimal:
                    resDer = (der as ExpresionDecimal).Numero;
                    break;
                case TipoSintaxis.ExpresionStirng:
                    resDer = (der as ExpresionString).Valor;
                    break;
                case TipoSintaxis.ExpresionIdentificador:
                    var identificador = (der as ExpresionIdentificador).Identificador;
                    if (TablaResultados.ContainsKey(identificador.Value.ToString()))
                    {
                        var valor = TablaResultados[identificador.Value.ToString()] as Token;
                        switch (valor.Tipo)
                        {
                            case TipoSintaxis.TokenInteger:
                                resDer = new Token(TipoSintaxis.TokenInteger, valor.Position, valor.Text, Int32.Parse(valor.Value.ToString()));
                                break;
                            case TipoSintaxis.TokenString:
                                resDer = new Token(TipoSintaxis.TokenString, valor.Position, valor.Text, valor.Value.ToString());
                                break;
                            case TipoSintaxis.TokenDecimal:
                                resDer = new Token(TipoSintaxis.TokenDecimal, valor.Position, valor.Text, Double.Parse(valor.Value.ToString()));
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case TipoSintaxis.ExpresionLogica:
                    resDer = (der as ExpresionLogica).Value;
                    // resIzq = new Token(TipoSintaxis.ExpresionLogica, -1, resToken.Value.ToString(), resToken.Value.ToString());
                    break;
                case TipoSintaxis.ExpresionRelacional:
                    resDer = EvaluarExpresionRelacional(der);
                    break;
                case TipoSintaxis.ExpresionBooleana:
                    resDer = EvaluarExpresionBoolean(der);
                    break;
            }

            var operador = expresionRelacional.Operador;

            bool boolRes = false;

            switch (operador.Tipo)
            {

                // TODO Poner case en orden de precedencia de los op relacionales.
                case TipoSintaxis.TokenMenorQue:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) < Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) < Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) < Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) < Int32.Parse(resDer.Value.ToString());

                    break;
                case TipoSintaxis.TokenMenorIgualQue:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) <= Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) <= Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) <= Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) <= Int32.Parse(resDer.Value.ToString());

                    break;
                case TipoSintaxis.TokenMayorQue:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) > Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) > Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) > Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) > Int32.Parse(resDer.Value.ToString());

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenMayorIgualQue:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) >= Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) >= Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) >= Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) >= Int32.Parse(resDer.Value.ToString());

                    break;
                case TipoSintaxis.TokenIgualIgual:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) == Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) == Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) == Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) == Int32.Parse(resDer.Value.ToString());

                    break;
                case TipoSintaxis.TokenNotIgual:

                    if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Int32.Parse(resIzq.Value.ToString()) != Int32.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenInteger && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Int32.Parse(resIzq.Value.ToString()) != Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenDecimal) boolRes = Double.Parse(resIzq.Value.ToString()) != Double.Parse(resDer.Value.ToString());
                    else if (resIzq.Tipo == TipoSintaxis.TokenDecimal && resDer.Tipo == TipoSintaxis.TokenInteger) boolRes = Double.Parse(resIzq.Value.ToString()) != Int32.Parse(resDer.Value.ToString());

                    break;
            }

            resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());
            return resultado;
        }

        public Token EvaluarExpresionBinaria(Expresion expresion)
        {
            bool esString = false;
            var izq = (expresion as ExpresionBinaria).Izquierda;

            Token resultadoIzquierda = null;

            // Expresion Izquierda
            switch (izq.Tipo)
            {
                case TipoSintaxis.ExpresionEntera:

                    resultadoIzquierda = EvaluarExpresionAritmetica(izq as ExpresionEntera);

                    break;
                case TipoSintaxis.ExpresionDecimal:

                    resultadoIzquierda = EvaluarExpresionAritmetica(izq as ExpresionDecimal);

                    break;
                case TipoSintaxis.ExpresionStirng:

                    resultadoIzquierda = EvaluarExpresionString(izq as ExpresionString);
                    esString = true;

                    break;
                case TipoSintaxis.ExpresionIdentificador:
                    var exprIdentificador = izq as ExpresionIdentificador;
                    if (!TablaResultados.ContainsKey(exprIdentificador.Identificador.Value.ToString()))
                    {
                        Diagnostico.Add($"Error, variable <{exprIdentificador.Identificador.Value.ToString()}> no encontrada!");
                        // TODO Terminar Evaluacion
                    }
                    else
                    {
                        var valor = TablaResultados[exprIdentificador.Identificador.Value.ToString()] as Token;
                        switch (valor.Tipo)
                        {
                            case TipoSintaxis.TokenInteger:
                                resultadoIzquierda = EvaluarExpresionAritmetica(new ExpresionEntera(valor));
                                break;
                            case TipoSintaxis.TokenDecimal:
                                resultadoIzquierda = EvaluarExpresionAritmetica(new ExpresionDecimal(valor));
                                break;
                            case TipoSintaxis.TokenString:
                                resultadoIzquierda = EvaluarExpresionString(new ExpresionString(valor));
                                esString = true;
                                break;
                        }
                    }
                    break;
                case TipoSintaxis.ExpresionBinaria:

                    resultadoIzquierda = EvaluarExpresionBinaria(izq);

                    break;
            }

            // Expresion derecha

            var der = (expresion as ExpresionBinaria).Derecha;
            Token resultadoDerecha = null;

            switch (der.Tipo)
            {
                case TipoSintaxis.ExpresionEntera:

                    resultadoDerecha = EvaluarExpresionAritmetica(der as ExpresionEntera);

                    break;
                case TipoSintaxis.ExpresionDecimal:

                    resultadoDerecha = EvaluarExpresionAritmetica(der as ExpresionDecimal);

                    break;
                case TipoSintaxis.ExpresionStirng:

                    resultadoDerecha = EvaluarExpresionString(der as ExpresionString);
                    esString = true;

                    break;
                case TipoSintaxis.ExpresionIdentificador:
                    var exprIdentificador = der as ExpresionIdentificador;
                    if (!TablaResultados.ContainsKey(exprIdentificador.Identificador.Value.ToString()))
                    {
                        Diagnostico.Add($"Error, variable <{exprIdentificador.Identificador.Value.ToString()}> no encontrada!");
                        // TODO Terminar Evaluacion
                    }
                    else
                    {
                        var valor = TablaResultados[exprIdentificador.Identificador.Value.ToString()] as Token;
                        switch (valor.Tipo)
                        {
                            case TipoSintaxis.TokenInteger:
                                resultadoDerecha = EvaluarExpresionAritmetica(new ExpresionEntera(valor));
                                break;
                            case TipoSintaxis.TokenDecimal:
                                resultadoDerecha = EvaluarExpresionAritmetica(new ExpresionDecimal(valor));
                                break;
                            case TipoSintaxis.TokenString:
                                resultadoDerecha = EvaluarExpresionString(new ExpresionString(valor));
                                esString = true;
                                break;
                        }
                    }
                    break;
                case TipoSintaxis.ExpresionBinaria:

                    resultadoDerecha = EvaluarExpresionBinaria(der);

                    break;
            }

            // Evaluar resultados
            var operador = (expresion as ExpresionBinaria).Operador;

            Token resultado = null;

            switch (operador.Tipo)
            {
                case TipoSintaxis.TokenMas:
                    if (esString)
                    {
                        var stringResult = resultadoIzquierda.Value.ToString() + resultadoDerecha.Value.ToString();
                        resultado = new Token(TipoSintaxis.TokenString, -1, stringResult, stringResult);
                    }
                    else
                    {
                        if (resultadoIzquierda.Tipo == TipoSintaxis.TokenInteger)
                        {
                            var intResult = Int32.Parse(resultadoIzquierda.Value.ToString()) + Int32.Parse(resultadoDerecha.Value.ToString());
                            resultado = new Token(TipoSintaxis.TokenInteger, -1, intResult.ToString(), intResult);
                        }
                        else
                        {
                            var decResult = Double.Parse(resultadoIzquierda.Value.ToString()) + Double.Parse(resultadoDerecha.Value.ToString());
                            resultado = new Token(TipoSintaxis.TokenDecimal, -1, decResult.ToString(), decResult);
                        }
                    }
                    break;
                case TipoSintaxis.TokenMenos:
                    if (resultadoIzquierda.Tipo == TipoSintaxis.TokenInteger)
                    {
                        var intResult = Int32.Parse(resultadoIzquierda.Value.ToString()) - Int32.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenInteger, -1, intResult.ToString(), intResult);
                    }
                    else
                    {
                        var decResult = Double.Parse(resultadoIzquierda.Value.ToString()) - Double.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenDecimal, -1, decResult.ToString(), decResult);
                    }
                    break;
                case TipoSintaxis.TokenMultiplicacion:
                    if (resultadoIzquierda.Tipo == TipoSintaxis.TokenInteger)
                    {
                        var intResult = Int32.Parse(resultadoIzquierda.Value.ToString()) * Int32.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenInteger, -1, intResult.ToString(), intResult);
                    }
                    else
                    {
                        var decResult = Double.Parse(resultadoIzquierda.Value.ToString()) * Double.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenDecimal, -1, decResult.ToString(), decResult);
                    }
                    break;
                case TipoSintaxis.TokenDivision:
                    if (resultadoIzquierda.Tipo == TipoSintaxis.TokenInteger)
                    {
                        var intResult = Int32.Parse(resultadoIzquierda.Value.ToString()) / Int32.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenInteger, -1, intResult.ToString(), intResult);
                    }
                    else
                    {
                        var decResult = Double.Parse(resultadoIzquierda.Value.ToString()) / Double.Parse(resultadoDerecha.Value.ToString());
                        resultado = new Token(TipoSintaxis.TokenDecimal, -1, decResult.ToString(), decResult);
                    }
                    break;
            }

            return resultado;
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
                var tokenString = (TablaResultados[identificador.Value.ToString()] as Token).Value;

                if (!TablaResultados.ContainsKey(identificador.Value.ToString()))
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

        public Token EvaluarExpresionAritmetica(Expresion nodo)
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
                float valor = (float)Double.Parse(nd.Numero.Value.ToString());
                var tokenDecimal = new Token(TipoSintaxis.TokenDecimal, 0, valor.ToString(), valor);
                return tokenDecimal;
            }

            if (nodo is ExpresionIdentificador id)
            {
                var identificador = id.Identificador;
                var tokenNumero = (TablaResultados[identificador.Value.ToString()] as Token).Value;

                if (!TablaResultados.ContainsKey(identificador.Value.ToString()))
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
                var izquierda = EvaluarExpresionAritmetica(b.Izquierda);
                var derecha = EvaluarExpresionAritmetica(b.Derecha);

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
                return EvaluarExpresionAritmetica(p.Expresion);

            throw new Exception($"Nodo inesperado {nodo.Tipo}");
        }

    }
}