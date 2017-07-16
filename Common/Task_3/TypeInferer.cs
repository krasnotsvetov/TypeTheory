using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_2;
using Task_2.LambdaType;

namespace Task_3
{

    public class TypeInferer
    {
        private LambdaExpression expression;
        private Dictionary<SingleType, IType> globalContext = new Dictionary<SingleType, IType>();
        private HashSet<SingleType> FreeVariables = new HashSet<SingleType>();

        public Dictionary<SingleType, IType> Context;

        public TypeInferer(LambdaExpression expression)
        {
            this.expression = expression;
        }

        public IType GetLambdaType()
        {
            var substs = new Dictionary<SingleType, IType>();

            var type = _GetLambdaType(expression, new Dictionary<SingleType, IType>(), out substs);

            foreach (var kvp in globalContext)
            {
                substs[kvp.Key] = kvp.Value;
            }

            var equations = new HashSet<Equation>();

            foreach (var kvp in substs)
            {
                equations.Add(new Equation(kvp.Key, kvp.Value));
            }

            Context = new Unificator(equations).Solve();

            return type; 
        }

        public void PrintContext(StreamWriter sw)
        {
            foreach (var kvp in Context)
            {
                if (FreeVariables.Contains(kvp.Key))
                    sw.WriteLine(kvp.Key + " : " + kvp.Value);
            }
        }

        ///HACK: context is singleType too, but really it's variable

        private IType _GetLambdaType(LambdaExpression expr, Dictionary<SingleType, IType> context, out Dictionary<SingleType, IType> substs)
        {
            if (expr is Variable)
            {
                substs = new Dictionary<SingleType, IType>();
                var variable = new SingleType((expr as Variable).Name);
                if (context.ContainsKey(variable))
                {
                    var cv = new List<SingleType>();
                    var type = RemoveQuantifiers(context[variable], out cv);
                    var d = new Dictionary<SingleType, IType>();
                    cv.ForEach(v => d[v] = GetNewType());
                    type = LambdaTypeEvaluator.Subst(type, d);

                    return type;
                } else
                {
                    var type = GetNewType();
                    globalContext[new SingleType((expr as Variable).Name)] = type;
                    FreeVariables.Add(new SingleType((expr as Variable).Name));
                    return type;
                }
            } 
            if (expr is Application)
            {
                var appl = expr as Application;

                var substs1 = new Dictionary<SingleType, IType>();
                var type1 = _GetLambdaType(appl.Left, context, out substs1);
                var newContext = Merge(context, substs1);

                var substs2 = new Dictionary<SingleType, IType>();
                var type2 = _GetLambdaType(appl.Right, newContext, out substs2);

                var type3 = LambdaTypeEvaluator.Subst(type1, substs2);
                var beta = GetNewType();
                var v = new Unificator(new Equation[] { new Equation(type3, new Implication(type2, beta))}).Solve();

                substs = Merge(Merge(substs2, substs1), v);

                return LambdaTypeEvaluator.Subst(beta, substs);
            }

            if (expr is Abstraction)
            {
                var abstr = expr as Abstraction;
                var newContext = CopyContext(context);

                var variable = new SingleType(abstr.Variable.Name);

                var beta = GetNewType();
                newContext[variable] = beta; 

                var type1 = _GetLambdaType(abstr.Expression, newContext, out substs);
                return new Implication(LambdaTypeEvaluator.Subst(beta, substs), type1);
            }
            if (expr is LetExpression)
            {
                var let = expr as LetExpression;
                var substs1 = new Dictionary<SingleType, IType>();

                var type = _GetLambdaType(let.Left, context, out substs1);

                var newContext = Merge(substs1, context);
                var variableType = ConnectAllFreeVariables(type, newContext);
                var variable = new SingleType(let.Variable.Name);
                if (newContext.ContainsKey(variable))
                {
                    newContext.Remove(variable);
                }

                newContext = Merge(newContext, substs1);

                newContext[variable] = variableType;

                var substs2 = new Dictionary<SingleType, IType>();
                var type2 = _GetLambdaType(let.Right, newContext, out substs2);
                substs = Merge(substs1, substs2);
                return type2;
            }

            throw new NotImplementedException("Type of expr is not supported");
        }

        private IType ConnectAllFreeVariables(IType type, Dictionary<SingleType, IType> context)
        {
            var freeVariables = new HashSet<SingleType>();
            foreach (var expr in context.Values)
            {
                var hs = GetFreeVariables(expr, new HashSet<SingleType>());
                foreach (var v in hs) freeVariables.Add(v);
            }

            var typeFreeVariables = GetFreeVariables(type, new HashSet<SingleType>());
            foreach (var fv in freeVariables)
            {
                typeFreeVariables.Remove(fv);
            }

            var result = type;
            foreach (var v in typeFreeVariables)
            {
                result = new Universal(v, result);
            }
            return result;
        }

        private HashSet<SingleType> GetFreeVariables(IType expr, HashSet<SingleType> connectedVariables)
        {
            if (expr is SingleType)
            {
                if (connectedVariables.Contains((expr as SingleType)))
                {
                    return new HashSet<SingleType>();
                } else
                {
                    return new HashSet<SingleType>() { expr as SingleType};
                }
            } else if (expr is Implication)
            {
                var impl = expr as Implication;
                var free1 = GetFreeVariables(impl.Left, connectedVariables);
                var free2 = GetFreeVariables(impl.Right, connectedVariables);

                foreach (var v in free2) free1.Add(v);
                return free1;
            } else if (expr is Universal)
            {
                var un = expr as Universal;
                Debug.Assert(connectedVariables.Contains(un.Variable));
                connectedVariables.Add(un.Variable);
                var rv = GetFreeVariables(un.Expression, connectedVariables);
                connectedVariables.Remove(un.Variable);
                return rv;
            }
            throw new NotImplementedException();
        }


        private Dictionary<SingleType, IType> Merge(Dictionary<SingleType, IType> origin, Dictionary<SingleType, IType> substs)
        {
            var _origin = ContextSubstitution(CopyContext(origin), substs);
            foreach (var kvp in substs)
            {
                _origin[kvp.Key] = kvp.Value;
            }
            return _origin;
        }


        private Dictionary<SingleType, IType> ContextSubstitution(Dictionary<SingleType, IType> origin, Dictionary<SingleType, IType> substs)
        {
            var _origin = CopyContext(origin);
            foreach (var key in origin.Keys)
            {
                _origin[key] = LambdaTypeEvaluator.Subst(_origin[key], substs);
            }
            return _origin;
        }


        private IType RemoveQuantifiers(IType expr, out List<SingleType> connectedVariables)
        {
            connectedVariables = new List<SingleType>();
            if (expr is Universal)
            {
                var cv = new List<SingleType>();
                var result = RemoveQuantifiers((expr as Universal).Expression,out cv);
                connectedVariables.AddRange(cv);
                return result;
            } else
            {
                return expr;
            }
        }

        private Dictionary<SingleType, IType> CopyContext(Dictionary<SingleType, IType> context)
        {
            var d = new Dictionary<SingleType, IType>();
            foreach (var kvp in context)
            {
                d[(SingleType)kvp.Key] = kvp.Value;
            }
            return d;
        }

        int index = 0;
        private SingleType GetNewType()
        {
            return new SingleType("'t" + (index++));
        }
    }
}
