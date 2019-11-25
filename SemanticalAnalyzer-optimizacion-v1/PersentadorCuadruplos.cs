using ExpressionEvaluator.CodeAnalysis;
using SemanticalAnalyzer.CodeAnalysis;
using System;
using System.Collections.Generic;

namespace SemanticalAnalyzer
{
    internal class PersentadorCuadruplos
    {
        private Dictionary<string, object> symbolsTable;

        public PersentadorCuadruplos(Dictionary<string, object> symbolsTable)
        {
            this.symbolsTable = symbolsTable;
        }

        // int a = 10;
        // int b = 10 + a;

        //  OPERATOR    OP1     OP2     RES
        //              10               T1
        //      =        a               T1
        // ---------------------------------- FIRST ASSIGNATION.
        //      +       10       a       T2
        //      =       b                T2
        Dictionary<String, String> identificadores = new Dictionary<String, String>();
        internal Dictionary<String, String> Procesar()
        {
            var cuadruplos = new Dictionary<String, String>();
            var cont = 0;
            // item -> key: String identifier, value: Expression
            foreach (var item in symbolsTable)
            {
                var expresion = (item.Value as Token).Value as Expresion;
                if (expresion is ExpresionBinaria bin)
                {
                    var valor = GetOperation(bin, ref cont);
                    identificadores[item.Key] = $"T{cont++}";
                    cuadruplos[item.Key] = valor;
                    continue;
                }
                else
                {
                    identificadores[item.Key] = cuadruplos[item.Key] = $"T{cont++}";
                }
            }

            return cuadruplos;
        }


        public string GetOperation(ExpresionBinaria expr, ref int cont)
        {
            string result = "";
            var izq = trasverse(expr.Izquierda, ref cont);
            var op = expr.Operador != null ? expr.Operador.Text.ToString() : "";
            var der = expr.Derecha != null ? trasverse(expr.Derecha, ref cont) : "";
            return izq + op + der;
        }
        public String trasverse(Expresion expr, ref int cont)
        {

            if (expr is ExpresionBinaria binaryExp)
            {
                var izq = trasverse(binaryExp.Izquierda, ref cont);
                var op = binaryExp.Operador.Text.ToString();
                var der = trasverse(binaryExp.Derecha, ref cont);
                return izq + op + der;
                // return izq + op + der;
            }
            else if (expr is ExpresionDecimal decExpr)
            {
                return $"T{cont++}";
                //return decExpr.Numero.Value.ToString();
            }
            else if (expr is ExpresionIdentificador idExpr)
            {
                var key = idExpr.Identificador.Value.ToString();
                if (identificadores.ContainsKey(key))
                {
                    return identificadores[key];
                }
                return $"T{cont++}"; //idExpr.Identificador.Value.ToString();
            }

            return $"T{cont++}"; // (expr as ExpresionEntera).Numero.Value.ToString();
        }

        //private string ShrinkExpression(string expression)
        //{
        //    var replacedString = Regex.Replace(expression, " ", "");
        //    return replacedString;
        //}
    }
}