using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LambdaElements
{
    public class Variable : LambdaExpression
    {
        public string Name { get; private set; }

        public Variable(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }



        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Variable)) return false;
            var v = obj as Variable;
            return v.Name.Equals(Name);
        }

        public override LambdaExpression GetNotation()
        {
            return new Variable("a0");
        }

        internal override LambdaExpression _makeNotation(Dictionary<Variable, int> notation, ref int variableCount)
        {

            if (!notation.ContainsKey(this))
            {
                notation[this] = variableCount++;
                return new Variable("a" + notation[this].ToString());
            }
            return new Variable("a" + notation[this].ToString());
        }

        public override LambdaExpression Clone()
        {
            return new Variable(Name.Clone().ToString());
        }
    }
}
