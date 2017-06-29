using Common;
using Common.Grammar;
using Common.LambdaElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_1
{
    class Program
    {

        static LambdaExpression GetL(uint t)
        {
            return LambdaConverter.UInt32ToLambda(t);
        }

        static LambdaExpression Add(uint a, uint b)
        {
            var add = Lambda.Parse(@"\a.\b.\f.\x.a f (b f x)");

            return new Application(new Application(add, GetL(a)), GetL(b));
        }

        static void Main(string[] args)
        {

            var input = File.ReadLines("test.in").First();
            File.WriteAllLines("test.out", new string[] { new Reducer().FullReduce(Lambda.Parse(input)).ToString() });

            Console.WriteLine(LambdaConverter.LambdaToUInt32(new Reducer().FullReduce(new Application(new Application(GetL(2), GetL(2)), GetL(2)))));

            return;

            var lambda = Lambda.Parse(@" (\n.\f.\x.f(n f x))(\f.\x.f x) ");

            /* var number = Lambda.Parse(@"\f.\x.x");
             var increment = Lambda.Parse(@"(\n.\f.\x.f(n f x))");
             while (true)
             {
                 var func = new Application(increment, number);
                 number = new Reducer().FullReduce(func);
                 Console.WriteLine(LambdaConverter.LambdaToUInt32(number));
             }*/

            //Console.WriteLine(LambdaConverter.LambdaToUInt32(new Reducer().FullReduce(Add(53, 51))));

            Console.WriteLine(new Reducer().FullReduce(Lambda.Parse(@"(\y.(\x.x y) y) (\x.x x)")));
            int x = 5;
           Console.WriteLine(new Reducer().FullReduce(Lambda.Parse(@"((\l0.((\l1.((\l2.((\l3.((\l4.((\l5.((\l6.((\l7.((\l8.((\l9.((\l10.(l10 (\l11.(\l12.(l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 (l11 l12))))))))))))))))) (\l10.((l0 (\l11.(\l12.(\l13.(\l14.((\l15.(((l1 (l8 l15)) (\l16.(\l17.(l16 l17)))) ((l6 (((l11 l15) ((l11 (\l16.(\l17.(l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 l17))))))))))))))))))))))))))))))))))))))))))))))))))))) (\l16.(\l17.l17)))) (l11 (\l16.(\l17.(l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 l17))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))) (((l11 (l4 l15)) (((l11 (\l16.(\l17.(l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 l17))))))))))))))))))))))))))))))))))))))))))) (\l16.(\l17.l17))) (\l16.(\l17.l17)))) (((l11 (\l16.(\l17.(l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 (l16 l17))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))) (\l16.(\l17.l17))) (\l16.(\l17.l17))))))) (l4 l12))))))) l10)))) (l0 (\l9.(\l10.(\l11.((\l12.((\l13.(((l1 l12) l13) (((l1 l13) l12) ((l9 (l4 l10)) (l4 l11))))) (l8 l11))) (l8 l10)))))))) (\l8.((l8 (\l9.l3)) l2)))) (\l7.(\l8.((l8 l4) l7))))) (\l6.(\l7.((l6 l5) l7))))) (\l5.(\l6.(\l7.((l5 l6) (l6 l7))))))) (\l4.(\l5.(\l6.(((l4 (\l7.(\l8.(l8 (l7 l5))))) (\l7.l6)) (\l7.l7))))))) (\l3.(\l4.l4)))) (\l2.(\l3.l2)))) (\l1.(\l2.(\l3.((l1 l2) l3)))))) (\l0.((\l1.(l0 (l1 l1))) (\l1.(l0 (l1 l1))))))")));
            // Console.WriteLine(LambdaConverter.LambdaToUInt32(new Reducer().FullReduce(new Application(GetL(2), GetL(5)))));

            ///
            /// \f.(\x.(f (\f.(\x.(x)) f) x))
            /// 
            /// 
            ///



            /* Console.WriteLine(lambda.GetNotation());
             Console.WriteLine(lambda);
             Console.WriteLine(new Reducer().Reduce(lambda));
             var two = new Reducer().FullReduce(lambda);
             Console.WriteLine(two);
             Console.WriteLine(lambda);
             Console.WriteLine(LambdaConverter.UInt32ToLambda(2).GetNotation());

             Console.WriteLine(LambdaConverter.LambdaToUInt32(two));
             Console.WriteLine(new Reducer().FullReduce(new Application(new Application(two, two),two)));*/
             //Console.WriteLine(LambdaConverter.LambdaToUInt32(Lambda.Parse(@"((\3.((\4.((3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 (3 4)))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))))")));

        }
    }
}
