using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace PolynomialTask1
{
    public struct Monomial
    {
        public int power;
        public double coefficient;
    }

    public class Polynomial : IComparable<Polynomial>, ICloneable, IEquatable<Polynomial>
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

        public Polynomial(params Monomial[] param)
            : this(param.Length)
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

        public Polynomial(string param)
        {
            if (!string.IsNullOrEmpty(param))
            {
                Initialize(16);
                GetPolynomialFromString(this, param);
                Normalization(this);
            }
            else Initialize(16);
        }
        #endregion

        #region operators
        #region polinomial polinomial
        public static Polynomial operator +(Polynomial a, Polynomial b)
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

        #region string polinomial
        public static Polynomial operator +(string str, Polynomial b)
        {
            Polynomial a = new Polynomial();
            GetPolynomialFromString(a, str);
            return a+b;
        }

        public static Polynomial operator -(string str, Polynomial b)
        {
            Polynomial a = new Polynomial();
            GetPolynomialFromString(a, str);
            return a - b;
        }

        public static Polynomial operator *(string str, Polynomial b)
        {
            Polynomial a = new Polynomial();
            GetPolynomialFromString(a, str);
            return a * b;
        }

        public static Polynomial operator /(string str, Polynomial b)
        {
            Polynomial a = new Polynomial();
            GetPolynomialFromString(a, str);
            return a / b;
        }

        public static Polynomial operator %(string str, Polynomial b)
        {
            Polynomial a = new Polynomial();
            GetPolynomialFromString(a, str);
            return a % b;
        }
        #endregion

        #region polinomial string
        public static Polynomial operator +(Polynomial a, string str)
        {
            Polynomial b = new Polynomial();
            GetPolynomialFromString(b, str);
            return a + b;
        }

        public static Polynomial operator -(Polynomial a, string str)
        {
            Polynomial b = new Polynomial();
            GetPolynomialFromString(b, str);
            return a - b;
        }

        public static Polynomial operator *(Polynomial a, string str)
        {
            Polynomial b = new Polynomial();
            GetPolynomialFromString(b, str);
            return a * b;
        }

        public static Polynomial operator /(Polynomial a, string str)
        {
            Polynomial b = new Polynomial();
            GetPolynomialFromString(b, str);
            return a / b;
        }

        public static Polynomial operator %(Polynomial a, string str)
        {
            Polynomial b = new Polynomial();
            GetPolynomialFromString(b, str);
            return a % b;
        }
        #endregion
        #endregion

        #region alternative for operators
        #region polinomial polinomial
        public static Polynomial Add(Polynomial a, Polynomial b)
        {
            return a + b;
        }

        public static Polynomial Subtract(Polynomial a, Polynomial b)
        {
            return a - b;
        }

        public static Polynomial Multiply(Polynomial a, Polynomial b)
        {
            return a * b;
        }

        public static Polynomial Divide(Polynomial a, Polynomial b)
        {
            return a / b;
        }

        public static Polynomial Mod(Polynomial a, Polynomial b)
        {
            return a % b;
        }
        #endregion

        #region string polinomial
        public static Polynomial Add(string a, Polynomial b)
        {
            return a + b;
        }

        public static Polynomial Subtract(string a, Polynomial b)
        {
            return a - b;
        }

        public static Polynomial Multiply(string a, Polynomial b)
        {
            return a * b;
        }

        public static Polynomial Divide(string a, Polynomial b)
        {
            return a / b;
        }

        public static Polynomial Mod(string a, Polynomial b)
        {
            return a % b;
        }
        #endregion

        #region polinomial string
        public static Polynomial Add(Polynomial a, string b)
        {
            return a + b;
        }

        public static Polynomial Subtract(Polynomial a, string b)
        {
            return a - b;
        }

        public static Polynomial Multiply(Polynomial a, string b)
        {
            return a * b;
        }

        public static Polynomial Divide(Polynomial a, string b)
        {
            return a / b;
        }

        public static Polynomial Mod(Polynomial a, string b)
        {
            return a % b;
        }
        #endregion

        #endregion

        #region interface realisation
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

        public bool Equals(Polynomial other)
        {
            return this.ToString() == other.ToString();
        }
        #endregion

        #region overrides
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = length - 1; i >= 0; i--)
            {
                if ((power[i] == 0) || (power[i] != 0 && coefficient[i] != 1))
                {
                    sb.AppendFormat("{0:0.00}", coefficient[i]);
                }
                if (power[i] != 0)
                {
                    sb.Append("x");
                    if (power[i] != 1) sb.AppendFormat("^{0}", power[i]);
                }
                if ((i > 0) && (coefficient[i - 1] >= 0)) sb.Append("+");
            }
            string str;
            if ((str = sb.ToString()) == "") str = "0";
            return str;
        }

        public override bool Equals(object other)
        {
            if (!(other is Polynomial)) return false;
            return Equals((Polynomial)other);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        #endregion

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
            int lastA = a.length - 1, lastB = b.length - 1;
            int countOfIteration = (a.power[lastA] - a.power[0]) - (b.power[lastB] - b.power[0]) + 1;

            Polynomial temp;
            Polynomial result = new Polynomial(countOfIteration);

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
                    lastA = remainder.length - 1;
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

        private static void GetPolynomialFromString(Polynomial a, string polinom)
        {
            int power = 0; double coefficient = 0;
            while (GetMonomialFromString(ref polinom, ref power, ref coefficient))
            {
                InsertMonomial(a, power, coefficient);
            }
        }

        private static bool GetMonomialFromString(ref string polinom, ref int power, ref double coefficient)
        {
            power = 0;
            coefficient = 1;

            Regex regular = new Regex(@"-?[0-9]+([.,][0-9]+)?");
            Match teg = regular.Match(polinom);
            string str = teg.ToString();
            int countIndex = polinom.IndexOf(str);
            int xIndex = polinom.IndexOf('x');
            if (((xIndex < 0) || (xIndex > countIndex)) && (teg.Success))
            {
                coefficient = double.Parse(str.Replace(',', '.'), CultureInfo.InvariantCulture);
                polinom = polinom.Remove(0, countIndex + teg.ToString().Length);
            }
            else
                if (xIndex > 0)
                {
                    polinom = polinom.Remove(0, xIndex);
                }
            if (string.IsNullOrEmpty(polinom))
            {
                if (teg.Success) return true;
                else return false;
            }

            if (polinom[0] == 'x')
            {
                if ((polinom.Length>1)&&(polinom[1] == '^'))
                {
                    regular = new Regex(@"-?[0-9]+");
                    teg = regular.Match(polinom);
                    if (int.TryParse(teg.ToString(), out power))
                    {
                        polinom = polinom.Remove(polinom.IndexOf(teg.ToString()) - 2, teg.ToString().Length + 2);
                    }
                    else
                    {
                        power = 1;
                        polinom = polinom.Remove(0, 2);
                    }
                }
                else
                {
                    power = 1;
                    polinom = polinom.Remove(0, 1);
                }
            }


            return true;
        }
    }
}
