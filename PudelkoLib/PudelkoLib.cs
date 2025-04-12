using System;
using System.Collections;
using System.Globalization;


namespace PudelkoLibrary
{
    //Reprezentujący jednostki miary enum o nazwie UnitOfMeasure
    public enum UnitOfMeasure
    {
        milimetr, 
        centimetr,
        meter
    }
    
    //Klasa pudełko reprezentująca nasze trójwymiarowe pudełko 
    //Używamy sealed class aby zablkokować dziedziczenie klasy
    public sealed class PudelkoLib : IEquatable<PudelkoLib>, IEnumerable, IFormattable
    {
        //Pola przechowujące wymiary pudełka w metrach 
        private readonly double a;
        private readonly double b;
        private readonly double c;

        //Teraz ustawiamy właściwości dostępu do wymiarów pudełka (zaokrąglone do trzech miejsc po przecinku)
        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);
    }
}
