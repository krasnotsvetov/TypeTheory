using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LambdaElements
{
    public class Abstraction : LambdaExpression
    {

        
        public Variable Variable { get; private set;} 
        public LambdaExpression Expression { get; private set;}

        public Abstraction(Variable variable, LambdaExpression expression)
        {
            this.Variable = variable;
            this.Expression = expression;
        }

        public override string ToString()
        {
            return $"(\\{Variable.Name}.({Expression.ToString()}))";
        }

        public override LambdaExpression GetNotation()
        {
            var dic = new Dictionary<Variable, int>();
            dic[Variable] = 0;
            int t = 1;
            return _makeNotation(dic,ref  t);
        }

        internal override LambdaExpression _makeNotation(Dictionary<Variable, int> notation,ref int variableCount)
        {
            if (notation.ContainsKey(Variable))
            {
                int temp = notation[Variable];
                notation[Variable] = variableCount++;
                var rv = new Abstraction(new Variable("a" + notation[Variable].ToString()), Expression._makeNotation(notation,ref variableCount));
                notation[Variable] = temp;
                return rv;
            }
            else
            {
                notation[Variable] = variableCount++;
                var rv = new Abstraction(new Variable("a" + notation[Variable].ToString()), Expression._makeNotation(notation,ref variableCount));
                notation.Remove(Variable);
                return rv;
            }
        }


        public override bool Equals(object obj)
        {

            if (!(obj is Abstraction)) return false;
            var abs = obj as Abstraction;
            return (abs.Variable.Equals(Variable) && abs.Expression.Equals(Expression));
            
        }

        public override LambdaExpression Clone()
        {
            return new Abstraction((Variable)Variable.Clone(), Expression.Clone());
        }


        public override int GetHashCode()
        {
            return Variable.GetHashCode() * 997 + Expression.GetHashCode();
        }
    }
}
