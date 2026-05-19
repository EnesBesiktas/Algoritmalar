# Lab Soruları — Recursive Bölme, Bitwise Toplama, Tatlı Optimizasyonu, Bellman-Ford

Bu dosya lab formatında dört soru ve çalışır C# örneklerini içerir. `Program.cs` ile çakışmaması için buradaki `Main` örnekleri **ayrı bir konsol projesinde** veya tek tek kopyalayıp denemeniz içindir.

---

## Lab 1 — Çıkarma ile tam sayı bölme (recursive)

**İstek:** İki **pozitif** tamsayı `bolunen` ve `bolen` verildiğinde `bolunen / bolen` (tam kısım) sonucunu **yalnızca çıkarma** kullanarak özyinelemeli hesaplayın.

**Temel fikir:** `bolunen >= bolen` iken her adımda `bolen` kadar çıkar; her çıkarma **+1** bölüm payıdır. `bolunen < bolen` olunca dur.

**Not:** `bolen == 0` tanımsızdır; gerçek kodda başta kontrol edilmelidir.

```csharp
using System;

class Program
{
    static void Main()
    {
        int bolunen = 17;
        int bolen = 5;

        if (bolen <= 0)
        {
            Console.WriteLine("Hata: bolen pozitif olmalıdır.");
            return;
        }

        int sonuc = Bol(bolunen, bolen);
        Console.WriteLine("Sonuç: " + sonuc); // 17/5 -> 3
    }

    /// <summary>
    /// Pozitif tamsayılar için tam bölüm; sadece çıkarma ve karşılaştırma kullanır.
    /// </summary>
    static int Bol(int bolunen, int bolen)
    {
        // Taban: bölünen bölenden küçükse bölüm 0
        if (bolunen < bolen)
            return 0;

        // Bir adet bolen çıkardık -> bölüme +1, kalanı tekrar böl
        return 1 + Bol(bolunen - bolen, bolen);
    }
}
```

---

## Lab 2 — Bitwise operatörlerle toplama ve taşma (overflow)

**İstek:** İki 32 bit `int` değeri **bitwise** (`^`, `&`, `<<`) kullanarak toplayın; **taşma** olup olmadığını tespit edip konsola `"Taşma var"` veya `"Taşma yok"` yazdırın.

**Toplama:** Klasik yarım toplayıcı mantığı — `a ^ b` toplamın XOR kısmı, `(a & b) << 1` elde taşınan bitler; elde sıfırlanana kadar tekrarlanır.

**Taşma:** Ara değerler `int` içinde dolanabilir; güvenilir kontrol için hesabı **`long`** üzerinde yapıp sonucun `int.MinValue`…`int.MaxValue` aralığında olup olmadığına bakmak uygundur.

```csharp
using System;

class Program
{
    static void Main()
    {
        int x = int.MaxValue; // 2147483647
        int y = 1;

        bool tasiyor = BitwiseToplaTaşmaKontrol(x, y, out int sonuc);

        Console.WriteLine(tasiyor ? "Taşma var" : "Taşma yok");
        Console.WriteLine("Bitwise toplam (int olarak): " + sonuc);
    }

    /// <summary>
    /// 32-bit int girişleri için XOR/AND/shift ile toplama; long ile taşma kontrolü.
    /// </summary>
    static bool BitwiseToplaTaşmaKontrol(int a, int b, out int sonuc)
    {
        long x = a;
        long y = b;

        // Elde bitleri (carry) sıfırlanana kadar XOR toplamını güncelle
        while (y != 0)
        {
            long carry = (x & y) << 1;
            x = x ^ y;
            y = carry;
        }

        if (x > int.MaxValue || x < int.MinValue)
        {
            sonuc = unchecked((int)x); // İstenirse sarılmış değer
            return true; // taşma
        }

        sonuc = (int)x;
        return false;
    }
}
```

**Not:** Sadece `int` ile XOR döngüsü yapıp taşmayı `unchecked` sonrası `toplam - x == y` ile yakalamak da mümkündür; bu labda istenen **bitwise toplama** ile tutarlılık için yukarıdaki yöntem tercih edilmiştir.

---

## Lab 3 — Diyetisyen tatlı tabağı (brute-force / recursive)

**Kurallar:**

- Toplam gram: **≤ 15**
- **En fazla 3 farklı malzeme** (gramı > 0 sayılan çeşit)
- Her malzemeden **0…5 gram**, tam sayı
- Toplam şeker: **≤ 20** (tablodaki şeker/gram katsayıları ile)
- **Kalori maksimum** (kalori/gram: A=5, B=4, C=7, D=3; şeker/gram: A=2, B=3, C=1, D=4)

