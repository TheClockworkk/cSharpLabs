using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Math
{
    public sealed class Fraction : IEquatable<Fraction>, IComparable, IComparable<Fraction>, ICloneable //по тз Илюши, наследовать неззя
    {
        #region Properties
        public BigInteger Numerator { get; private set; } // пилим геттеры сеттеры для числителя и знаменателя
        public BigInteger Denominator { get; private set; }
        #endregion

        #region Constructors
        public Fraction() // и конструкторы, сначала для пустого fraction, на случай создпния 0/0 по умолчанию
        {
            Numerator = BigInteger.Zero;
            Denominator = BigInteger.One;
        }

        public Fraction(BigInteger numerator) : this() // а затем и не для пустых. This для передачи ссылки на первое пустое место в скобках, а затем выполняется предыдущий
        {
            Numerator = numerator;
        }

        public Fraction(BigInteger numerator, BigInteger denominator) : this(numerator)
        {
            if (denominator == BigInteger.Zero)
                throw new DivideByZeroException("The denominator must be nonzero!");

            Denominator = denominator;

            Reduce(); //это сокращение, описывается ниже
        }

        public Fraction(Fraction fraction) //сюда передается готовая дробь, типа конструктор копирования
        {
            Numerator = fraction.Numerator;
            Denominator = fraction.Denominator;
        }
        #endregion

        #region Private
        private Fraction Reduce() //сокращение. Нахождение общих делителей и сокращение
        {
            BigInteger gcd = BigInteger.GreatestCommonDivisor(Numerator, Denominator);

            Numerator /= gcd;
            Denominator /= gcd;

            if (Denominator.Sign == -1)
            {
                Numerator = BigInteger.Negate(Numerator);
                Denominator = BigInteger.Negate(Denominator);
            }

            return this;
        }

        private Fraction Reciprocate() //взаимообратная дробь
        {
            if (Numerator == BigInteger.Zero)
                return this;

            return new Fraction(Denominator, Numerator);
        }

        private void ToDenominator(BigInteger targetDenominator) //надо будет при сложении и вычитании, типа приведенпие к общему заменателю
        {
            if ((targetDenominator < Denominator) || (targetDenominator % Denominator != 0))
                return;

            if (Denominator != targetDenominator)
            {
                BigInteger factor = targetDenominator / Denominator;
                Numerator *= factor;
                Denominator = targetDenominator;
            }
        }

        private static BigInteger GetLeastCommonMultiple(BigInteger first, BigInteger second) //ищется НОК
        {
            return (first * second) / BigInteger.GreatestCommonDivisor(first, second);
        }
        #endregion

        #region Public
        public static Fraction Abs(Fraction fraction) //модуль
        {
            return new Fraction(BigInteger.Abs(fraction.Numerator), BigInteger.Abs(fraction.Denominator));
        }

        public static Fraction Pow(Fraction fraction, int exponent) //в степень
        {
            Fraction result = fraction.Clone() as Fraction;

            if (exponent < 0)
            {
                result.Reciprocate();
                exponent = -exponent;
            }

            result.Numerator = BigInteger.Pow(result.Numerator, exponent);
            result.Denominator = BigInteger.Pow(result.Denominator, exponent);

            return result.Reduce();
        }

        public static Fraction Sqrt(Fraction fraction)// корень
        {
            if (fraction < 0)
                throw new ArgumentException("Unable to extract square root from negative fraction!");

            // Extreme values
            if (fraction == 0 || fraction == 1)
                return fraction;

            Fraction eps = new Fraction(1, 100000);

            Fraction A0 = fraction / 2;
            Fraction A1 = (A0 + fraction / A0) / 2;

            while (Abs(A1 - A0) > eps)
            {
                A0 = A1;
                A1 = (A0 + fraction / A0) / 2;
            }

            return A1.Reduce();
        }

        public static Fraction SqrtN(Fraction fraction, int n) //корень энной степени
        {
            if ((fraction < 0) && (n % 2 == 0))
                throw new ArgumentException("It is not possible to isolate the even root of a negative fraction!");

            if (n < 2)
                throw new ArgumentException("The root of the nth degree must be greater than or equal to 2!");

            if (fraction == 0 || fraction == 1)
                return fraction;

            Fraction eps = new Fraction(1, 1000);

            Fraction A0 = fraction / n;
            Fraction A1 = ((n - 1) * A0 + fraction / Pow(A0, n - 1)) / n;

            while (Abs(A1 - A0) > eps)
            {
                A0 = A1;
                A1 = ((n - 1) * A0 + fraction / Pow(A0, n - 1)) / n;
            }

            return A1.Reduce();
        }


        public string GetDecimalRepresentation(int decimals = 28) //привод обычной дроби в десятичную
        {
            if (decimals < 0)
                throw new ArgumentException("Decimals can not be less than Zero!");

            BigInteger numerator = BigInteger.Abs(Numerator);
            BigInteger denumerator = BigInteger.Abs(Denominator);

            BigInteger quotient = BigInteger.DivRem(numerator, denumerator, out BigInteger remainder);
            StringBuilder sb = new StringBuilder();

            if (this < 0)
                sb.Append('-');

            sb.Append(quotient.ToString());

            if (decimals != 0)
                sb.Append('.');

            for (int i = 0; i < decimals; i++)
            {
                remainder *= 10;
                sb.Append(BigInteger.DivRem(remainder, denumerator, out remainder));
            }

            return sb.ToString();
        }
        #endregion

        #region Operators
        public static Fraction UnaryPlus(Fraction fraction)
        {
            return fraction;
        }
        public static Fraction operator +(Fraction fraction) //перегрузка + унарного
        {
            return UnaryPlus(fraction);
        }

        public static Fraction UnaryNegation(Fraction fraction)
        {
            return new Fraction(BigInteger.Negate(fraction.Numerator), fraction.Denominator);
        }
        public static Fraction operator -(Fraction fraction)
        {
            return UnaryNegation(fraction);
        }

        public static Fraction Addition(Fraction left, Fraction right)
        {
            BigInteger lcm = GetLeastCommonMultiple(left.Denominator, right.Denominator);

            left.ToDenominator(lcm);
            right.ToDenominator(lcm);

            return new Fraction(left.Numerator + right.Numerator, lcm).Reduce();
        }
        public static Fraction operator +(Fraction left, Fraction right) // перегрузки плюсов
        {
            return Addition(left, right);
        }
        public static Fraction operator +(Fraction left, BigInteger right)
        {
            return Addition(left, new Fraction(right));
        }
        public static Fraction operator +(BigInteger left, Fraction right)
        {
            return Addition(new Fraction(left), right);
        }

        public static Fraction Substraction(Fraction left, Fraction right)// вычитание
        {
            return Addition(left, -right);
        }
        public static Fraction operator -(Fraction left, Fraction right)
        {
            return Substraction(left, right);
        }
        public static Fraction operator -(Fraction left, BigInteger right)
        {
            return Substraction(left, new Fraction(right));
        }
        public static Fraction operator -(BigInteger left, Fraction right)
        {
            return Substraction(new Fraction(left), right);
        }

        public static Fraction Multiplication(Fraction left, Fraction right)// умножение
        {
            BigInteger newNumerator = left.Numerator * right.Numerator;
            BigInteger newDenominator = left.Denominator * right.Denominator;

            return new Fraction(newNumerator, newDenominator).Reduce();
        }
        public static Fraction operator *(Fraction left, Fraction right)
        {
            return Multiplication(left, right);
        }
        public static Fraction operator *(Fraction left, BigInteger right)
        {
            return Multiplication(left, new Fraction(right));
        }
        public static Fraction operator *(BigInteger left, Fraction right)
        {
            return Multiplication(new Fraction(left), right);
        }

        public static Fraction Division(Fraction left, Fraction right) //деление
        {
            if (right == 0)
                throw new DivideByZeroException("Can not divide by Zero!");

            return Multiplication(left, right.Reciprocate());
        }
        public static Fraction operator /(Fraction left, Fraction right)
        {
            return Division(left, right);
        }
        public static Fraction operator /(Fraction left, BigInteger right)
        {
            return Division(left, new Fraction(right));
        }
        public static Fraction operator /(BigInteger left, Fraction right)
        {
            return Division(new Fraction(left), right);
        }

        public static Fraction Increment(Fraction fraction) //инкремент
        {
            return Addition(fraction, new Fraction(BigInteger.One));
        }
        public static Fraction operator ++(Fraction fraction)
        {
            return Increment(fraction);
        }

        public static Fraction Decrement(Fraction fraction) //декремент
        {
            return Substraction(fraction, new Fraction(BigInteger.One));
        }
        public static Fraction operator --(Fraction fraction)
        {
            return Decrement(fraction);
        }

        public static bool operator ==(Fraction left, Fraction right) //равенство
        {
            if (ReferenceEquals(left, null) && right is null)
                return true;
            else if (left is object && right is null)
                return left.Equals(right);
            else
                return right.Equals(left);
        }
        public static bool operator ==(Fraction left, BigInteger right)
        {
            return (left == new Fraction(right));
        }
        public static bool operator ==(BigInteger left, Fraction right)
        {
            return (new Fraction(left) == right);
        }

        public static bool operator !=(Fraction left, Fraction right) //не равно
        {
            return !(left == right);
        }
        public static bool operator !=(Fraction left, BigInteger right)
        {
            return (left != new Fraction(right));
        }
        public static bool operator !=(BigInteger left, Fraction right)
        {
            return (new Fraction(left) != right);
        }

        public static bool operator >(Fraction left, Fraction right) //больше
        {
            if (left.CompareTo(right) == 1)
                return true;
            return false;
        }
        public static bool operator >(Fraction left, BigInteger right)
        {
            return (left > new Fraction(right));
        }
        public static bool operator >(BigInteger left, Fraction right)
        {
            return (new Fraction(left) > right);
        }

        public static bool operator <(Fraction left, Fraction right) //меньше
        {
            if (left.CompareTo(right) == -1)
                return true;
            return false;
        }
        public static bool operator <(Fraction left, BigInteger right)
        {
            return (left < new Fraction(right));
        }
        public static bool operator <(BigInteger left, Fraction right)
        {
            return (new Fraction(left) < right);
        }

        public static bool operator >=(Fraction left, Fraction right) // больше равно
        {
            if (left.CompareTo(right) >= 0)
                return true;
            return false;
        }
        public static bool operator >=(Fraction left, BigInteger right)
        {
            return (left >= new Fraction(right));
        }
        public static bool operator >=(BigInteger left, Fraction right)
        {
            return (new Fraction(left) >= right);
        }

        public static bool operator <=(Fraction left, Fraction right) //меньше равно
        {
            if (left.CompareTo(right) <= 0)
                return true;
            return false;
        }
        public static bool operator <=(Fraction left, BigInteger right)
        {
            return (left <= new Fraction(right));
        }
        public static bool operator <=(BigInteger left, Fraction right)
        {
            return (new Fraction(left) <= right);
        }
        #endregion

        #region Overriden
        public override string ToString() //создание строки по тз aka перегрузка метода to string. Короче числитель / знаменатеоль
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Numerator).Append('/').Append(Denominator);
            return sb.ToString();
        }

        public override int GetHashCode() //ХЭЭЭЭШ
        {
            return Numerator.GetHashCode() ^ Denominator.GetHashCode();
        }

        public override bool Equals(object obj) //перегрузка сравнения для дробей юзания
        {
            if (obj is null)
                return false;

            if (obj is Fraction)
                return Equals(obj as Fraction);

            return false;
        }
        #endregion

        #region IEquatable<T>
        public bool Equals(Fraction other) //юзание для дробей
        {
            if (other is null)
                return false;

            if (Denominator == other.Denominator)
                if (Numerator == other.Numerator)
                    return true;

            return false;
        }
        #endregion

        #region IComparable
        public int CompareTo(object obj) // для > <
        {
            if (obj is null)
                return 1;

            if (obj is Fraction)
                return CompareTo(obj as Fraction);

            throw new ArgumentException("Object is not a Fraction!");
        }
        #endregion

        #region IComparable<T>
        public int CompareTo(Fraction other) //сравненьице для дроби нашей
        {
            if (other is null)
                return 1;

            BigInteger lcm = GetLeastCommonMultiple(Denominator, other.Denominator);

            ToDenominator(lcm);
            other.ToDenominator(lcm);

            return Numerator.CompareTo(other.Numerator);
        }
        #endregion

        #region ICloneable
        public object Clone() //клоны
        {
            return new Fraction(this);
        }
        #endregion
    }
}