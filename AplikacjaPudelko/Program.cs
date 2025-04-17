//Piotr Bacior - zadanie Pudelko - 15 722 - WSEI 

using System;
using System.Collections.Generic;
using PudelkoLibrary;

namespace AplikacjaPudelko
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tworzymy listę pudełek wraz z różnymi jednostkami oraz wymiarami 
            List<Pudelko> pudelka = new List<Pudelko>
        {
            new Pudelko(2, 3, 4, UnitOfMeasure.meter),
            new Pudelko(1, 1, 1, UnitOfMeasure.meter),
            new Pudelko(0.5, 0.5, 0.5, UnitOfMeasure.meter),
            new Pudelko(100, 200, 300, UnitOfMeasure.milimeter),
            new Pudelko(50, 50, 100, UnitOfMeasure.centimeter)
        };

            //Wypisanie pudełek przed sortowaniem 
            Console.WriteLine("Pudełka przed zastosowaniem sortownaia: ");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko);
            }

            //Kryterium sortowania jako delegat Comparison<Pudelko>
            pudelka.Sort((p1, p2) =>
            {
                int wynik = p1.Objetosc.CompareTo(p2.Objetosc);
                if (wynik == 0)
                {
                    wynik = p1.Pole.CompareTo(p2.Pole);
                }
                if (wynik == 0)
                {
                    wynik = (p1.A + p1.B + p1.C).CompareTo(p2.A + p2.B + p2.C);
                }
                return wynik;
            });

            //Wypisanie pudełek po sortowaniu
            Console.WriteLine("");
            Console.WriteLine("Pudełka po zastosowaniu sortowania: ");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko);
            }

            //Testowanie przeciążonych operatorów 
            Console.WriteLine();
            Console.WriteLine("Testowanie przeciążonych operatorów: ");
            var pudelko1 = new Pudelko(1, 2, 3, UnitOfMeasure.meter);
            var pudelko2 = new Pudelko(3, 2, 1, UnitOfMeasure.meter);
            var pudelko3 = new Pudelko(1, 1, 1, UnitOfMeasure.meter);

            Console.WriteLine($"Czy pudelko1 jest równe pudelko2 (pudelko1 == pudelko2): {pudelko1 == pudelko2}");
            Console.WriteLine($"Czy pudelko1 nie jest równe pudelko 3 (pudelko1 != pudelko3): {pudelko1 != pudelko3}");
            Console.WriteLine($"Suma pudelko2 oraz pudelko 3: {pudelko2 + pudelko3}");

            //Testowanie metody konwersji na tablice typu double[]
            Console.WriteLine();
            Console.WriteLine("Testowanie konwersji na tablicę double[]: ");
            double[] wymiaryPudelko1 = (double[])pudelko1;
            Console.WriteLine($"Tablica wymiarów pudełko1: [{string.Join("," , wymiaryPudelko1)}]");

            //Testowanie metody indeksowania 
            Console.WriteLine();
            Console.WriteLine("Testowanie metody indeksowania:");
            Console.WriteLine($"pudelko1[0]: {pudelko1[0]}, pudelko1[1]: {pudelko1[1]}, pudelko[2]: {pudelko1[2]}");

            //Testowanie metody parse
            Console.WriteLine();
            Console.WriteLine("Testowanie metody Parse:");
            var parsedPudelko = Pudelko.Parse("2.500 m × 3.000 m × 4.000 m");
            Console.WriteLine($"Pudełko utworzone za pomocą metody Parse: {parsedPudelko}");

            //Testowanie metody iteracji foreach 
            Console.WriteLine();
            Console.WriteLine("Testowanie iteracji foreach po wymiarach naszego pudełka (pudelko1)");
            foreach (var pudelko in pudelko1)
            {
                Console.WriteLine(pudelko);
            }

            //Testowanie metody Equals 
            Console.WriteLine("Testowanie metody Equals:");
            Console.WriteLine($"Czy pudelko1 jest równe pudelko2: {pudelko1.Equals(pudelko2)}");
            Console.WriteLine($"Czy pudelko1 jest równe samemu sobie: {pudelko1.Equals(pudelko1)}");

            //Testowanie właściwości pole oraz objętność
            Console.WriteLine();
            Console.WriteLine("Testowanie właściwości pole oraz objętość:");
            Console.WriteLine($"pudelko1: pole = {pudelko1.Pole}, objętość = {pudelko1.Objetosc}");

            //Testowanie metody przeciążonej ToString
            Console.WriteLine();
            Console.WriteLine("Testowanie metody przeciążonej ToString:");
            Console.WriteLine($"pudelko1 w metrach: {pudelko1.ToString("m")}");
            Console.WriteLine($"pudelko1 w centymetrach: {pudelko1.ToString("cm")}");
            Console.WriteLine($"pudelko1 w milimetrach: {pudelko1.ToString("mm")}");

            Console.WriteLine();
            Console.WriteLine("Program pudełko - Piotr Bacior - 15 722 - WSEI");
        }
    }
}
