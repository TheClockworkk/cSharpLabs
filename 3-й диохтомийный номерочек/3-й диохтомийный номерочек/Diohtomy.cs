using Fractions;
using System;

namespace Practice
{
    public static class Dichotomy
    {
        public static Fraction Solve(Fraction left, Fraction right, Func<Fraction, Fraction> function, Fraction epsilon)
        {
            if (left > right)
                throw new ArgumentException("Wrong interval. The left border should be less than the right!");

            if (function(left) * function(right) > 0)
                throw new ArgumentException("Wrong interval. Can not find root here!");

            if (function == null)
                throw new ArgumentException("No function was passed!");

            if (epsilon < 0)
                throw new ArgumentException("Epsilon must be positive!");

            Fraction a = left;
            Fraction b = right;
            Fraction c;

            while (Fraction.Abs(a - b) > epsilon)
            {
                c = (a + b) / 2;

                if (function(b) * function(c) < 0)
                    a = c;
                else
                    b = c;
            }

            return (a + b) / 2;
        }
    }
}