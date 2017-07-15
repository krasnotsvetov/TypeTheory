using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LambdaElements
{
    public class LetExpression : LambdaExpression
    {
        public LambdaExpression Left { get; }
        public LambdaExpression Right { get; }
        public Variable Variable { get; }


        public LetExpression(Variable variable, LambdaExpression left, LambdaExpression right)
        {
            this.Variable = variable;
            this.Left = left;
            this.Right = right;
        }

        public override LambdaExpression Clone()
        {
            return new LetExpression((Variable)Variable.Clone(), Left.Clone(), Right.Clone());
        }

        public override LambdaExpression GetNotation()
        {
            throw new NotImplementedException();
        }

        internal override LambdaExpression _makeNotation(Dictionary<Variable, int> notation, ref int variableCount)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return Variable.ToString() + " = " + Left.ToString() + " in " + Right.ToString();
        }
    }
}
