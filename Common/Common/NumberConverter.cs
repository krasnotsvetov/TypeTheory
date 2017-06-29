using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LambdaConverter
    {
        private LambdaConverter() { }

        public static LambdaExpression UInt32ToLambda(UInt32 number)
        {
            LambdaExpression expression = new Variable("x");
            for (int i = 1; i <= number; i++)
            {
                expression = new Application(new Variable("f"), expression);
            }

            return new Abstraction(new Variable("f"), new Abstraction(new Variable("x"), expression));
        }

        public static UInt32 LambdaToUInt32(LambdaExpression lambda)
        {
            var firstAbstraction = lambda as Abstraction;
            if (firstAbstraction == null) throw new FormatException();
            var secondAbstraction = firstAbstraction.Expression as Abstraction;
            if (secondAbstraction == null) throw new FormatException();
            if (secondAbstraction.Expression is Variable)
            {
                var v = secondAbstraction.Expression as Variable;
                if (v.Equals(secondAbstraction.Variable)) return 0;
                throw new FormatException();
            }

            var startApplication = secondAbstraction.Expression as Application;
            if (startApplication == null) throw new FormatException();

            var variable = startApplication.Left as Variable;
            if (!variable.Equals(firstAbstraction.Variable)) throw new FormatException();

            UInt32 value = 1;
            while (startApplication.Right is Application)
            {
                startApplication = startApplication.Right as Application;
                variable = startApplication.Left as Variable;
                if (!variable.Equals(firstAbstraction.Variable)) throw new FormatException();
                value++;
            }

            variable = startApplication.Right as Variable;

            if (variable == null || !variable.Equals(secondAbstraction.Variable))
            {
                throw new FormatException();
            }

            return value;
        }
    }
}
