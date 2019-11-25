using System.Linq;
using System.Collections.Generic;

namespace ExpressionEvaluator.CodeAnalysis
{
    public class Token : NodoSintaxis
    {
        public Token(TipoSintaxis kind, int position, string text, object value)
        {
            Tipo = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override TipoSintaxis Tipo { get; }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<NodoSintaxis> GetChildren()
        {
            return Enumerable.Empty<NodoSintaxis>();
        }

        public override string ToString()
        {
            return $"[ Kind={Tipo}, Position={Position}, Text={Text}, Value={Value} ]";
        }
    }

}