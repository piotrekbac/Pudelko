
using System;
using System.Collections;
using System.Globalization;
using System.Text.RegularExpressions;

//Piotr Bacior - zadanie Pudelko - 15 722 - WSEI 

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
    //Używamy sealed class aby zablokować dziedziczenie klasy
    public sealed class Pudelko : IEquatable<Pudelko>, IEnumerable, IFormattable
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

        //Właściwość dzięki której obliczamy pole powierzchni całkowitej, zaokrągloną do 6 miejsc po przecinku
        public double Pole => Math.Round(2 * (A * B + B * C + A * C), 6);

        // Konstruktor klasy Pudelko przyjmujący wymiary pudełka i jednostkę miary
        public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            double defaultValue = unit switch
            {
                UnitOfMeasure.milimeter => 100,   // 10 cm w milimetrach
                UnitOfMeasure.centimeter => 10,  // 10 cm w centymetrach
                UnitOfMeasure.meter => 0.1,      // 10 cm w metrach
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieznana jednostka miary!")
            };

            // Przypisujemy oryginalne wartości w podanej jednostce (dla walidacji)
            double originalA = a ?? defaultValue;
            double originalB = b ?? defaultValue;
            double originalC = c ?? defaultValue;

            // Walidacja oryginalnych wartości w jednostkach wejściowych
            WalidacjaWymiarow(originalA, originalB, originalC, unit);

            // Konwersja wartości na metry z precyzyjnym zaokrągleniem
            this.a = ConvertToMeters(originalA, unit); // Zaokrąglenie w ConvertToMeters
            this.b = ConvertToMeters(originalB, unit);
            this.c = ConvertToMeters(originalC, unit);
        }

        // Prywatna metoda ConvertToMeters konwertująca jednostki na metry
        private static double ConvertToMeters(double value, UnitOfMeasure unit)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Wymiary muszą być dodatnie!");

            // Konwersja bez zaokrągleń, następnie zaokrąglenie w dół (MidpointRounding.ToZero)
            return unit switch
            {
                UnitOfMeasure.milimeter => Math.Round(value / 1000.0, 3, MidpointRounding.ToZero),   // Milimetry na metry
                UnitOfMeasure.centimeter => Math.Round(value / 100.0, 3, MidpointRounding.ToZero),  // Centymetry na metry
                UnitOfMeasure.meter => Math.Round(value, 3, MidpointRounding.ToZero),              // Metry bez zmian
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieprawidłowa jednostka miary!")
            };
        }

        // Metoda WalidacjaWymiarow sprawdzająca poprawność wymiarów pudełka (muszą być dodatnie i nie większe niż 10 metrów)
        private void WalidacjaWymiarow(double originalA, double originalB, double originalC, UnitOfMeasure unit)
        {
            // Zakresy dla różnych jednostek
            double min = unit switch
            {
                UnitOfMeasure.milimeter => 1,      // Minimalna wartość w milimetrach
                UnitOfMeasure.centimeter => 0.1,  // Minimalna wartość w centymetrach
                UnitOfMeasure.meter => 0.001,     // Minimalna wartość w metrach
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieprawidłowa jednostka miary!")
            };

            double max = unit switch
            {
                UnitOfMeasure.milimeter => 10000,  // Maksymalna wartość w milimetrach
                UnitOfMeasure.centimeter => 1000, // Maksymalna wartość w centymetrach
                UnitOfMeasure.meter => 10,        // Maksymalna wartość w metrach
                _ => throw new ArgumentOutOfRangeException(nameof(unit), "Nieprawidłowa jednostka miary!")
            };

            // Walidacja: sprawdzamy oryginalne wartości w podanej jednostce
            if (originalA < min || originalA > max ||
                originalB < min || originalB > max ||
                originalC < min || originalC > max)
            {
                throw new ArgumentOutOfRangeException("Wymiary muszą być dodatnie i odpowiednie dla podanej jednostki miary!");
            }
        }

        //Przeciążanie metody ToString() do formatowania wymiarów
        // Metoda do formatowania i konwersji wymiarów pudełka do stringa
        public string ToString(string format, IFormatProvider formatProvider = null)
        {
            if (string.IsNullOrWhiteSpace(format))
            {
                format = "m"; // Domyślny format
            }

            formatProvider ??= CultureInfo.InvariantCulture;

            // Określenie jednostki miary i współczynnika konwersji
            string unit = format switch
            {
                "m" => "m",
                "cm" => "cm",
                "mm" => "mm",
                _ => throw new FormatException($"Nieznany format: {format}")
            };

            double factor = format switch
            {
                "m" => 1,
                "cm" => 100,
                "mm" => 1000,
                _ => throw new FormatException($"Nieznany format: {format}")
            };

            // Ustawienie precyzji w zależności od formatu
            string numberFormat = format switch
            {
                "m" => "0.000",  // 3 miejsca po przecinku dla metrów
                "cm" => "0.0",   // 1 miejsce po przecinku dla centymetrów
                "mm" => "0",     // Brak miejsc po przecinku dla milimetrów
                _ => throw new FormatException($"Nieznany format: {format}")
            };

            // Zwracamy sformatowany ciąg znaków z odpowiednimi zaokrągleniami
            return string.Format(formatProvider,
                "{0:" + numberFormat + "} {1} × {2:" + numberFormat + "} {1} × {3:" + numberFormat + "} {1}",
                Math.Round(A * factor, format == "m" ? 3 : format == "cm" ? 1 : 0), unit,
                Math.Round(B * factor, format == "m" ? 3 : format == "cm" ? 1 : 0),
                Math.Round(C * factor, format == "m" ? 3 : format == "cm" ? 1 : 0));
        }

        // Przeciążenie metody ToString() bez parametrów
        public override string ToString()
        {
            // Domyślny format to "m" (metry)
            return ToString("m", CultureInfo.InvariantCulture);
        }

        //Implementacja metody Equals do porównania pudełek bez względu na kolejność ich wymiarów 
        public bool Equals(Pudelko other)
        {
            //Jeżeli obiekt jest null to zwracamy false
            if (other is null) return false;

            //Jeżeli obiekt jest ten sam to zwracamy true - tworzymy tablice wymiarów
            double[] wym1 = { A, B, C };
            double[] wym2 = { other.A, other.B, other.C };

            //Sortujemy tablice wymiarów aby porównać je bez względu na kolejność
            Array.Sort(wym1);
            Array.Sort(wym2);

            //Porównujemy posortowane tablice wymiarów
            return wym1[0] == wym2[0] &&
                   wym1[1] == wym2[1] &&
                   wym1[2] == wym2[2];
        }

        //Przeciążenie metody Equals dla zgodności z operatorem ==
        public override bool Equals(object obj) => Equals(obj as Pudelko);

        //Przeciążenie metody GetHashCode() aby uzyskać unikalny kod dla każdego pudełka
        public override int GetHashCode()
        {
            //Tworzymy tablice wymiarów typu double i przypisujemy do niej wartości wymiarów A, B, C
            double[] wym = { A, B, C };
            //Sortujemy tablice wymiarów aby uzyskać unikalny kod dla każdego pudełka
            Array.Sort(wym);

            //Zwracamy unikalny kod dla każdego pudełka za pomocą metody HashCode.Combine
            return HashCode.Combine(wym[0], wym[1], wym[2]);
        }

        //Definiujemy teraz przeciążone operatory == oraz != aby mieć możliwość porównywania pudełek
        public static bool operator ==(Pudelko p1, Pudelko p2)
        {
            //Jeżeli obiekt p1 oraz p2 jest null to zwracamy true
            if (p1 is null && p2 is null) return true;
            //Jeżeli obiekt p1 lub obiekt p2 jest null to zwracamy false
            if (p1 is null || p2 is null) return false;

            //Wywołujemy metodę Equals aby porównać dwa pudełka (dla obiektu p1)
            return p1.Equals(p2);
        }

        //Przeciążenie operatora != aby mieć możliwość porównywania pudełek za pomocą operatora != (negacja)
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        //Definiujemy teraz przeciążony operator łączenia pudełek (+)
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            //Jeżeli obiekt p1 oraz p2 jest null to wyrzucamy wyjątek ArgumentNullException mówiący o tym, że nie można dodać pustych pudełek
            if (p1 is null || p2 is null)
                throw new ArgumentNullException("Nie można dodać pustego pudełka!");

            //Definiujemy nowe wymiary pudełka na podstawie wymiarów p1 i p2
            double noweA = Math.Max(p1.A, p2.A);
            double noweB = Math.Max(p1.B, p2.B);
            double noweC = Math.Max(p1.C, p2.C);

            //Tworzymy nowe pudełko na podstawie wymiarów newA, newB, newC
            return new Pudelko(noweA, noweB, noweC); //Zwracamy nowe pudełko z wymiarami A, B, C
        }

        //Definiujemy jawną konwersję (explicit) z klasy Pudelko na typ double[] (w metrach)
        public static explicit operator double[](Pudelko p) => new[] { p.A, p.B, p.C };

        //Definiujemy niejawną (implicit) konwersję z ValueTuple na typ pudełko (w milimetrach)
        public static implicit operator Pudelko(ValueTuple<int, int, int> wymiary)
        {
            return new Pudelko(wymiary.Item1, wymiary.Item2, wymiary.Item3, UnitOfMeasure.milimeter);
        }

        //Mechanizm przeglądania (tylko do odczytu - immutable) długości krawędzi pudełka poprzez odwołanie się do indeksów
        public double this[int index] => index switch
        {
            //Jeżeli indeks jest równy 0 to zwracamy wymiar A
            0 => A,
            //Jeżeli indeks jest równy 1 to zwracamy wymiar B
            1 => B,
            //Jeżeli indeks jest równy 2 to zwracamy wymiar C
            2 => C,
            //Jeżeli indeks nie pasuje do żadnej z powyższych wartości to wyrzucamy wyjątek IndexOutOfRangeException
            _ => throw new IndexOutOfRangeException("Indeks musi być z zakresu 0-2")
        };

        //Przeglądanie długości krawędzi pudełka poprzez pętlę foreach
        public IEnumerator GetEnumerator()
        {
            //Zwracamy długości krawędzi pudełka w kolejności A, B, C
            yield return A;
            yield return B;
            yield return C;
        }

        //Metoda Parse do tworzenia obiektu na podstawie tekstowej reprezentacji wymiarów pudełka
        public static Pudelko Parse(string input)
        {
            // Sprawdzamy, czy input jest nullem bądź pusty, jeśli tak, to wyrzucamy wyjątek ArgumentException
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Wejściowy string nie może być nullem bądź być pusty!");

            // Tworzymy wzorzec regex do wyodrębnienia wymiarów pudełka z tekstu
            var pattern = @"(\d+(\.\d{1,3})?)\s*(m|cm|mm)\s*×\s*(\d+(\.\d{1,3})?)\s*(m|cm|mm)\s*×\s*(\d+(\.\d{1,3})?)\s*(m|cm|mm)";
            var match = Regex.Match(input, pattern);

            // Jeśli nie udało się dopasować wzorca do tekstu, wyrzucamy wyjątek FormatException
            if (!match.Success)
                throw new FormatException("Nieprawidłowy format wymiarów pudełka!");

            // Wyodrębniamy wartości wymiarów pudełka z dopasowanego wzorca, używając kultury InvariantCulture
            double a = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            string unitA = match.Groups[3].Value;

            double b = double.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
            string unitB = match.Groups[6].Value;

            double c = double.Parse(match.Groups[7].Value, CultureInfo.InvariantCulture);
            string unitC = match.Groups[9].Value;

            // Mapujemy jednostki miary na enum UnitOfMeasure
            UnitOfMeasure unit = unitA switch
            {
                "m" => UnitOfMeasure.meter,
                "cm" => UnitOfMeasure.centimeter,
                "mm" => UnitOfMeasure.milimeter,
                _ => throw new FormatException("Nieprawidłowa jednostka miary!")
            };

            // Tworzymy nowe pudełko na podstawie wymiarów a, b, c oraz jednostki miary (unit)
            return new Pudelko(a, b, c, unit);
        }
    }
}