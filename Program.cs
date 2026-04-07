using System;
using System.Collections.Generic;

namespace Algoritmalar
{
    internal static class Program
    {
        // Buradan programın giriş noktasını çalıştırırsın.
        // Aşağıya her soru için ayrı fonksiyonlar ekleyebilirsin.
        static void Main(string[] args)
        {
            Console.WriteLine("Algoritma soruları için C# çalışma dosyası hazır.");
            Console.WriteLine();

            // Örnek kullanım:
            // OrnekSoru1();
            BitwiseCalisma.Calistir();

            Console.WriteLine("Bir tuşa basarak çıkabilirsiniz...");
            Console.ReadKey();
        }

        // Örnek: Verilen dizideki en büyük elemanı bulan fonksiyon.
        // Bu kısmı silebilir veya kendine göre değiştirebilirsin.
        static void OrnekSoru1()
        {
            int[] dizi = { 3, 7, 2, 9, 4 };
            int max = MaxBul(dizi);
            Console.WriteLine("En büyük eleman: " + max);
        }

        static int MaxBul(int[] sayilar)
        {
            int max = sayilar[0];
            for (int i = 1; i < sayilar.Length; i++)
            {
                if (sayilar[i] > max)
                {
                    max = sayilar[i];
                }
            }

            return max;
        }
    }
}

