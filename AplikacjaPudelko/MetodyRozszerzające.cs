using PudelkoLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Piotr Bacior - zadanie Pudelko - 15 722 - WSEI 
namespace AplikacjaPudelko
{
    
    public static class MetodyRozszerzające
    {
        //Metoda rozszerzająca do klasy Pudelko o nazwie "Kompresuj", która zwraca objętość pudelka w metrach sześciennych
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            //Sprawdzenie, czy pudelko nie jest null
            if (pudelko == null)
                //Jeśli tak, to zgłoszenie wyjątku ArgumentNullException
                throw new ArgumentNullException(nameof(pudelko));

            //Obliczenie objętości pudelka w metrach sześciennych
            double szesciennePudelko = Math.Pow(pudelko.Objetosc, 1.0 / 3.0);
            //Zwrócenie nowego pudelka o wymiarach szesciennych
            return new Pudelko(szesciennePudelko, szesciennePudelko, szesciennePudelko, UnitOfMeasure.meter);
        }
    }
}
