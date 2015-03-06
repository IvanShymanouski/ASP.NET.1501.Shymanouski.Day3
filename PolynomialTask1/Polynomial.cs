using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolynomialTask1
{
    public struct Monomial
    {
        public int power;
        public double coefficient;
    }


    public class Polynomial: IComparable<Polynomial>,ICloneable
    {
        #region Monomial
        int[] power;
        double[] coefficient;
        int length;
        #endregion

        #region ctor
        public Polynomial()
            : this(16)
        {
        }

        private void Initialize(int len)
        {
            if (len < 1) len = 1;
            power = new int[len];
            coefficient = new double[len];
            length = 0;
        }

        public Polynomial(params Monomial[] param):this(param.Length)
        {
            for (int i = 0; i < param.Length; i++)
            {
                InsertMonomial(this, param[i].power, param[i].coefficient);
            }
            Normalization(this);
        }

        public Polynomial(int len)
        {
            Initialize(len);
        }

        public Polynomial(Polynomial polynomial)
        {
            if (polynomial != null)
            {
                Initialize(polynomial.length);
                Array.Copy(polynomial.power, power, polynomial.length);
                Array.Copy(polynomial.coefficient, coefficient, polynomial.length);
                length = polynomial.length;
            }
            else Initialize(16);
        }
        #endregion

        #region operators
        public static Polynomial operator +(Polynomial a,Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            return Normalization(PolynomialMerge(a, b, +1));
        }

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            return Normalization(PolynomialMerge(a, b, -1));
        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;

            Polynomial result = GetProduct(a, b.power[0], b.coefficient[0]);
            Polynomial temp; 

            for (int i = 1; i < b.length; i++)
            {
                temp = GetProduct(a, b.power[i], b.coefficient[i]);
                result = PolynomialMerge(result, temp, +1);
            }

            return Normalization(result);
        }

        public static Polynomial operator /(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;

            Polynomial rem;
            return Normalization(GetDevideResult(a, b, out rem));
        }

        public static Polynomial operator %(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;

            Polynomial rem;
            GetDevideResult(a, b, out rem);
            return Normalization(rem);
        }
        #endregion

        #region alternative for operators
        public static Polynomial Add(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            return Normalization(PolynomialMerge(a, b, +1));
        }

        public static Polynomial Subtract(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            return Normalization(PolynomialMerge(a, b, -1));
        }

        public static Polynomial Multiply(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            Polynomial result = GetProduct(a, b.power[0], b.coefficient[0]);
            Polynomial temp;

            for (int i = 1; i < b.length; i++)
            {
                temp = GetProduct(a, b.power[i], b.coefficient[i]);
                result = PolynomialMerge(result, temp, +1);
            }

            return Normalization(result);
        }

        public static Polynomial Divide(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            Polynomial rem;
            return Normalization(GetDevideResult(a, b, out rem));
        }

        public static Polynomial Mod(Polynomial a, Polynomial b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            Polynomial rem;
            GetDevideResult(a, b, out rem);
            return Normalization(rem);
        }
        #endregion

        public object Clone()
        {
            return new Polynomial(this);
        }

        public int CompareTo(Polynomial other)
        {
            if (other == null) return 1;
            int lenOther = other.length - 1,
                len = length - 1;
            while (len >= 0 && lenOther >= 0)
            {
                if (other.power[lenOther] < power[len]) return 1;
                if (other.power[lenOther] > power[len]) return -1;
                if (other.coefficient[lenOther] < coefficient[len]) return 1;
                if (other.coefficient[lenOther] > coefficient[len]) return -1;
                len--;
                lenOther--;
            }
            if (len >= 0) return 1;
            else if (lenOther >= 0) return 1;
            else return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.AppendFormat("{0:0.00}",coefficient[i]);
                sb.AppendFormat("x^{0}+", power[i]);
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        //removes zero coefficient monomials
        private static Polynomial Normalization(Polynomial a)
        {
            int skip = 0, i = 0;

            while ((i < a.length) && (!DoubleEqual(a.coefficient[i], 0))) i++;

            for (; i < a.length; i++)
            {
                if (DoubleEqual(a.coefficient[i], 0))
                {
                    skip++;
                }
                else
                {
                    a.coefficient[i - skip] = a.coefficient[i];
                    a.power[i - skip] = a.power[i];
                }
            }
            a.length -= skip;
            return a;
        }

        private static bool DoubleEqual(double a, double b)
        {
            const double eps = 1E-2;
            return Math.Abs(a - b) < eps;
        }

        private static Polynomial GetProduct(Polynomial a, int power, double coefficient)
        {
            Polynomial result = new Polynomial(a);

            for (int i = 0; i < a.length; i++)
            {
                result.power[i] += power;
                result.coefficient[i] *= coefficient;
            }

            return result;
        }

        private static Polynomial GetDevideResult(Polynomial a, Polynomial b, out Polynomial remainder)
        {
            int lastA=a.length-1, lastB=b.length-1;
            int countOfIteration = (a.power[lastA] - a.power[0]) - (b.power[lastB] - b.power[0]) + 1;

            Polynomial temp;
            Polynomial result=new Polynomial(countOfIteration);

            remainder = a;

            if (countOfIteration > 0)
            {
                for (int i = 0; i < countOfIteration; i++)
                {
                    int powerMul = remainder.power[lastA] - b.power[lastB];
                    double coefficientMul = remainder.coefficient[lastA] / b.coefficient[lastB];

                    InsertMonomial(result, powerMul, coefficientMul);
                    temp = GetProduct(b, powerMul, coefficientMul);
                    remainder = Normalization(PolynomialMerge(remainder, temp, -1));
                    lastA = remainder.length-1;
                }
            }
            else
            {
                result = new Polynomial();
                remainder = new Polynomial(a);
            }

            return result;
        }

        private static void InsertMonomial(Polynomial a, int power, double coefficient)
        {
            int j = 0;

            while ((a.power[j] < power) && (j < a.length)) j++;

            if (j < a.length)
            {
                if (a.power[j] > power)
                    AddMonomial(a, power, coefficient, j);
                else
                {
                    a.coefficient[j] += coefficient;
                }
            }
            else
            {
                a.power[a.length] = power;
                a.coefficient[a.length] = coefficient;
                a.length++;
            }
        }

        private static Polynomial PolynomialMerge(Polynomial a, Polynomial b, int sign)
        {
            Polynomial result = new Polynomial(a);

            for (int i = 0; i < b.length; i++)
            {
                int j = 0;
                while ((j < result.length) && (b.power[i] > result.power[j])) { j++; continue; }

                if (j == result.length)
                    AddMonomial(result, b.power[i], (sign * b.coefficient[i]), result.length);
                else
                    if (b.power[i] == result.power[j])
                    {
                        result.coefficient[j] += (sign * b.coefficient[i]);
                    }
                    else
                        AddMonomial(result, b.power[i], (sign * b.coefficient[i]), j);
            }

            return result;
        }

        private static void AddMonomial(Polynomial a, int power, double coefficient, int position)
        {
            int[] tempPower;
            double[] tempCoefficient;
            if (a.length == a.power.Length)
            {
                tempPower = new int[a.length * 2];
                tempCoefficient = new double[a.length * 2];
            }
            else
            {
                tempPower = new int[a.power.Length];
                tempCoefficient = new double[a.coefficient.Length];
            }

            tempPower[position] = power;
            tempCoefficient[position] = coefficient;

            int shift = 0;
            for (int i = 0; i <= a.length; i++)
            {
                if (i == position)
                {
                    shift--;
                }
                else
                {
                    tempPower[i] = a.power[i + shift];
                    tempCoefficient[i] = a.coefficient[i + shift];
                }
            }
            a.length++;
            a.power = tempPower;
            a.coefficient = tempCoefficient;
        }
    }
}
