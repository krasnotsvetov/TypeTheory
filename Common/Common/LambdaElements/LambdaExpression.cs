using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LambdaElements
{
    public abstract class LambdaExpression 
    {
        public abstract LambdaExpression GetNotation();

        internal abstract LambdaExpression _makeNotation(Dictionary<Variable, int> notation, ref int variableCount);

        public abstract LambdaExpression Clone();
    }
}
