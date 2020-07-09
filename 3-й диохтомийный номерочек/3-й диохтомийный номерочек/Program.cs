using Fractions;
using System;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Dichotomy.Solve(new Fraction(), new Fraction(1), (x) => x * x * x - 7 * x * x + 5, new Fraction(1, 1000));

            Console.WriteLine(result.GetDecimalRepresentation(4));
            Console.ReadKey();
        }
    }
}