### Yaklaşım A — Encapsulation + yaprakta kısıt kontrolü

Tüm kombinasyonlar `Dene(0)` ile derinlikte denenir; `index == 4` olunca toplamlar hesaplanır, kurallar sağlanıyorsa en iyi kalori güncellenir.

```csharp
using System;

/// <summary>
/// Verileri ve arama mantığını tek sınıfta toplar (encapsulation).
/// </summary>
class TatliSecici
{
    private readonly int[] kaloriler = { 5, 4, 7, 3 };
    private readonly int[] sekerler = { 2, 3, 1, 4 };
    private readonly string[] isimler = { "A", "B", "C", "D" };

    private readonly int[] secim = new int[4];
    private readonly int[] enIyiSecim = new int[4];
    private int enYuksekKalori = -1;

    public void Hesapla()
    {
        Dene(0);
        YazdirSonuc();
    }

    // Her malzeme için 0..5 gram dene; 4 malzeme bitince kombinasyonu değerlendir
    private void Dene(int index)
    {
        if (index == 4)
        {
            int toplamGram = 0;
            int toplamKalori = 0;
            int toplamSeker = 0;
            int seciliCesit = 0;

            for (int i = 0; i < 4; i++)
            {
                if (secim[i] > 0)
                {
                    seciliCesit++;
                    toplamGram += secim[i];
                    toplamKalori += kaloriler[i] * secim[i];
                    toplamSeker += sekerler[i] * secim[i];
                }
            }

            // Tüm kısıtlar yaprak düğümde kontrol edilir
            if (toplamGram <= 15 && toplamSeker <= 20 && seciliCesit <= 3)
            {
                if (toplamKalori > enYuksekKalori)
                {
                    enYuksekKalori = toplamKalori;
                    for (int i = 0; i < 4; i++)
                        enIyiSecim[i] = secim[i];
                }
            }

            return;
        }

        // Bu malzemeden 0,1,...,5 gram seçimlerini sırayla dene
        for (int g = 0; g <= 5; g++)
        {
            secim[index] = g;
            Dene(index + 1);
        }
    }

    private void YazdirSonuc()
    {
        Console.WriteLine("En yüksek kalori: " + enYuksekKalori);
        Console.Write("Seçilenler: ");
        for (int i = 0; i < 4; i++)
        {
            if (enIyiSecim[i] > 0)
                Console.Write("{0}:{1}g  ", isimler[i], enIyiSecim[i]);
        }

        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        new TatliSecici().Hesapla();
    }
}
```

### Yaklaşım B — Erken budama (daha az dallanma)

Aynı problem; kısımlar **recursive çağrıdan önce** kontrol edilir, limit aşılıyorsa iç döngü `break` ile kesilir.

```csharp
using System;

class TatliSeciciBudamali
{
    private const int N = 4;

    private readonly int[] kaloriler = { 5, 4, 7, 3 };
    private readonly int[] sekerler = { 2, 3, 1, 4 };
    private readonly string[] isimler = { "A", "B", "C", "D" };

    private readonly int[] secim = new int[N];
    private readonly int[] enIyiSecim = new int[N];
    private int enYuksekKalori = -1;

    public void Hesapla()
    {
        Dene(0, 0, 0, 0, 0);
        YazdirSonuc();
    }

    /// <param name="gram">Şu ana kadarki toplam gram</param>
    /// <param name="seker">Şu ana kadarki toplam şeker</param>
    /// <param name="cesit">Gramı &gt; 0 olan malzeme sayısı</param>
    /// <param name="kalori">Şu ana kadarki toplam kalori</param>
    private void Dene(int index, int gram, int seker, int cesit, int kalori)
    {
        if (index == N)
        {
            // Budama sayesinde buraya sadece uygun kısmi yolların tamamlanmış hali gelir
            if (kalori > enYuksekKalori)
            {
                enYuksekKalori = kalori;
                for (int i = 0; i < N; i++)
                    enIyiSecim[i] = secim[i];
            }

            return;
        }

        for (int g = 0; g <= 5; g++)
        {
            int yeniGram = gram + g;
            int yeniSeker = seker + sekerler[index] * g;
            int yeniCesit = cesit + (g > 0 ? 1 : 0);

            // Gram / şeker / çeşit sınırını aşan dalları kes
            if (yeniGram > 15) break;
            if (yeniSeker > 20) break;
            if (yeniCesit > 3) break;

            secim[index] = g;
            Dene(index + 1, yeniGram, yeniSeker, yeniCesit, kalori + kaloriler[index] * g);
        }

        secim[index] = 0;
    }

    private void YazdirSonuc()
    {
        Console.WriteLine("En yüksek kalori: " + enYuksekKalori);
        Console.Write("Seçilenler: ");
        for (int i = 0; i < N; i++)
        {
            if (enIyiSecim[i] > 0)
                Console.Write("{0}:{1}g  ", isimler[i], enIyiSecim[i]);
        }

        Console.WriteLine();
    }
}
```

