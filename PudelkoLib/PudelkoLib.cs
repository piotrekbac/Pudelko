using System;
using System.Collections;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;


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
    public sealed class Pudelko :  IEquatable<Pudelko>, IEnumerable ,IFormattable
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
        public Pudelko(double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter)
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


        //Implementacja metody Equals do porównania pudełek bez względu na kolejnośc ich wymiarów 
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
        //Sprawdzamy czy dowolny obiekt (object obj) jest instancją klasy pudełko, jeżeli tak to porównujemy te dwa obiekty za pomocą metody (Pudelko other)
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

        //Teraz tworzymy przeciążone operatory == oraz != aby mieć możliwość porównywania pudełek
        public static bool operator ==(Pudelko p1, Pudelko p2)
        {
            //Jeżeli obiekt p1 oraz p2 jest null to zwracamy false
            if (p1 is null && p2 is null) return true;
            //Jeżeli obiekt p1 lub obiekt p2 jest null to zwracamy false
            if (p1 is null || p2 is null) return false;

            //Wywołujemy metodę Equals aby porównać dwa pudełka (dla obiektu p1)
            return p1.Equals(p2);   
        }

        //Przeciążenie operatora != aby mieć możliwość porównywania pudełek za pomocą operatora != (negacja)
        //Warto zauważyć, że ten operator musi zostać nadpisany w taki sposób, aby był zgodny z operatorem ==
        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        //Definiujemy teraz przeciązony operator łączenia pudełek (+)
        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            //Jeżeli obiekt p1 oraz p2 jest null to wyrzucamy wyjątek ArgumentNullException mówiący o tym, że nie można dodać pustych pudełek
            if (p1 is null || p2 is null)
                throw new ArgumentNullException("Nie można dodać pustego pudełka!");

            //definiujemy nowe wymiary pudełka na podstawie wymiarów p1 i p2
            double noweA = Math.Max(p1.A, p2.A);
            double noweB = Math.Max(p1.B, p2.B);
            double noweC = Math.Max(p1.C, p2.C);

            //Tworzymy nowe pudełko na podstawie wymiarów newA, newB, newC
            return new Pudelko(noweA, noweB, noweC); //Zwracamy nowe pudełko z wymiarami A, B, C
        }

        //Definiujemy jawną konwersję (explicit) z klasy Pudelko na typ double[] (w metrach)
        public static explicit operator double[](Pudelko p) => new [] { p.A, p.B, p.C };

        //Definiujemy niejawną (implicit) konwersję z ValueTuple na typ pudełko (w milimetrach)
        public static implicit operator Pudelko(ValueTuple<int, int, int> wymiary)
        {
            return new Pudelko(wymiary.Item1, wymiary.Item2, wymiary.Item3, UnitOfMeasure.milimeter);
        }

        //Mechanizm przeglądarnia (tylko do odczytu - immutable) długości krawędzi pudełka poprzez odwołanie się do indeksów
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

        //Przeglądanie długości krawędzi pudełka poprzez pętle foreach
        public IEnumerator GetEnumerator()
        {
            //Zwracamy długość krawędzi pudełka w kolejności A, B, C
            //Używamy yield return aby zwrócić długości krawędzi pudełka A, B, C w konkretnie ustalonej kolejności
            yield return A;
            yield return B;
            yield return C;
        }

        //Metoda Parse do tworzenia obiektu na podstawie tekstowej reprezentacji wymiarów pudełka
        public static Pudelko Parse(string input)
        {
            //sprawdzamy czy input jest nullem bądź pusty, jezeli tak to wyrzucamy wyjątek ArgumentException
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Wejściowy string nie może być nullem bądź być pusty!");

            //Tworzymy wzorzec regex do wyodrębnienia wymiarów pudełka z tekstu
            //wzorzec wyrażenia regularnego (regex) do wyodrębnienia wymiarów pudełka
            var pattern = @"(\d+(\.\d{1,3})?)\s*(m|cm|mm)\s*×\s*(\d+(\.\d{1,3})?)\s*(m|cm|mm)\s*×\s*(\d+(\.\d{1,3})?)\s*(m|cm|mm)";
            //Regex.Match() - metoda do dopasowania wzorca regex do tekstu
            var match = Regex.Match(input, pattern);

            //Jeżeli nie udało się dopasować wzorca do tekstu to wyrzucamy wyjątek FormatException
            if (!match.Success)
                throw new FormatException("Nieprawidłowy format wymiarów pudełka!");

            //Wyodrębniamy wartości wymiarów pudełka z dopasowanego wzorca
            //match.Groups[1].Value - wartość wymiaru A
            double a = double.Parse(match.Groups[1].Value);
            //match.Groups[3].Value - jednostka miary A
            string unitA = match.Groups[3].Value;

            //match.Groups[4].Value - wartość wymiaru B
            double b = double.Parse(match.Groups[4].Value);
            //match.Groups[6].Value - jednostka miary B
            string unitB = match.Groups[6].Value;

            //match.Groups[7].Value - wartość wymiaru C
            double c = double.Parse(match.Groups[7].Value);
            //match.Groups[9].Value - jednostka miary C
            string unitC = match.Groups[9].Value;

            //Mapujemy jednostki miary A, B, C na jedną jednostkę miary im odpowiadającą 
            UnitOfMeasure unit = unitA switch
            {
                "m" => UnitOfMeasure.meter,
                "cm" => UnitOfMeasure.centimeter,
                "mm" => UnitOfMeasure.milimeter,
                _ => throw new FormatException("Nieprawidłowa jednostka miary!")
            };

            //Tworzymy nowe ppudełko na podstawie wymiarów a, b, c oraz jednostki miary (unit)
            return new Pudelko(a, b, c, unit);
        }

    }
}

