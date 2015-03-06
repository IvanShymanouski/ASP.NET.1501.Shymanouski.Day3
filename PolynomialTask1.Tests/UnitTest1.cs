using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PolynomialTask1.Tests
{
    [TestClass]
    public class PolynomialTests
    {
        [TestMethod]
        public void ctorAndOperatorsTests()
        {
            Monomial[] a1 = 
                {
                   new Monomial {power = 8, coefficient = 13.1},
                   new Monomial {power = 8, coefficient = 123.2},
                   new Monomial {power = 9, coefficient = 1.4},
                   new Monomial {power = 8, coefficient = 1.4},
                   new Monomial {power = 2, coefficient = 2.8},
                   new Monomial {power = 8, coefficient = 76},
                   new Monomial {power = 2, coefficient = 1.8},
                   new Monomial {power = 9, coefficient = 1.4}
                };
            Polynomial o1 = new Polynomial(a1);

            Assert.AreEqual(o1.ToString(), "4,60x^2+213,70x^8+2,80x^9","ctor fail");

            Monomial[] aa1 =
            {
                new Monomial {power = 2, coefficient = 4},
                new Monomial {power = 1, coefficient = 3}
            };
            o1 = new Polynomial(aa1);

            Monomial[] a2 =
            {
                new Monomial {power = 2, coefficient = 5},
                new Monomial {power = 3, coefficient = 6}
            };
            Polynomial o2 = new Polynomial(a2);

            Polynomial summ = o1 * o2;

            Monomial[] a3 =
            {
                new Monomial {power = 3, coefficient = 15},
                new Monomial {power = 4, coefficient = 38},
                new Monomial {power = 5, coefficient = 24}
            };
            Polynomial o3 = new Polynomial(a3);

            Assert.AreEqual(summ.ToString(), o3.ToString(),"mul fail");

            Polynomial o4 = o3 / o2;
            Polynomial o5 = o3 / o1;

            Assert.AreEqual(o4.ToString(), o1.ToString(), "divide fail 1");
            Assert.AreEqual(o5.ToString(), o2.ToString(), "divide fail 2");

            o4 = o3 - o2;
            o5 = o4 + o2;

            Assert.AreEqual(o3.ToString(), o5.ToString(), "Subtract or Sum fail");

        }

        [TestMethod]
        public void NullStabilityTests()
        {
            Polynomial p0 = null;
            Polynomial p1 = new Polynomial(p0);

            p1 += p0;
            Assert.AreNotEqual(p1,null,"null error 1");

            p0 += p1;
            Assert.AreEqual(p1, p0, "null error 2");
        }
    }
}
