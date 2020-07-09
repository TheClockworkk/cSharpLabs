using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Complexx
{
    class Program
    {
        static void Main(string[] args)
        {
            Complex complex = new Complex(6, 13);

            Console.WriteLine(Complex.Arg(complex).GetDecimalRepresentation(8));

            Console.ReadKey();
        }
    }
}
