using Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math
{
    class Program
    {
        static void Main(string[] args)
        {
            Fraction fraction1 = new Fraction(3, 7);
            Fraction fraction2 = new Fraction(1, 3);

            var example1 = new Fraction();
            var example2 = new Fraction(5);
            var example3 = new Fraction(5, -3);
            var example4 = new Fraction(fraction1);

            Console.WriteLine(example1);
            Console.WriteLine(example2);
            Console.WriteLine(example3);
            Console.WriteLine(example4);

            Console.WriteLine(fraction1);
            Console.WriteLine(fraction1.GetDecimalRepresentation());

            Console.WriteLine(fraction1 + fraction2);
            Console.WriteLine(fraction2 - fraction1);

            Console.WriteLine(fraction1 < fraction2);
            Console.WriteLine(fraction2 > fraction1);

            Console.WriteLine(fraction2 / 2);

            Console.WriteLine(Fraction.Pow(fraction2, 3));
            Console.WriteLine(Fraction.Sqrt(new Fraction(1, 4)).GetDecimalRepresentation());

            Console.ReadKey();
        }
    }
}