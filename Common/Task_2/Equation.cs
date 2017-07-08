﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_2.LambdaType;

namespace Task_2
{
    public class Equation
    {
        public IType Left { get; private set; }
        public IType Right { get; private set; }

        public Equation(IType left, IType right)
        {
            this.Left = left;
            this.Right = right;
        }

        public override string ToString()
        {
            return Left.ToString() + " = " + Right.ToString();
        }

        public override int GetHashCode()
        {
            return Left.GetHashCode() * 19 + Right.GetHashCode() * 3;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Implication;
            if (other == null) return false;
            return (other.Left.Equals(Left) && other.Right.Equals(Right));
        }
    }
}
