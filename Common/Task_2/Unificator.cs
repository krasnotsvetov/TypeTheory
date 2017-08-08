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
      var newEquations = equations.ToList();
      var nextEquations = equations.ToList();
      while (isChanged)
      {
        isChanged = false;

        isChanged |= ApplyFirst(newEquations, out nextEquations);
        newEquations = nextEquations;

        isChanged |= ApplySecond(newEquations, out nextEquations);
        newEquations = nextEquations;

        isChanged |= ApplyThird(newEquations, out nextEquations);
        newEquations = nextEquations;

        isChanged |= ApplyFourth(newEquations, out nextEquations);
        newEquations = nextEquations;
      }
      return newEquations.ToDictionary(t => (SingleType)t.Left, k => k.Right);
    }

    private bool ApplyFirst(List<Equation> equations, out List<Equation> result)
    {
      result = new List<Equation>();
      bool isChanged = false;
      foreach (var eq in equations)
      {
        if (!(eq.Left is SingleType) && eq.Right is SingleType)
        {
          result.Add(new Equation(eq.Right, eq.Left));
          isChanged = true;
        }
        else
        {
          result.Add(eq);
        }
      }
      return isChanged;
    }

    private bool ApplySecond(List<Equation> equations, out List<Equation> result)
    {
      result = new List<Equation>();

      foreach (var eq in equations)
      {
        if (eq.Left is SingleType)
        {
          ThrowIfContainVariable(eq.Left as SingleType, eq.Right);
        }

        if (eq.Left.Equals(eq.Right)) continue;
        result.Add(eq);
      }
      return result.Count != equations.Count;
    }

    private bool ApplyThird(List<Equation> equations, out List<Equation> result)
    {
      result = new List<Equation>();

      foreach (var eq in equations)
      {
        if (eq.Left is Implication && eq.Right is Implication)
        {
          var implLeft = eq.Left as Implication;
          var implRight = eq.Right as Implication;
          
          result.Add(new Equation(implLeft.Left, implRight.Left));
          result.Add(new Equation(implLeft.Right, implRight.Right));
        }
        else
        {
          result.Add(eq);
        }
      }
      return result.Count != equations.Count;
    }

    private bool ApplyFourth(List<Equation> equations, out List<Equation> result)
    {
      result = new List<Equation>(equations);

      bool isChanged = false;

      for (int i = 0; i < result.Count; i++)
      {
        if (result[i].Left is SingleType)
        {
          for (int j = 0; j < result.Count; j++)
          {
            if (i == j) continue;
            var newEquation = Substitute(result[j], (SingleType)result[i].Left, result[i].Right);
            if (newEquation != result[j])
            {
              result[j] = newEquation;
              isChanged = true;
            }
          }
        }
      }
      return isChanged;
    }

    private void ThrowIfContainVariable(SingleType variable, IType expr)
    {
      if (expr is Implication)
      {
        var impl = expr as Implication;
        if (impl.Left.Equals(variable))
          throw new UnificatorUnresolvedExpection("The system can't be resolved");
        if (impl.Right.Equals(variable))
          throw new UnificatorUnresolvedExpection("The system can't be resolved");
        if (!(impl.Left is SingleType)) ThrowIfContainVariable(variable, (Implication)impl.Left);
        if (!(impl.Right is SingleType)) ThrowIfContainVariable(variable, (Implication)impl.Right);
      }
      else
      if (expr is SingleType)
      {
        //It can happend, we will remove x = x on next iteration
        //if (expr.Equals(variable)) throw new Exception();
      }
      else
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
      }
      else if (expr is Implication)
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
