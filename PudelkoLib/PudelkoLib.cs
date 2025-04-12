using System;
using System.Collections;
using System.Globalization;


namespace PudelkoLibrary
{
    //Reprezentujący jednostki miary enum o nazwie UnitOfMeasure
    public enum UnitOfMeasure
    {
        milimeter,   // Milimetry
        centimeter,  // Centymetry
        meter        // Metry
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

        //Właściwość dzięki której obliczamy objętość pudełka, zaokrągloną do 9 miejsc po przecinku
        public double Objetosc => Math.Round(A * B * C, 9);

        //Właściwość dzięki które obliczamy pole powierzchni całkowitej, zaokrągloną do 6 miejsc po przecinku
        public double Pole => Math.Round(2 * (A * B + B * C + A * C), 6);

        //Konstruktor klasy PudelkoLib przyjmujący wymiary pudełka i jednostkę miary
        public PudelkoLib(double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            //konwertujemy jednostki na metry
            this.a = ConvertToMeters(a, unit);
            this.b = ConvertToMeters(b, unit);
            this.c = ConvertToMeters(c, unit);


            //walidacja poprawności wymiarów
            WalidacjaWymiarow();
        }

        //Prywatna metoda ConvertToMeters konwertująca jednostki na metry
        private static double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            return unit switch
            {
                UnitOfMeasure.milimeter => value / 1000,
                UnitOfMeasure.centimeter => value / 100,
                UnitOfMeasure.meter => value,
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieprawidłowa jednostka miary!")
            };
        }

        private void WalidacjaWymiarow()
        {
            if (a <= 0 || b <= 0 || c <= 0 || a > 10 || b > 10 || c > 10)
                throw new ArgumentOutOfRangeException("Wymiary muszą być dodatnie i nie mogą przekraczać 10 metrów (10m)"); 
            
        }


    }
}

