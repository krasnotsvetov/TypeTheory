using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LambdaElements
{
    public class Application : LambdaExpression
    {
        public LambdaExpression Left { get; private set; }
        public LambdaExpression Right { get; private set; }

        public Application(LambdaExpression left, LambdaExpression right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return  "(" + Left.ToString() + " " + Right.ToString() + ")";
        }

        public override LambdaExpression GetNotation()
        {
            int t = 0;
            return _makeNotation(new Dictionary<Variable, int>(),ref t);
        }

        internal override LambdaExpression _makeNotation(Dictionary<Variable, int> notation,ref int variableCount)
        {
            return new Application(Left._makeNotation(notation,ref variableCount), Right._makeNotation(notation,ref variableCount));
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Application)) return false;
            var app = obj as Application;
            return (app.Left.Equals(Left) && app.Right.Equals(Right));
        }

        public override LambdaExpression Clone()
        {
            return new Application(Left.Clone(), Right.Clone());
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() * 631 + Right.GetHashCode() * 211;
        }
    }
}
