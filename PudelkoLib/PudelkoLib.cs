using System;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;


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
    public sealed class PudelkoLib :  IFormattable
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

        //Metoda WalidacjaWymiarow sprawdzająca poprawność wymiarów pudełka (muszą być dodatnie i nie większe niż 10 metrów)
        private void WalidacjaWymiarow()
        {
            if (a <= 0 || b <= 0 || c <= 0 || a > 10 || b > 10 || c > 10)
                throw new ArgumentOutOfRangeException("Wymiary muszą być dodatnie i nie mogą przekraczać 10 metrów (10m)");

        }

        //Przciązanie metody ToString() do formowatowania wymiarów
        public override string ToString()
        {
            return ToString("m");
        }

        //Metoda do formatowania i konwersji wymiarów pudełka do stringa
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            //Jeśli format jest pusty (formatProvider = null) to ustawiamy domyślny format - domyślna kultura (CultureInfo.CurrentCulture) - formatowanie liczb w zależności od języka bądź regionu
            formatProvider ??= CultureInfo.CurrentCulture;

            //Na podstawie wartości format switch przypisywana jest odpowiednia jednostka 
            string unit = format switch
            {
                "m" => "m",         //przypisanie wartości m jak metry 
                "cm" => "cm",       //przypisanie wartości cm jak centymetry
                "mm" => "mm",       //przypisanie wartości mm jak milimetry
                //Jeżeli format nie pasuje do żadnej z powyższych jednostek to wyrzucamy wyjątek o nieznanym formacie.
                _ => throw new FormatException($"Nieznany format: {format}")
            };

            //Na podstawie wartości format switch określany jest współczynnik przeliczeniowy dla danych jednostek
            double factor = format switch
            {
                "m" => 1,           //brak przeliczenia - jednostka bazowa
                "cm" => 100,        //przeliczenie na centymetry (1 metr to 100 centymetrów) 
                "mm" => 1000,       //przeliczenie na milimetry (1 metr to 1000 milimetrów)
                //Jeżeli format nie pasuje do żadnej z powyższych jednostek to wyrzucamy wyjątek o nieznanym formacie.
                _ => throw new FormatException($"Nieznany format: {format}")
                
            };


            //Metoda string.Format formatuje tekst z wykorzystaniem miejsc zastępczych {n}
            return string.Format("{0:0.###} {1} × {2:0.###} {1} × {3:0.###} {1}",
                //{0:0.###}: Pierwsza liczba (A * factor) zaokrąglana jest do maksymalnie trzech miejsc po przycinku
                A * factor, unit, B * factor, C * factor);
            //{1}: Jednostka miary (unit) czyli przykładowo np. "m", "cm", "mm"
            //{2:0.###}: Druga liczba (B * factor), również zaokrąglona do trzech miejsc po przecinku.
            //{3:0.###}: Trzecia liczba (C * factor), również zaokrąglona do trzech miejsc po przecinku.

        }

        //Metoda Parse do tworzenia obiektu na podstawie tekstowej reprezentacji wymiarów pudełka
        public static PudelkoLib Parse(string input)
        {
            throw new NotImplementedException();
        }




    }
}

