using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Grammar;
namespace Common
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Lambda.Parse(@"\f.\x.f (f (f x))").GetNotation());            var first = Lambda.Parse(@"\x.(\x.x) x (\x.\x.\x.x)").GetNotation();            var second = Lambda.Parse(@"\x.(\x.x) x (\y.\z.\y.y)").GetNotation();            Console.WriteLine(first.Equals(second));            Console.WriteLine(first);            Console.WriteLine(second);            Console.WriteLine(Lambda.Parse(@"\x.y").GetNotation());            Console.WriteLine(Lambda.Parse(@"\x.\y.x y").GetNotation());            Console.WriteLine(Lambda.Parse("((a)a)").Equals(Lambda.Parse("(a(a))")));







            /// ((((\x.x)w)y)v)(\t.t)b            ///              ///
            Console.WriteLine(Lambda.Parse(@"a \a.a a"));
            Console.WriteLine(Lambda.Parse(@"\y.y a"));            Console.WriteLine(Lambda.Parse(@"(\y.y) a"));

         }
    }
}
