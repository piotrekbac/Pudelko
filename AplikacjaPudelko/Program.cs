//Piotr Bacior 15 722 - Pudelko 

using System;
using System.Collections.Generic;
using PudelkoLibrary;

namespace AplikacjaPudelko
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Pudelko> pudelka = new List<Pudelko>
        {
            new Pudelko(2, 3, 4, UnitOfMeasure.meter),
            new Pudelko(1, 1, 1, UnitOfMeasure.meter),
            new Pudelko(0.5, 0.5, 0.5, UnitOfMeasure.meter),
            new Pudelko(100, 200, 300, UnitOfMeasure.milimeter),
            new Pudelko(50, 50, 100, UnitOfMeasure.centimeter)
        };

            Console.WriteLine("Pudełka przed zastosowaniem sortownaia: ");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko);
            }

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

            Console.WriteLine("");
            Console.WriteLine("Pudełka po zastosowaniu sortowania: ");
            foreach (var pudelko in pudelka)
            {
                Console.WriteLine(pudelko);
            }
        }
    }
}
