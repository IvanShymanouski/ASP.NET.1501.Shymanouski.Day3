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

            Assert.AreEqual(o1.ToString(), "2,80x^9+213,70x^8+4,60x^2", "ctor fail");

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

            Assert.AreEqual(summ.ToString(), o3.ToString(), "mul fail");

            Polynomial o4 = o3 / o2;
            Polynomial o5 = o3 / o1;

            Assert.AreEqual(o4.ToString(), o1.ToString(), "divide fail 1");
            Assert.AreEqual(o5.ToString(), o2.ToString(), "divide fail 2");

            o4 = o3 - o2;
            o5 = o4 + o2;

            Assert.AreEqual(o3.ToString(), o5.ToString(), "Subtract or Sum fail");

            o4 = o5 - o3;

            Assert.AreEqual(o4.ToString(), "0", "Subtract or Sum fail");

        }

        [TestMethod]
        public void StringCtorTests()
        {
            Polynomial p = new Polynomial("x+x^+x+x+x");
            Assert.AreEqual(p.ToString(),"5,00x","1");

            p = new Polynomial("x+23+x+x+55+x");
            Assert.AreEqual(p.ToString(), "4,00x+78,00", "2");

            p = new Polynomial("-8x^3-0x+0x^2");
            Assert.AreEqual(p.ToString(), "-8,00x^3", "3");

            p = new Polynomial("54x+9x^-9+27");
            Assert.AreEqual(p.ToString(), "54,00x+27,00+9,00x^-9", "4");

            p = new Polynomial("-82,89x^3+1-1+x+12x^0+1x^0+x^0+27");
            Assert.AreEqual(p.ToString(), "-82,89x^3+x+41,00", "4");
        }

        [TestMethod]
        public void NullStabilityTests()
        {
            Polynomial p0 = null;
            Polynomial p1 = new Polynomial(p0);

            p1 += p0;
            Assert.AreNotEqual(p1, null, "null error 1");

            p0 += p1;
            Assert.AreEqual(p1, p0, "null error 2");
        }

        [TestMethod]
        public void DividerAndModTest1()
        {
            Monomial[] a1 =
            {
                new Monomial {power = 6, coefficient = 2},
                new Monomial {power = 5, coefficient = -1},
                new Monomial {power = 3, coefficient = 12},
                new Monomial {power = 2, coefficient = -72},
                new Monomial {power = 0, coefficient = 3}
            };
            Polynomial o1 = new Polynomial(a1);

            Monomial[] a2 =
            {
                new Monomial {power = 3, coefficient = 1},
                new Monomial {power = 2, coefficient = 2},
                new Monomial {power = 0, coefficient = -1}
            };
            Polynomial o2 = new Polynomial(a2);

            Polynomial o3 = o1 / o2;
            Assert.AreEqual(o3.ToString(), "2,00x^3-5,00x^2+10,00x-6,00", "Divide problem");

            Polynomial o4 = o1 % o2;
            Assert.AreEqual(o4.ToString(), "-65,00x^2+10,00x-3,00", "Mod problem");
        }

        [TestMethod]
        public void DividerAndModTest2()
        {
            Polynomial o1 = new Polynomial("8x^3+36x^2+54x+27");

            Polynomial o2 = new Polynomial("2x+3");

            Polynomial o3 = o1 / o2;
            Assert.AreEqual(o3.ToString(), "4,00x^2+12,00x+9,00", "Divide problem");

            Polynomial o4 = o1 % o2;
            Assert.AreEqual(o4.ToString(), "0", "Mod problem");
        }

        [TestMethod]
        public void StringOperatorsTests()
        {
            Polynomial p = new Polynomial("8,0x^3+36.0x^2+54x+27");

            Polynomial p1 = p / "2x+3";
            Assert.AreEqual(p1.ToString(), "4,00x^2+12,00x+9,00", "Divide problem");

            p = new Polynomial("2x+3");
            p1 = "8x^3+36x^2+54x+27" / p;
            Assert.AreEqual(p1.ToString(), "4,00x^2+12,00x+9,00", "Divide problem2");

            p1 = p % p1;
            Assert.AreEqual(p1.ToString(), "2,00x+3,00", "Mod problem");
        }

        [TestMethod]
        public void ToStringTest()
        {
            Polynomial o1 = new Polynomial(
                                           new Monomial { power = 1, coefficient = 54 },
                                           new Monomial { power = 0, coefficient = -1 }
                                          );
            Assert.AreEqual(o1.ToString(), "54,00x-1,00", "coefficient = -1 power = 0");

            o1 = new Polynomial(
                                new Monomial { power = 0, coefficient = 1 },
                                new Monomial { power = 1, coefficient = 1 }
                               );

            Assert.AreEqual(o1.ToString(), "x+1,00", "coefficient = 1(1) power = 0(1)");

            o1 = new Polynomial(
                                new Monomial { power = 0, coefficient = 20.999 },
                                new Monomial { power = -1, coefficient = 1 }
                               );

            Assert.AreEqual(o1.ToString(), "21,00+x^-1", "coefficient = 1(20,999) power = -1(0)");
        }

        [TestMethod]
        public void EqualMethodsTest()
        {
            Polynomial o1 = new Polynomial(
                                           new Monomial { power = 1, coefficient = 54 },
                                           new Monomial { power = 0, coefficient = 1 }
                                          );

            Polynomial o2 = new Polynomial(
                                            new Monomial { power = 1, coefficient = 54 },
                                            new Monomial { power = 0, coefficient = 1 }
                                          );

            Assert.AreEqual(o1.ToString(), o2.ToString(), "ToString problem");
            Assert.AreEqual(o1, o2, "Equal problem");
            Assert.AreEqual(o1 == o2, false, "== problem");
            Assert.AreEqual(o1.CompareTo(o2), 0, "CompareTo problem (o1=o2)");
            o2 = o1;
            Assert.AreEqual(o1 == o2, true, "== problem (o1=o2)");
            o2 += o1;
            Assert.AreEqual(o1.CompareTo(o2), -1, "CompareTo problem (o1>o2)");
            Assert.AreEqual(o2.CompareTo(o1), 1, "CompareTo problem (o2<o1)");
            o1 = null;
            Assert.AreEqual(o2.CompareTo(o1), 1, "CompareTo problem (o1=null)");
            o2 = null;
            Assert.AreEqual(o1, o2, "Equal(null) problem");
            Assert.AreEqual(o1 == o2, true, "== problem");

        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            Polynomial o1 = new Polynomial(
                                           new Monomial { power = 1, coefficient = 54 },
                                           new Monomial { power = 0, coefficient = 1 }
                                          );

            Polynomial o2 = new Polynomial(
                                            new Monomial { power = 1, coefficient = 54 },
                                            new Monomial { power = 0, coefficient = 1 }
                                          );

            Assert.AreEqual(o1.GetHashCode(), o2.GetHashCode(), "GetHashCode problem");
            o1 += o2;
            Assert.AreNotEqual(o1.GetHashCode(), o2.GetHashCode(), "== GetHashCode problem");

        }
    }
}
