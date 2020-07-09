using Fractions;
using System;

namespace Practice
{
    public static class Dichotomy
    {
        public static Fraction Solve(Fraction left, Fraction right, Func<Fraction, Fraction> function, Fraction epsilon)
        {
            if (left > right)
                throw new ArgumentException("Интервал не верен, левая граница должна быть меньше правой!!!!");

            if (function(left) * function(right) > 0)
                throw new ArgumentException("Неверный интервал, не можем найти тут корень!!!!!!!");

            if (function == null)
                throw new ArgumentException("Не задана функция!!!!");

            if (epsilon < 0)
                throw new ArgumentException("Эпсилон не должен быть меньше нуля!!!!!!");

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