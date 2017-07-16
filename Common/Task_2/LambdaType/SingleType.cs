using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2.LambdaType
{
    public class SingleType : IType
    {
        public string Name { get; private set; }
        public SingleType(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            var other = obj as SingleType;
            if (other == null) return false;
            return Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public IType Clone()
        {
            return new SingleType(Name);
        }
    }
}
