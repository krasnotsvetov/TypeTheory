using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    public class Reducer
    {
        Dictionary<Application, WeakReference<LambdaExpression>> cache = new Dictionary<Application, WeakReference<LambdaExpression>>();

        public LambdaExpression Reduce(LambdaExpression lambda)
        {
            var notation = lambda.GetNotation();
            return ReduceWithoutNotation(notation);
        }

        private LambdaExpression ReduceWithoutNotation(LambdaExpression lambda)
        {
            var r0 = FindRedecsAndReduce(lambda, new HashSet<Variable>());
            return r0 ?? lambda;
        }

        ///
        ///  \x.\x.x x x
        /// 

        private LambdaExpression FindRedecsAndReduce(LambdaExpression lambda, HashSet<Variable> variables)
         {
            if (IsBetaRedecs(lambda))
            {
                return InnerReduce(lambda, variables);
            }

            if (lambda is Application)
            {
                var r0 = FindRedecsAndReduce((lambda as Application).Left, variables);
                if (r0 != null)
                {
                    return new Application(r0, (lambda as Application).Right);
                }
                var r1 = FindRedecsAndReduce((lambda as Application).Right, variables);
                if (r1 != null)
                {
                    return new Application((lambda as Application).Left, r1);
                }
            }

            if (lambda is Abstraction)
            {
                var abstraction = lambda as Abstraction;
                variables.Add(abstraction.Variable);
                var r0 = FindRedecsAndReduce(abstraction.Expression, variables);
                variables.Remove(abstraction.Variable);
                if (r0 != null)
                {
                    return new Abstraction(abstraction.Variable, r0);
                }
            }
            return null;
        }

        private LambdaExpression InnerReduce(LambdaExpression lambda, HashSet<Variable> variables)
        {
            var app = lambda as Application;
            LambdaExpression r0;
            if (cache.ContainsKey(app as Application) && cache[app].TryGetTarget(out r0))
            {
                return r0;
            }
            var abstraction = app.Left as Abstraction;
            r0 = Subst(abstraction.Expression, abstraction.Variable,  app.Right, variables);
            if (r0 == null)
            {
                r0 = abstraction.Expression;
            }
            if (cache.ContainsKey(app))
            {
                cache[app].SetTarget(r0);
            }
            else
            {
                cache[app] = new WeakReference<LambdaExpression>(r0);
            }
            return r0;
        }

        private LambdaExpression Subst(LambdaExpression expr, Variable variable, LambdaExpression subst, HashSet<Variable> variables)
        {
            if (expr is Variable)
            {
                if (expr.Equals(variable)) return Rename(variables, subst);
                return null;
            }
            if (expr is Abstraction)
            {
                
                var abstraction = expr as Abstraction;

                if (variables.Contains(abstraction.Variable)) throw new Exception("Assert");

                variables.Add(abstraction.Variable);
                var r0 = Subst(abstraction.Expression, variable, subst, variables);
                variables.Remove(abstraction.Variable);
                if (r0 == null) return abstraction;
                return new Abstraction(abstraction.Variable, r0);
            }

            if (expr is Application)
            {
                var application = expr as Application;
                var s0 = Subst(application.Left, variable, subst, variables);
                var s1 = Subst(application.Right, variable, subst, variables);
                if (s0 == null && s1 == null)
                {
                    return null;
                }
                return new Application(s0 ?? application.Left, s1 ?? application.Right);
            }

            throw new NotImplementedException(expr.GetType().Name + " doesn't support subst");
        }

        private LambdaExpression Rename(HashSet<Variable> variables, LambdaExpression expr)
        {
            return InnerRename(variables, new Dictionary<Variable, string>(), expr) ?? expr;
        }

        private LambdaExpression InnerRename(HashSet<Variable> variables, Dictionary<Variable, string> newNames, LambdaExpression expr)
        {
            if (expr is Variable)
            {
                var var = expr as Variable;
                if (newNames.ContainsKey(var)) return new Variable(newNames[var]);
                return null;
            }

            if (expr is Application)
            {
                var app = expr as Application;
                var r0 = InnerRename(variables, newNames, app.Left);
                var r1 = InnerRename(variables, newNames, app.Right);
                if (r0 == null && r1 == null)
                {
                    return null;
                }
                return new Application(r0 ?? app.Left, r1 ?? app.Right);
            }

            if (expr is Abstraction)
            {
                var abst = expr as Abstraction;
                var startName = abst.Variable;
                while (variables.Contains(startName))
                {
                    startName = new Variable(startName.Name + "'");
                }
                variables.Add(startName);
                if (!abst.Variable.Equals(startName))
                {
                    newNames[abst.Variable] = startName.Name;
                }
                var r0 = InnerRename(variables, newNames, abst.Expression);
                variables.Remove(startName);
                if (r0 == null && !newNames.ContainsKey(abst.Variable))
                {
                    return null;
                }  else
                {
                    return new Abstraction(startName, r0 ?? abst.Expression);
                }
            }

            throw new NotImplementedException(expr.GetType().Name + " doesn't support rename");
        }

        private bool IsBetaRedecs(LambdaExpression expression)
        {
            var app = expression as Application;

            return app != null && app.Left is Abstraction;
        }

        public LambdaExpression FullReduce(LambdaExpression lambda)
        {
            var notation = lambda.GetNotation();

            LambdaExpression current = notation, next = null;
            int reductionCount = 0;
            while (!(next = ReduceWithoutNotation(current)).Equals(current))
            {
              //  Console.WriteLine(current);
               // Console.WriteLine(next);
               // Console.WriteLine("------------------------------------------");
                current = next;
                reductionCount++;
                if (reductionCount % 1000 == 0)
                {
                    Console.WriteLine("Reduction count: " + reductionCount);
                }
            }
            Console.WriteLine("Reduction count: " + reductionCount);
            return current.GetNotation();
        }
    }
}