**Dallanma özeti:** `Dene(0) → secim[0]=g₀ → Dene(1) → … → Dene(4)` yaprakta değerlendirme veya yolda budama.

---

## Lab 4 — Bellman-Ford (negatif kenar, negatif döngü tespiti)

**Özet:** Başlangıç düğümüne mesafe 0, diğerlerine “sonsuz”. Tüm kenarlar üzerinde **V−1** tur **relax** (`dist[u] + w < dist[v]` ise güncelle). Ek bir turda hâlâ iyileşme varsa **negatif ağırlıklı döngü** vardır.

**Örnek graf:** A(0)→B(1) ağırlık −4, B→C −2, C→A −3 → toplam döngü ağırlığı negatif; algoritma bunu son turda yakalar.

```csharp
using System;

class Program
{
    static void Main()
    {
        int N = 3;

        // Kenarlar: [u, v, w]
        int[,] kenarlar = new int[3, 3]
        {
            { 0, 1, -4 }, // A -> B
            { 1, 2, -2 }, // B -> C
            { 2, 0, -3 }  // C -> A
        };

        int kenarSayisi = 3;

        int[] mesafe = new int[N];
        for (int i = 0; i < N; i++)
            mesafe[i] = int.MaxValue;

        int baslangic = 0; // A
        mesafe[baslangic] = 0;

        // V - 1 kez tüm kenarları gevşet
        for (int i = 0; i < N - 1; i++)
        {
            for (int j = 0; j < kenarSayisi; j++)
            {
                int u = kenarlar[j, 0];
                int v = kenarlar[j, 1];
                int w = kenarlar[j, 2];

                if (mesafe[u] != int.MaxValue && mesafe[u] + w < mesafe[v])
                    mesafe[v] = mesafe[u] + w;
            }
        }

        // Negatif döngü kontrolü (V. tur)
        bool negatifDongu = false;
        for (int j = 0; j < kenarSayisi; j++)
        {
            int u = kenarlar[j, 0];
            int v = kenarlar[j, 1];
            int w = kenarlar[j, 2];

            if (mesafe[u] != int.MaxValue && mesafe[u] + w < mesafe[v])
            {
                negatifDongu = true;
                break;
            }
        }

        if (negatifDongu)
            Console.WriteLine("Negatif agirlikli bir dongu bulundu!");
        else
        {
            Console.WriteLine("A noktasindan en kisa mesafeler:");
            Console.WriteLine("A: " + mesafe[0]);
            Console.WriteLine("B: " + mesafe[1]);
            Console.WriteLine("C: " + mesafe[2]);
        }
    }
}
```

### Bellman-Ford ne zaman?

| Durum | Not |
|--------|-----|
| Kenarlar pozitif | Kullanılabilir; genelde **Dijkstra** daha hızlı (`O((V+E) log V)` min-heap ile). |
| Negatif kenarlar | **Bellman-Ford** uygun; `O(V·E)`. |
| Negatif döngü riski | BF son turda **tespit** edebilir; döngü varsa tek kaynak–tüm düğümler anlamında en kısa yol **tanımsız** olabilir. |

### Dijkstra vs Bellman-Ford (kısa)

1. **Negatif ağırlık:** Klasik Dijkstra negatif kenarda güvenilir değildir; BF negatif kenarı işler (döngü yoksa).  
2. **Karmaşıklık:** Dijkstra `O((V+E) log V)` (heap); BF `O(V·E)`.  
3. **Strateji:** Dijkstra greedy (o an en yakın düğüm); BF tüm kenarlarda tekrarlı relax.  
4. **Yönsüz graf:** BF yönlü kenar listesiyle tanımlanır; yönsüz kenar genelde **iki yönlü kenar** olarak kodlanır. “Sadece yönlü” ifadesi pratikte modelleme tercihidir; algoritmanın kendisi yönsüzü iki kenarla temsil ederek çalıştırılabilir.

### Aşamalar (özet tablo)

| Aşama | Ne yapıyor? |
|--------|----------------|
| Başlangıç | `dist` çoğu ∞, kaynak 0 |
| V−1 tur relax | Her kenar için kısaltma mümkünse güncelle |
| Son tur | Hâlâ kısalıyorsa → negatif döngü |

---

## Dosya konumu

Bu lab içeriği projede şu dosyada tutulur: `Lab_Recursive_Bitwise_Tatli_BellmanFord.md`.
