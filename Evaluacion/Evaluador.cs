
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
        public Dictionary<String, Object> TablaSimbolos { get; set; }
        public Dictionary<String, Object> TablaSintaxis { get; set; }
        public List<String> Diagnostico;
        public List<String> Salida;
        public List<Token> TokenList;
        public long SintaxTimeTaken, LexTimeTaken;

        public Dictionary<String, Object> TablaResultados;
        public int ContadorIfs;

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

            //// Evaluacion de expresiones en llamadas a println()
            //var listaExpresionesImpresion = parser.Salida;

            //foreach (var expresion in listaExpresionesImpresion)
            //{
            //    if (expresion is ExpresionString expStr)
            //    {
            //        var resultado = EvaluarExpresionString(expStr);
            //        Salida.Add(resultado.Value.ToString());
            //    }

            //    if (expresion is ExpresionBinaria expBin)
            //    {
            //        var izq = expBin.Izquierda;
            //        if (izq != null && izq is ExpresionString)
            //        {
            //            var resultado = EvaluarExpresionString(expBin);
            //            Salida.Add(resultado.Value.ToString());
            //        }
            //        else if (izq != null && (izq is ExpresionEntera || izq is ExpresionDecimal))
            //        {
            //            var resultado = EvaluarExpresionAritmetica(expBin);
            //            Salida.Add(resultado.Value.ToString());
            //        }
            //    }
            //}

            stopwatch.Stop();
            SintaxTimeTaken = stopwatch.ElapsedMilliseconds;

            return Diagnostico;
        }

        private bool EvaluarFuncionMain(Expresion expresionMain)
        {

            var listaExpresionesMain = expresionMain as ExpresionMain;
            

            return true;
        }

        private bool EvaluarBloqueCodigo(List<Expresion> expresiones)
        {
            foreach (var expresion in expresiones)
            {
                if (expresion is ExplresionDeclaracion declaracion)
                {
                    var identificador = declaracion.Identificador;
                    var exp = declaracion.Expresion;

                    Token resultadoExpresion = null;

                    switch (exp.Tipo)
                    {
                        case TipoSintaxis.ExpresionEntera:

                            resultadoExpresion = EvaluarExpresionAritmetica(exp as ExpresionEntera);
                            TablaResultados[declaracion.Identificador.Value.ToString()] = resultadoExpresion.Value.ToString();

                            break;
                        case TipoSintaxis.ExpresionDecimal:

                            resultadoExpresion = EvaluarExpresionAritmetica(exp as ExpresionDecimal);
                            TablaResultados[declaracion.Identificador.Value.ToString()] = resultadoExpresion.Value.ToString();

                            break;
                        case TipoSintaxis.ExpresionStirng:

                            resultadoExpresion = EvaluarExpresionString(exp as ExpresionString);
                            TablaResultados[declaracion.Identificador.Value.ToString()] = resultadoExpresion.Value.ToString();

                            break;
                        case TipoSintaxis.ExpresionBinaria:

                            resultadoExpresion = EvaluarExpresionBinaria(exp);
                            TablaResultados[declaracion.Identificador.Value.ToString()] = resultadoExpresion.Value.ToString();

                            break;
                    }
                }
                else if (expresion is ExpresionIf ifExpr)
                {
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

                    if (Boolean.Parse(result.Value.ToString()) == true)
                    {
                        bool ejecuccionExitosa = EvaluarBloqueCodigo(ifExpr.Sentencias.ListaExpresiones);
                    }
                    else
                    {
                        // Condicion if fallida
                        // TODO finzalizar ejecucion.
                    }

                }
                else if (expresion is ExpresionElse elseExpr)
                {

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

                            var izq = (exp as ExpresionBinaria).Izquierda;

                            if (izq is ExpresionString) resultadoExp = EvaluarExpresionString(exp);
                            else resultadoExp = EvaluarExpresionAritmetica(exp);

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
                    var resToken = izq as ExpresionLogica;
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
                    var resToken = izq as ExpresionLogica;
                    resDer = new Token(TipoSintaxis.ExpresionLogica, -1, resToken.Value.ToString(), resToken.Value.ToString());
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
                    resIzq = (izq as ExpresionIdentificador).Identificador;
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
                    resDer = (izq as ExpresionEntera).Numero;
                    break;
                case TipoSintaxis.ExpresionDecimal:
                    resDer = (izq as ExpresionDecimal).Numero;
                    break;
                case TipoSintaxis.ExpresionStirng:
                    resDer = (izq as ExpresionString).Valor;
                    break;
                case TipoSintaxis.ExpresionIdentificador:
                    resDer = (izq as ExpresionIdentificador).Identificador;
                    break;
                case TipoSintaxis.ExpresionLogica:
                    resDer = (izq as ExpresionLogica).Value;
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

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) < Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) < Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) < Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) < Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenMenorIgualQue:

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) <= Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) <= Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) <= Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) <= Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenMayorQue:

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) > Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if(izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) > Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if(izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) > Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if(izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) > Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenMayorIgualQue:

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) >= Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) >= Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) >= Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) >= Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenIgualIgual:

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) == Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) == Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) == Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) == Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
                case TipoSintaxis.TokenNotIgual:

                    if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) != Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionEntera && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionEntera).Numero.Value.ToString()) != Double.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionDecimal)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) != Int32.Parse((der as ExpresionEntera).Numero.Value.ToString());
                    }
                    else if (izq.Tipo == TipoSintaxis.ExpresionDecimal && der.Tipo == TipoSintaxis.ExpresionEntera)
                    {
                        boolRes = Int32.Parse((izq as ExpresionDecimal).Numero.Value.ToString()) != Int32.Parse((der as ExpresionDecimal).Numero.Value.ToString());
                    }

                    resultado = new Token(TipoSintaxis.ExpresionLogica, -1, boolRes.ToString(), boolRes.ToString());

                    break;
            }

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
                float valor = (float)nd.Numero.Value;
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