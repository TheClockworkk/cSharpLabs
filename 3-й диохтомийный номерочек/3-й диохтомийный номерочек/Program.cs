using Fractions;
using System;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Dichotomy.Solve(new Fraction(), new Fraction(1), (x) => x * x * x - 3 * x + 1, new Fraction(1, 1000));

            Console.WriteLine(result.GetDecimalRepresentation(4));
            Console.ReadKey();
        }
    }
}