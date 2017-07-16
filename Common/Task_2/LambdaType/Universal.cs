using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2.LambdaType
{
    public class Universal : IType
    {
        public SingleType Variable { get; }
        public IType Expression { get; }
        public Universal(SingleType variable, IType Expression)
        {
            this.Variable = variable;
            this.Expression = Expression;
        }
        public override string ToString()
        {
            return "(@" + Variable.ToString() + "." + Expression.ToString() + ")";
        }

        public IType Clone()
        {
            return new Universal((SingleType)Variable.Clone(), Expression.Clone());
        }

        public override bool Equals(object obj)
        {
            var other = obj as Universal;
            if (other == null) return false;
            return Variable.Equals(other.Variable) && Expression.Equals(other.Expression);
        }

        public override int GetHashCode()
        {
            return Variable.GetHashCode() * 31 + Expression.GetHashCode();
        }
    }
}
