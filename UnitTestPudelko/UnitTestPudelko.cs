using Microsoft.VisualStudio.TestTools.UnitTesting;
using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace PudelkoUnitTests
{

    [TestClass]
    public static class InitializeCulture
    {
        [AssemblyInitialize]
        public static void SetEnglishCultureOnAllUnitTest(TestContext context)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        }
    }

    // ========================================

    [TestClass]
    public class UnitTestsPudelkoConstructors
    {
        private static double defaultSize = 0.1; // w metrach
        private static double accuracy = 0.001; //dokładność 3 miejsca po przecinku

        private void AssertPudelko(Pudelko p, double expectedA, double expectedB, double expectedC)
        {
            Assert.AreEqual(expectedA, p.A, delta: accuracy);
            Assert.AreEqual(expectedB, p.B, delta: accuracy);
            Assert.AreEqual(expectedC, p.C, delta: accuracy);
        }

        #region Constructor tests ================================

        [TestMethod, TestCategory("Constructors")]
        public void Constructor_Default()
        {
            Pudelko p = new Pudelko();

            Assert.AreEqual(defaultSize, p.A, delta: accuracy);
            Assert.AreEqual(defaultSize, p.B, delta: accuracy);
            Assert.AreEqual(defaultSize, p.C, delta: accuracy);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_DefaultMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.543, 3.1,
                 1.0, 2.543, 3.1)]
        [DataRow(1.0001, 2.54387, 3.1005,
                 1.0, 2.543, 3.1)] // dla metrów liczą się 3 miejsca po przecinku
        public void Constructor_3params_InMeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100.0, 25.5, 3.1,
                 1.0, 0.255, 0.031)]
        [DataRow(100.0, 25.58, 3.13,
                 1.0, 0.255, 0.031)] // dla centymertów liczy się tylko 1 miejsce po przecinku
        public void Constructor_3params_InCentimeters(double a, double b, double c,
                                                      double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(a: a, b: b, c: c, unit: UnitOfMeasure.centimeter);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(100, 255, 3,
                 0.1, 0.255, 0.003)]
        [DataRow(100.0, 25.58, 3.13,
                 0.1, 0.025, 0.003)] // dla milimetrów nie liczą się miejsca po przecinku
        public void Constructor_3params_InMilimeters(double a, double b, double c,
                                                     double expectedA, double expectedB, double expectedC)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b, c: c);

            AssertPudelko(p, expectedA, expectedB, expectedC);
        }


        // ----

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_DefaultMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a, b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(1.0, 2.5, 1.0, 2.5)]
        [DataRow(1.001, 2.599, 1.001, 2.599)]
        [DataRow(1.0019, 2.5999, 1.001, 2.599)]
        public void Constructor_2params_InMeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(a: a, b: b, unit: UnitOfMeasure.meter);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 2.5, 0.11, 0.025)]
        [DataRow(100.1, 2.599, 1.001, 0.025)]
        [DataRow(2.0019, 0.25999, 0.02, 0.002)]
        public void Constructor_2params_InCentimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 2.0, 0.011, 0.002)]
        [DataRow(100.1, 2599, 0.1, 2.599)]
        [DataRow(200.19, 2.5999, 0.2, 0.002)]
        public void Constructor_2params_InMilimeters(double a, double b, double expectedA, double expectedB)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a, b: b);

            AssertPudelko(p, expectedA, expectedB, expectedC: 0.1);
        }

        // -------

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_DefaultMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(2.5)]
        public void Constructor_1param_InMeters(double a)
        {
            Pudelko p = new Pudelko(a);

            Assert.AreEqual(a, p.A);
            Assert.AreEqual(0.1, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11.0, 0.11)]
        [DataRow(100.1, 1.001)]
        [DataRow(2.0019, 0.02)]
        public void Constructor_1param_InCentimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.centimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(11, 0.011)]
        [DataRow(100.1, 0.1)]
        [DataRow(200.19, 0.2)]
        public void Constructor_1param_InMilimeters(double a, double expectedA)
        {
            Pudelko p = new Pudelko(unit: UnitOfMeasure.milimeter, a: a);

            AssertPudelko(p, expectedA, expectedB: 0.1, expectedC: 0.1);
        }

        // ---

        public static IEnumerable<object[]> DataSet1Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5, 3.1},
            new object[] {1.0, -2.5, 3.1},
            new object[] {1.0, 2.5, -3.1},
            new object[] {-1.0, -2.5, 3.1},
            new object[] {-1.0, 2.5, -3.1},
            new object[] {1.0, -2.5, -3.1},
            new object[] {-1.0, -2.5, -3.1},
            new object[] {0, 2.5, 3.1},
            new object[] {1.0, 0, 3.1},
            new object[] {1.0, 2.5, 0},
            new object[] {1.0, 0, 0},
            new object[] {0, 2.5, 0},
            new object[] {0, 0, 3.1},
            new object[] {0, 0, 0},
            new object[] {10.1, 2.5, 3.1},
            new object[] {10, 10.1, 3.1},
            new object[] {10, 10, 10.1},
            new object[] {10.1, 10.1, 3.1},
            new object[] {10.1, 10, 10.1},
            new object[] {10, 10.1, 10.1},
            new object[] {10.1, 10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_DefaultMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet1Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.01, 0.1, 1)]
        [DataRow(0.1, 0.01, 1)]
        [DataRow(0.1, 0.1, 0.01)]
        [DataRow(1001, 1, 1)]
        [DataRow(1, 1001, 1)]
        [DataRow(1, 1, 1001)]
        [DataRow(1001, 1, 1001)]
        [DataRow(1, 1001, 1001)]
        [DataRow(1001, 1001, 1)]
        [DataRow(1001, 1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InCentimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.centimeter);
        }


        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1, 1)]
        [DataRow(1, -1, 1)]
        [DataRow(1, 1, -1)]
        [DataRow(-1, -1, 1)]
        [DataRow(-1, 1, -1)]
        [DataRow(1, -1, -1)]
        [DataRow(-1, -1, -1)]
        [DataRow(0, 1, 1)]
        [DataRow(1, 0, 1)]
        [DataRow(1, 1, 0)]
        [DataRow(0, 0, 1)]
        [DataRow(0, 1, 0)]
        [DataRow(1, 0, 0)]
        [DataRow(0, 0, 0)]
        [DataRow(0.1, 1, 1)]
        [DataRow(1, 0.1, 1)]
        [DataRow(1, 1, 0.1)]
        [DataRow(10001, 1, 1)]
        [DataRow(1, 10001, 1)]
        [DataRow(1, 1, 10001)]
        [DataRow(10001, 10001, 1)]
        [DataRow(10001, 1, 10001)]
        [DataRow(1, 10001, 10001)]
        [DataRow(10001, 10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_3params_InMiliimeters_ArgumentOutOfRangeException(double a, double b, double c)
        {
            Pudelko p = new Pudelko(a, b, c, unit: UnitOfMeasure.milimeter);
        }


        public static IEnumerable<object[]> DataSet2Meters_ArgumentOutOfRangeEx => new List<object[]>
        {
            new object[] {-1.0, 2.5},
            new object[] {1.0, -2.5},
            new object[] {-1.0, -2.5},
            new object[] {0, 2.5},
            new object[] {1.0, 0},
            new object[] {0, 0},
            new object[] {10.1, 10},
            new object[] {10, 10.1},
            new object[] {10.1, 10.1}
        };

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_DefaultMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DynamicData(nameof(DataSet2Meters_ArgumentOutOfRangeEx))]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.01, 1)]
        [DataRow(1, 0.01)]
        [DataRow(0.01, 0.01)]
        [DataRow(1001, 1)]
        [DataRow(1, 1001)]
        [DataRow(1001, 1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InCentimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1, 1)]
        [DataRow(1, -1)]
        [DataRow(-1, -1)]
        [DataRow(0, 1)]
        [DataRow(1, 0)]
        [DataRow(0, 0)]
        [DataRow(0.1, 1)]
        [DataRow(1, 0.1)]
        [DataRow(0.1, 0.1)]
        [DataRow(10001, 1)]
        [DataRow(1, 10001)]
        [DataRow(10001, 10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_2params_InMilimeters_ArgumentOutOfRangeException(double a, double b)
        {
            Pudelko p = new Pudelko(a, b, unit: UnitOfMeasure.milimeter);
        }




        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_DefaultMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(10.1)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.meter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1.0)]
        [DataRow(0)]
        [DataRow(0.01)]
        [DataRow(1001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InCentimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.centimeter);
        }

        [DataTestMethod, TestCategory("Constructors")]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(10001)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_1param_InMilimeters_ArgumentOutOfRangeException(double a)
        {
            Pudelko p = new Pudelko(a, unit: UnitOfMeasure.milimeter);
        }

        #endregion


        #region ToString tests ===================================

        [TestMethod, TestCategory("String representation")]
        public void ToString_Default_Culture_EN()
        {
            var p = new Pudelko(2.5, 9.321);
            string expectedStringEN = "2.500 m × 9.321 m × 0.100 m";

            Assert.AreEqual(expectedStringEN, p.ToString());
        }

        [DataTestMethod, TestCategory("String representation")]
        [DataRow(null, 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("m", 2.5, 9.321, 0.1, "2.500 m × 9.321 m × 0.100 m")]
        [DataRow("cm", 2.5, 9.321, 0.1, "250.0 cm × 932.1 cm × 10.0 cm")]
        [DataRow("mm", 2.5, 9.321, 0.1, "2500 mm × 9321 mm × 100 mm")]
        public void ToString_Formattable_Culture_EN(string format, double a, double b, double c, string expectedStringRepresentation)
        {
            var p = new Pudelko(a, b, c, unit: UnitOfMeasure.meter);
            Assert.AreEqual(expectedStringRepresentation, p.ToString(format));
        }

        [TestMethod, TestCategory("String representation")]
        [ExpectedException(typeof(FormatException))]
        public void ToString_Formattable_WrongFormat_FormatException()
        {
            var p = new Pudelko(1);
            var stringformatedrepreentation = p.ToString("wrong code");
        }

        #endregion


        #region Pole, Objętość ===================================

        [TestMethod]
        public void TestPole()
        {
            //Arrange 
            Pudelko mojePudelkoPiotrBacior = new Pudelko(2, 3, 4, UnitOfMeasure.meter);

            //Act i assert
            Assert.AreEqual(52.000000000, mojePudelkoPiotrBacior.Pole);
        }

        [TestMethod]
        public void TestObjetosc()
        {
            //Arrange
            Pudelko mojePudelkoPiotrBacior = new Pudelko(2, 3, 4, UnitOfMeasure.meter);

            //Act i assert
            Assert.AreEqual(24.000000000, mojePudelkoPiotrBacior.Objetosc);
        }

        #endregion

        #region Equals ===========================================
        [TestMethod]
        public void OperatorEqauls()
        {
            //Arrange 
            Pudelko mojePudelkoPiotrBacior1 = new Pudelko(1, 2, 3, UnitOfMeasure.meter);
            Pudelko mojePudelkoPiotrBacior2 = new Pudelko(3, 1, 2, UnitOfMeasure.meter);

            //Act i assert
            Assert.IsTrue(mojePudelkoPiotrBacior1 == mojePudelkoPiotrBacior2);
        }
        #endregion

        #region Operators overloading ===========================

        [TestMethod]
        public void OperatorDodawania()
        {
            //Arrange 
            Pudelko mojePudelkoPiotrBacior1 = new Pudelko(2, 3, 4, UnitOfMeasure.meter);
            Pudelko mojePudelkoPiotrBacior2 = new Pudelko(1, 5, 3, UnitOfMeasure.meter);

            //Act
            Pudelko mojePudelkoPiotrBaciorWynik = mojePudelkoPiotrBacior1 + mojePudelkoPiotrBacior2;

            //Assert
            Assert.AreEqual(new Pudelko(2, 5, 4, UnitOfMeasure.meter), mojePudelkoPiotrBaciorWynik);
        }

        #endregion

        #region Conversions =====================================
        [TestMethod]
        public void ExplicitConversion_ToDoubleArray_AsMeters()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            double[] tab = (double[])p;
            Assert.AreEqual(3, tab.Length);
            Assert.AreEqual(p.A, tab[0]);
            Assert.AreEqual(p.B, tab[1]);
            Assert.AreEqual(p.C, tab[2]);
        }

        [TestMethod]
        public void ImplicitConversion_FromAalueTuple_As_Pudelko_InMilimeters()
        {
            var (a, b, c) = (2500, 9321, 100); // in milimeters, ValueTuple
            Pudelko p = (a, b, c);
            Assert.AreEqual((int)(p.A * 1000), a);
            Assert.AreEqual((int)(p.B * 1000), b);
            Assert.AreEqual((int)(p.C * 1000), c);
        }

        #endregion

        #region Indexer, enumeration ============================
        [TestMethod]
        public void Indexer_ReadFrom()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            Assert.AreEqual(p.A, p[0]);
            Assert.AreEqual(p.B, p[1]);
            Assert.AreEqual(p.C, p[2]);
        }

        [TestMethod]
        public void ForEach_Test()
        {
            var p = new Pudelko(1, 2.1, 3.231);
            var tab = new[] { p.A, p.B, p.C };
            int i = 0;
            foreach (double x in p)
            {
                Assert.AreEqual(x, tab[i]);
                i++;
            }
        }

        #endregion

        #region Parsing =========================================

        [TestMethod]
        public void MetodaParse()
        {
            //Arrange
            Pudelko PBPudelko = Pudelko.Parse("2.500 m × 9.321 m × 0.100 m");

            //Act i assert
            Assert.AreEqual(2.5, PBPudelko.A);
            Assert.AreEqual(9.321, PBPudelko.B);
            Assert.AreEqual(0.1, PBPudelko.C);
        }
        #endregion

        //===================================================================
        //Poniżej znajdują się dodatkowe testy jednostkowe, wprowadzone przeze mnie ku lepszemu pokryciu kodu  

        #region Testy operatora + ==================================

        //Test dla operatora dodawania pudełek o takich samych wymiarach
        [TestMethod]
        public void OperatorDodawania_TakieSameWymiary()
        {
            //Arrange
            Pudelko p1 = new Pudelko(2.5, 1.2, 3.3);
            Pudelko p2 = new Pudelko(2.5, 1.2, 3.3);

            //Act
            Pudelko wynik = p1 + p2;

            //Assert
            Assert.AreEqual(new Pudelko(2.5, 1.2, 3.3), wynik);
        }


        //Test dla operatora dodawania pudełek, gdy jedno pudełko jest null'em
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OperatorDodawania_NullPudelko()
        {
            //Arrange
            Pudelko p1 = new Pudelko(1.0, 1.0, 1.0);

            //Act i assert
            Pudelko wynik = p1 + null;
        }

        #endregion

        #region Testy Indexera =====================================

        //Test dla indexera, gdzie podajemy ujemny indeks 
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Indexer_PozaZakresem_UjemnyArgument()
        {
            //Arrange
            var p = new Pudelko(1.0, 1.0, 1.0);

            //Act i assert
            var value = p[-1];
        }

        //Test dla indexera, gdzie podajemy indeks większy od 2 (zakres od 0 do 2)
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void Indexer_PozaZakresem_ZbytDużyZakres()
        {
            //Arrange
            var p = new Pudelko(1.0, 1.0, 1.0);

            //Act i assert
            var value = p[3];
        }

        #endregion

        #region Testy operatora Equals ===================

        //Test dla operatora Equals, gdzie porównujemy dwa pudełka, sprawdzamy czy nie są null'em
        [TestMethod]
        public void Equals_KompresjaNull()
        {
            //Arrange
            Pudelko p = new Pudelko(1.0, 1.0, 1.0);

            //Act i assert
            Assert.IsFalse(p.Equals(null));
        }

        //Test dla operatora Equals, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void Equals_TakieSameObiekty()
        {
            //Arrange
            Pudelko p = new Pudelko(1.0, 1.0, 1.0);

            //Act i assert
            Assert.IsTrue(p.Equals(p));
        }

        #endregion

        #region Testy metody Parse ================================

        //Test dla metody Parse, gdzie podajemy niepoprawny format, konkretnie - bez saparatora
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidFormat_BezSeparatora()
        {
            Pudelko.Parse("2.5 m 9.3 m 1.0 m");
        }

        //Test dla metody Parse, gdzie podajemy niepoprawny format, konkretnie - niewłaściwe jednostki
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_InvalidFormat_ZłeJednostki()
        {
            Pudelko.Parse("2.5 km × 9.3 km × 1.0 km");
        }

        //Test dla metody Parse, gdzie podajemy pusty string
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Parse_PustyString()
        {
            Pudelko.Parse("");
        }

        //Test dla metody Parse, gdzie podajemy string z dodatkowymi spacjami i tabulatorami
        [TestMethod]
        public void Parse_DodatkoweSpacjeOrazTabulatory()
        {
            var p = Pudelko.Parse("  1.123  m  ×  2.345  m  ×  3.567  m  ");
            Assert.AreEqual(1.123, p.A);
            Assert.AreEqual(2.345, p.B);
            Assert.AreEqual(3.567, p.C);
        }

        //Test dla metody Parse, gdzie podajemy string z niepoprawnymi znakami
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NiewłaściweZnaki()
        {
            Pudelko.Parse("1.123m × 2.345m × abc");
        }

        #endregion

        #region Testy ToString ====================================

        //Test dla metody ToString, gdzie podajemy jednostki o różnej kulturze
        [TestMethod]
        public void ToString_RóżnaKultura()
        {
            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("pl-PL");
                var p = new Pudelko(2.5, 1.2, 3.3);
                string expected = "2.500 m × 1.200 m × 3.300 m";
                Assert.AreEqual(expected, p.ToString());
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        //Test dla metody ToString, gdzie podajemy jednostki o niewłaściwym formacie
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ToString_NiewłaściwyFormat()
        {
            var p = new Pudelko(1, 1, 1);
            p.ToString("invalid");
        }

        //Test dla metody ToString, gdzie podajemy dodatkowe spacje
        [TestMethod]
        public void ToString_DodatkoweSpacje()
        {
            var p = new Pudelko(1.123, 2.345, 3.567, UnitOfMeasure.meter);
            string result = p.ToString("m").Replace(" ", "").Trim();
            Assert.AreEqual("1.123m×2.345m×3.567m", result);
        }

        #endregion

        #region Testy wyjątków dla konstruktorów =======================

        //Test dla konstruktora, gdzie podajemy ujemne wymiary - oczekujemy wyjątku
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_Centimeters_OutOfRange()
        {
            Pudelko p = new Pudelko(a: 1001, unit: UnitOfMeasure.centimeter);
        }

        #endregion

        #region Testy walidatora ==================================

        //Test dla walidatora, gdzie podajemy niewłaściwe jednostki - oczekujemy wyjątku
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WalidacjaWymiarow_NiewłaściwaJednostka()
        {
            // Test dla nieobsługiwanej jednostki miary
            Pudelko p = new Pudelko(1, 1, 1, (UnitOfMeasure)999);
        }

        //Test dla walidatora, gdzie podajemy wymiar ujemny - oczekujemy wyjątku
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WalidacjaWymiarow_UjemnyWymiar()
        {
            // Test dla wymiaru ujemnego
            Pudelko p = new Pudelko(-1, 1, 1, UnitOfMeasure.meter);
        }

        //Test dla walidatora, gdzie podajemy wymiar zerowy - oczekujemy wyjątku
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WalidacjaWymiarow_WymiarZero()
        {
            // Test dla wymiaru zerowego
            Pudelko p = new Pudelko(0, 0, 0, UnitOfMeasure.meter);
        }

        #endregion

        #region Testy IEnumerable ================================

        //Testy dla IEnumerable, gdzie iterujemy po wymiarach pudełka
        [TestMethod]
        public void IEnumerable_IteracjaPoWymiarach()
        {
            var p = new Pudelko(1.1, 2.2, 3.3);
            var dimensions = new List<double> { 1.1, 2.2, 3.3 };

            int index = 0;
            foreach (var dimension in p)
            {
                Assert.AreEqual(dimensions[index], dimension);
                index++;
            }
        }

        #endregion

        #region Pole i Objetosc Tests ==========================

        //Test dla minimalnego wymiaru pola 
        [TestMethod]
        public void Pole_MinWymiar()
        {
            var p = new Pudelko(0.001, 0.001, 0.001, UnitOfMeasure.meter);
            Assert.AreEqual(0.000006, p.Pole); // 6 * 10^-6
        }

        //Test makysmalnego wymiaru objętości
        [TestMethod]
        public void Objetosc_MaxWymiar()
        {
            var p = new Pudelko(10, 10, 10, UnitOfMeasure.meter);
            Assert.AreEqual(1000, p.Objetosc); // 10^3
        }

        #endregion

        #region Testy operatora przepełnienia =======================

        //Test dla operatora dodawania, gdzie suma wymiarów przekracza maksymalne wartości
        [TestMethod]
        public void OperatorDodawania_MaxDimensions()
        {
            var p1 = new Pudelko(10, 10, 10, UnitOfMeasure.meter);
            var p2 = new Pudelko(10, 10, 10, UnitOfMeasure.meter);
            var result = p1 + p2;

            Assert.AreEqual(10, result.A);
            Assert.AreEqual(10, result.B);
            Assert.AreEqual(10, result.C);
        }

        //Test dla operatora dodawania takich samych obiektów
        [TestMethod]
        public void OperatorEquals_TakieSameObiekty()
        {
            var p = new Pudelko(1, 2, 3, UnitOfMeasure.meter);
            Assert.IsTrue(p == p);
        }


        #endregion

        #region Testy dla ArgumentOutOfRangeException =====================

        //Test dla walidacji wymiarów, gdzie podajemy niewłaściwe jednostki
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WalidacjaWymiarow_NiewlasciweJednostki_WyrzucaWyjątek()
        {
            //Arrange
            double a = 1.0, b = 1.0, c = 1.0;
            UnitOfMeasure unit = (UnitOfMeasure)999; // Nieprawidłowa jednostka

            //Act
            var pudelko = new Pudelko(a, b, c, unit);
        }

        #endregion

        #region Testy dla FormatException =====================

        //Test dla metody ToString, gdzie podajemy niepoprawny format
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ToString_NiepoprawnyFormat_WyrzucaWyjątek()
        {
            //Arrange
            var p = new Pudelko(1, 1, 1);

            //Act i assert
            p.ToString("invalid_format");
        }

        //Test dka metody Parse, gdzie podajemy niepoprawne jednostki
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NiepoprawneJednostki_WyrzucaWyjątek()
        {
            //Arrange
            string invalidInput = "2.5 km × 9.3 km × 1.0 km"; // Nieprawidłowe jednostki

            //Act
            Pudelko.Parse(invalidInput);
        }

        //Test dla metody Parse, gdzie podajemy niepoprawny format
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NieprawidłowyFormat_WyrzucaWyjątek()
        {
            //Arrange
            string invalidInput = "2.5×9.3×1.0"; // Brak wymaganych separatorów i jednostek

            //Act
            Pudelko.Parse(invalidInput);
        }

        #endregion

        #region Testy dla Equals =====================

        //Testy dla Equals, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void Equals_TakieSameWymiary_ZwracaPrawde()
        {
            //Arrange
            var p1 = new Pudelko(1.0, 2.0, 3.0);
            var p2 = new Pudelko(3.0, 1.0, 2.0); // Te same wymiary, ale w innej kolejności

            //Act i Assert
            Assert.IsTrue(p1.Equals(p2));
        }

        //Testy dla Equals, gdzie porównujemy dwa pudełka o różnych wymiarach
        [TestMethod]
        public void Equals_RóżneWymiary_ZwracaFałsz()
        {
            //Arrange
            var p1 = new Pudelko(1.0, 2.0, 3.0);
            var p2 = new Pudelko(4.0, 5.0, 6.0);

            //Act & Assert
            Assert.IsFalse(p1.Equals(p2));
        }

        #endregion

        #region Testy dla GetHashCode =====================

        //Testy dla GetHashCode, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void GetHashCode_TeSameWymiary_ZwracaTenSamHash()
        {
            //Arrange
            var p1 = new Pudelko(1.0, 2.0, 3.0);
            var p2 = new Pudelko(3.0, 1.0, 2.0); // Te same wymiary, ale w innej kolejności

            //Act
            int hash1 = p1.GetHashCode();
            int hash2 = p2.GetHashCode();

            //Assert
            Assert.AreEqual(hash1, hash2);
        }

        //Testy dla GetHashCode, gdzie porównujemy dwa pudełka o różnych wymiarach
        [TestMethod]
        public void GetHashCode_RóżneWymiary_ZwracaInnyHash()
        {
            //Arrange
            var p1 = new Pudelko(1.0, 2.0, 3.0);
            var p2 = new Pudelko(4.0, 5.0, 6.0);

            //Act
            int hash1 = p1.GetHashCode();
            int hash2 = p2.GetHashCode();

            //Assert
            Assert.AreNotEqual(hash1, hash2);
        }

        #endregion

        #region Testy dla operatorów == oraz != =====================

        //Testy dla operatora ==, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void OperatorEquals_ObydwaNull_ZwracaPrawdę()
        {
            //Arrange
            Pudelko p1 = null;
            Pudelko p2 = null;

            //Act i Assert
            Assert.IsTrue(p1 == p2);
        }

        //Testy dla operatora ==, gdzie porównujemy dwa pudełka o różnych wymiarach
        [TestMethod]
        public void OperatorEquals_JedenNull_ZwracaFałsz()
        {
            //Arrange
            Pudelko p1 = new Pudelko(1, 2, 3);
            Pudelko p2 = null;

            //Act i Assert
            Assert.IsFalse(p1 == p2);
        }

        //Testy dla operatora !=, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void OperatorNotEquals_ObydwaNull_ZwracaFałsz()
        {
            //Arrange
            Pudelko p1 = null;
            Pudelko p2 = null;

            //Act i Assert
            Assert.IsFalse(p1 != p2);
        }

        #endregion

        #region Testy dla Parse =====================

        //Testy dla metody Parse, gdzie podajemy poprawny format
        [TestMethod]
        public void Parse_ValidInput_ZwracaPrawidłowePudełko()
        {
            // Arrange
            string input = "2.500 m × 9.321 m × 0.100 m";

            // Act
            var p = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(2.5, p.A);
            Assert.AreEqual(9.321, p.B);
            Assert.AreEqual(0.1, p.C);
        }

        //Testy dla metody Parse, gdzie podajemy niepoprawny format - nieznaną jednostkę
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NieznanaJednostka_WyrzucaWyjątek()
        {
            // Arrange
            string input = "2.5 dm × 9.3 dm × 1.0 dm"; // Nieznana jednostka "dm"

            // Act
            Pudelko.Parse(input);
        }

        #endregion

        #region Testy dla ArgumentOutOfRangeException =====================

        // Test dla wymiarów, które są niedodatnie (ujemne lub 0)
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NieprawidłowyWymiar_WyrzucaArgumentOutOfRangeException()
        {
            // Arrange & Act
            var pudelko = new Pudelko(-1, 1, 1, UnitOfMeasure.meter);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_WymiarZerowy_WyrzucaArgumentOutOfRangeException()
        {
            // Arrange & Act
            var pudelko = new Pudelko(0, 1, 1, UnitOfMeasure.meter);
        }

        #endregion

        #region Testy dla ArgumentOutOfRangeException dla nieprawidłowej jednostki =====================

        // Test dla konstruktora, gdzie podajemy nieprawidłową jednostkę
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Constructor_NieprawidłowaJednostka_WyrzucaArgumentOutOfRangeException()
        {
            // Arrange & Act
            var pudelko = new Pudelko(1, 1, 1, (UnitOfMeasure)999); // Nieprawidłowa jednostka
        }

        #endregion

        #region Testy dla FormatException =====================

        // Test dla metody ToString z nieznanym formatem
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ToString_NieprawidłowyFormat_WyrzucaFormatException()
        {
            // Arrange
            var pudelko = new Pudelko(1, 1, 1);

            // Act
            pudelko.ToString("invalid");
        }

        // Test dla metody Parse z nieznaną jednostką miary
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Parse_NieprawidłowaJednostka_WyrzucaFormatException()
        {
            // Arrange
            string input = "2.5 dm × 9.3 dm × 1.0 dm"; // Nieznana jednostka "dm"

            // Act
            Pudelko.Parse(input);
        }

        #endregion

        #region Testy dla Equals =====================

        // Test dla metody Equals, gdzie porównujemy obiekt null
        [TestMethod]
        public void Equals_ObjektNull_ZwracaFałsz()
        {
            // Arrange
            var pudelko = new Pudelko(1, 1, 1);

            // Act
            bool result = pudelko.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        // Test dla metody Equals, gdzie porównujemy obiekt innego typu
        [TestMethod]
        public void Equals_RóżnyTypObiektów_ZwracaFałsz()
        {
            // Arrange
            var pudelko = new Pudelko(1, 1, 1);

            // Act
            bool result = pudelko.Equals("Not a Pudelko");

            // Assert
            Assert.IsFalse(result);
        }

        // Test dla metody Equals, gdzie porównujemy dwa pudełka o takich samych wymiarach
        [TestMethod]
        public void Equals_IdentycznyWymiar_ZwracaPrawdę()
        {
            // Arrange
            var pudelko1 = new Pudelko(1, 1, 1);
            var pudelko2 = new Pudelko(1, 1, 1);

            // Act
            bool result = pudelko1.Equals(pudelko2);

            // Assert
            Assert.IsTrue(result);
        }

        #endregion

        #region Testy dla Parse z jednostkami =====================

        // Test dla metody Parse z jednostką "cm"
        [TestMethod]
        public void Parse_JednostkaCentymetry_SprasowaneWPełni()
        {
            // Arrange
            string input = "2.5 cm × 9.3 cm × 1.0 cm";

            // Act
            var pudelko = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(0.025, pudelko.A);
            Assert.AreEqual(0.093, pudelko.B);
            Assert.AreEqual(0.01, pudelko.C);
        }

        // Test dla metody Parse z jednostką "mm"
        [TestMethod]
        public void Parse_JednostkaMilimetry_SparsowaneWPełni()
        {
            // Arrange
            string input = "25 mm × 93 mm × 10 mm";

            // Act
            var pudelko = Pudelko.Parse(input);

            // Assert
            Assert.AreEqual(0.025, pudelko.A);
            Assert.AreEqual(0.093, pudelko.B);
            Assert.AreEqual(0.01, pudelko.C);
        }

        #endregion

    }
}
