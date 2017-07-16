using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_2.LambdaType;

namespace Task_2
{
    public class Unificator
    {
        private HashSet<Equation> equations;

        public Unificator(IEnumerable<Equation> equations)
        {
            this.equations = new HashSet<Equation>(equations);
        }

        public Dictionary<SingleType, IType> Solve()
        {
            bool isChanged = true;
            while (isChanged)
            {
                isChanged = false;
                var newEquations = new HashSet<Equation>();

                //TODO: it's only placeholder fix!
                bool isChanged2 = true;
                while (isChanged2)
                {
                    isChanged2 = false;
                    /// first step
                    foreach (var eq in equations)
                    {
                        if (!(eq.Left is SingleType) && eq.Right is SingleType)
                        {
                            isChanged = true;
                            isChanged2 = true;
                            newEquations.Add(new Equation(eq.Right, eq.Left));
                        }
                        else if (eq.Left.Equals(eq.Right))
                        {
                            isChanged = true;
                            isChanged2 = true;
                        }
                        else if (eq.Left is Implication && eq.Right is Implication)
                        {
                            isChanged = true;
                            isChanged2 = true;
                            var left = eq.Left as Implication; var right = eq.Right as Implication;
                            newEquations.Add(new Equation(left.Left, right.Left));
                            newEquations.Add(new Equation(left.Right, right.Right));
                        }
                        else
                        {
                            newEquations.Add(eq);
                        }
                    }
                    if (isChanged2)
                    {
                        equations = newEquations;
                        newEquations = new HashSet<Equation>();
                    }
                }

                ///secondStep, find x = T ,where T doesn not contain x and subst T to x for each equation
                var resultEquations = new List<Equation>(newEquations);
                Equation toSubst = null;
                for (int i = 0; i < resultEquations.Count; i++)
                {
                    var eq = resultEquations[i];
                    if (eq.Left is SingleType)
                    {
                        ThrowIfContainVariable((SingleType)eq.Left, eq.Right);
                        toSubst = eq;
                        for (int j = 0; j < resultEquations.Count; j++)
                        {
                            var toChange = resultEquations[j];
                            if (toChange == toSubst) continue;
                            var newEquation = Substitute(toChange, (SingleType)toSubst.Left, toSubst.Right);
                            if (newEquation != toChange)
                            {
                                resultEquations[j] = newEquation;
                                isChanged = true;
                            }
                        }
                    }
                }
                equations = new HashSet<Equation>(resultEquations);
            }
            return equations.ToDictionary(t => (SingleType)t.Left, k => k.Right);
        }

        private void ThrowIfContainVariable(SingleType variable, IType expr)
        {
            if (expr is Implication)
            {
                var impl = expr as Implication;
                if (impl.Left.Equals(variable)) throw new UnificatorUnresolvedExpection("The system can't be resolved");
                if (impl.Right.Equals(variable)) throw new UnificatorUnresolvedExpection("The system can't be resolved");
                if (!(impl.Left is SingleType)) ThrowIfContainVariable(variable, (Implication)impl.Left);
                if (!(impl.Right is SingleType)) ThrowIfContainVariable(variable, (Implication)impl.Right);
            } else
            if (expr is SingleType)
            {
                if (expr.Equals(variable)) throw new Exception();
            } else 
            throw new Exception("Unsupported type");
        }

        private Equation Substitute(Equation eq, SingleType variable, IType toSubst)
        {
            var left = Substitute(eq.Left, variable, toSubst);
            var right = Substitute(eq.Right, variable, toSubst);
            if (left != eq.Left || right != eq.Right)
            {
                return new Equation(left, right);
            }
            return eq;
        }

        private IType Substitute(IType expr, SingleType variable, IType toSubst)
        {
            if (expr is SingleType)
            {
                if (expr.Equals(variable))
                {
                    return toSubst;
                }
                return expr;
            } else if (expr is Implication)
            {
                var impl = expr as Implication;
                var left = Substitute(impl.Left, variable, toSubst);
                var right = Substitute(impl.Right, variable, toSubst);
                if (left != impl.Left || right != impl.Right)
                {
                    return new Implication(left, right);
                }
                return impl;
            }
            else
            {
                throw new Exception("Unsupported type");
            }
        }

    }

    public class UnificatorUnresolvedExpection : Exception
    {
        public UnificatorUnresolvedExpection(string str) : base(str)
        {

        }
    }
}
