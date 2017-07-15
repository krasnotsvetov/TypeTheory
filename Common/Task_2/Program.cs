using Common.Grammar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    class Program
    {
        static void Main(string[] args)
        {

            var input = File.ReadAllLines("test.in").First();
            using (var sw = new StreamWriter(new FileStream("test.out", FileMode.Create)))
            {
                try
                {
                    var ev = new LambdaTypeEvaluator(Lambda.Parse(input));

                    sw.WriteLine(ev.GetLambdaType());
                    ev.PrintContext(sw);
                }
                catch (UnificatorUnresolvedExpection e)
                {
                    sw.WriteLine("The lambda expression has no type");
                }
            }

        }
    }
}
