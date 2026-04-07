using System;

namespace Algoritmalar
{
    internal static class BitwiseCalisma
    {
        public static void Calistir()
        {
            Console.WriteLine("=== Bitwise Soru Cozumleri ===");
            Console.WriteLine();

            Soru1_TekrarEdeniBul();
            Soru2_CiftMiTekMi();
            Soru3_IkininKuvvetiMi();
            Soru4_BitSayisi();
            Soru5_SayiDegistirSwap();
        }

        // Soru 1:
        // Dizide bir eleman haric tum elemanlar cift sayida tekrar ediyor.
        // Tek kalan elemani bul.
        // Cozum: XOR (^)
        // x ^ x = 0, x ^ 0 = x oldugu icin tekrar edenler silinir.
        private static void Soru1_TekrarEdeniBul()
        {
            int[] dizi = { 4, 1, 2, 1, 2, 4, 9 };
            int sonuc = 0;

            foreach (int sayi in dizi)
            {
                sonuc ^= sayi;
            }

            Console.WriteLine($"Soru 1 -> Tek kalan eleman: {sonuc}");
        }

        // Soru 2:
        // Verilen sayi cift mi tek mi?
        // Cozum: sayi & 1
        // Son bit 0 ise cift, 1 ise tek.
        private static void Soru2_CiftMiTekMi()
        {
            int sayi = 13;
            bool ciftMi = (sayi & 1) == 0;

            Console.WriteLine($"Soru 2 -> {sayi} sayisi {(ciftMi ? "cift" : "tek")}");
        }

        // Soru 3:
        // Bir sayi 2'nin kuvveti mi?
        // Cozum: n > 0 ve (n & (n - 1)) == 0
        // 2'nin kuvvetlerinde sadece tek bir bit 1 olur.
        private static void Soru3_IkininKuvvetiMi()
        {
            int n = 16;
            bool ikininKuvvetiMi = n > 0 && (n & (n - 1)) == 0;

            Console.WriteLine($"Soru 3 -> {n} sayisi 2'nin kuvveti mi? {ikininKuvvetiMi}");
        }

        // Soru 4:
        // Bir sayinin binary gosteriminde kac tane 1 biti var?
        // Cozum: Brian Kernighan yontemi
        // n &= (n - 1) her adimda en sagdaki 1 bitini temizler.
        private static void Soru4_BitSayisi()
        {
            int n = 29; // 11101 -> 4 tane 1 biti
            int adet = 0;
            int gecici = n;

            while (gecici > 0)
            {
                gecici &= (gecici - 1);
                adet++;
            }

            Console.WriteLine($"Soru 4 -> {n} sayisinda 1 bit sayisi: {adet}");
        }

        // Soru 5:
        // Iki sayiyi ucuncu degisken kullanmadan yer degistir.
        // Cozum: XOR swap
        private static void Soru5_SayiDegistirSwap()
        {
            int a = 7;
            int b = 11;

            a ^= b;
            b ^= a;
            a ^= b;

            Console.WriteLine($"Soru 5 -> Swap sonrasi a={a}, b={b}");
        }
    }
}
