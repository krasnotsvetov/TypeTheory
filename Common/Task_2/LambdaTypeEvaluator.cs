using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_2.LambdaType;

namespace Task_2
{
    public class LambdaTypeEvaluator
    {
        private LambdaExpression lambda;
        private Dictionary<Variable, IType> Context = new Dictionary<Variable, IType>();
        public LambdaTypeEvaluator(LambdaExpression lambda)
        {
            this.lambda = lambda;
        }

        public IType GetLambdaType()
        {
            HashSet<Equation> equations;
            var variableType = new Dictionary<Variable, IType>();
            IType type = CalculateType(lambda, out equations, variableType);

            var result = new Unificator(equations).Solve();


            foreach (var var in variableType.Keys)
            {
                Context[var] = Subst(variableType[var], result);
            }
            return Subst(type, result);
        }
    
        public static IType Subst(IType expr, Dictionary<SingleType, IType>  map)
        {
            if (expr is SingleType)
            {
                var singleType = expr as SingleType;
                if (map.ContainsKey(singleType)) return map[singleType];
                return expr;
            } else if (expr is Implication)
            {
                var impl = expr as Implication;
                var left = Subst(impl.Left, map);
                var right = Subst(impl.Right, map);
                if (left != impl.Left || right != impl.Right)
                {
                    return new Implication(left, right);
                }
                return impl;
            } else 
            {
                throw new Exception("Unsupported type");
            }
        }

        public void PrintContext(StreamWriter sw)
        {
            foreach (var kvp in Context)
            {
                sw.WriteLine(kvp.Key + " : " + kvp.Value);
            }
        }

        private IType CalculateType(LambdaExpression lambda, out HashSet<Equation> equations, Dictionary<Variable, IType> type)
        {
            if (lambda is Abstraction)
            {
                var abstraction = lambda as Abstraction;
                IType prev = null;
                if (type.ContainsKey(abstraction.Variable))
                {
                    prev = type[abstraction.Variable];
                }

                var variableType = type[abstraction.Variable] = new SingleType(NextName());
                var exprType = CalculateType(abstraction.Expression, out equations, type);
                if (prev != null)
                {
                    type[abstraction.Variable] = prev;
                } else
                {
                    type.Remove(abstraction.Variable);
                }
                return new Implication(variableType, exprType);
               
            } else if (lambda is Application)
            {
                var application = lambda as Application;
                HashSet<Equation> left;
                HashSet<Equation> right;

                var leftType = CalculateType(application.Left, out left, type);
                var rightType = CalculateType(application.Right, out right, type);

                equations = new HashSet<Equation>(left);
                foreach (var e in right) equations.Add(e);
                var newType = new SingleType(NextName());
                equations.Add(new Equation(leftType, new Implication(rightType, newType)));

                return newType;

            }
            else if (lambda is Variable)
            {
                var variable = lambda as Variable;
                equations = new HashSet<Equation>();
                if (!type.ContainsKey(variable))
                {
                    type[variable] = new SingleType(NextName());
                }
                return type[variable];
            }
            throw new Exception();
        }

        int name = 0;
        private string NextName()
        {
            return "'t" + (name++);
        }
    }
}
