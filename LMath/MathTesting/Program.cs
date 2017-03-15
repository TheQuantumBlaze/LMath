using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMath;

namespace MathTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("EnterNumber");
                string s = Console.ReadLine();
                BigInt m = new BigInt(s);
                string s1 = Console.ReadLine();
                BigInt m1 = new BigInt(s1);
                Console.WriteLine((m%m1).ToString());
                Console.ReadKey();
            }
        }
    }
}
