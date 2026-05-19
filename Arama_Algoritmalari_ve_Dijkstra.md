# Arama Algoritmaları ve Dijkstra (Graf)

Bu dosya doğrusal / ikili / atlamalı / üssel arama ile komşuluk matrisi üzerinde Dijkstra örneklerini içerir. Kodlar `Program.cs` ile çakışmaması için ayrı kopyalanıp denenebilir.

---

## LINEAR SEARCH (Doğrusal Arama)

Küçük veri kümeleri için uygundur; dizi **sıralı olmak zorunda değildir**. Baştan sona her eleman kontrol edilir. Aranan bulunursa **indeks**, yoksa **-1** döner.

**Karmaşıklık:** En kötü durumda eleman sonda veya yok → **n** kez bakılır → **O(n)**.

```csharp
static int DogrusalArama(int[] dizi, int aranan)
{
    for (int i = 0; i < dizi.Length; i++)
    {
        if (dizi[i] == aranan)
            return i; // Aranan bulundu, indeks döndürülüyor
    }

    return -1; // Aranan eleman dizide yok
}
```

---

## BINARY SEARCH (İkili Arama)

**Sadece sıralı** dizilerde çalışır. Ortadaki elemana bakılarak sol veya sağ yarı seçilir. **O(log n)**.

**Fikir:** Diziyi ikiye böl → aranan hangi yarıda? → Tek elemana inene kadar tekrarla.

```csharp
static int IkiliArama(int[] dizi, int aranan)
{
    int sol = 0;
    int sag = dizi.Length - 1;

    while (sol <= sag)
    {
        int orta = (sol + sag) / 2;

        if (dizi[orta] == aranan)
            return orta; // Bulundu

        if (dizi[orta] < aranan)
            sol = orta + 1; // Sağ yarıya geç
        else
            sag = orta - 1; // Sol yarıya geç
    }

    return -1; // Aranan bulunamadı
}
```

**Not:** Çok büyük dizilerde taşmayı önlemek için `orta = sol + (sag - sol) / 2` de kullanılabilir.

---

## JUMP SEARCH (Atlamalı Arama)

Dizi **sıralı** olmalıdır. **√n** adım aralıklarla ilerlenir; uygun blok bulununca blok içinde **doğrusal arama** yapılır.

**Neden?**

- Her elemana tek tek bakmak (linear) yavaştır.
- Ortayı bölmek (binary) bazı donanım senaryolarında daha zordur.
- Belirli aralıklarla “zıplayıp” sonra küçük blokta lineer arama yapılır.

**Örnek:** 100 eleman → adım = √100 = **10**. Her zıplamada `dizi[i] < aranan` mı? Uygun blokta geriye dönülüp lineer arama.

**Karmaşıklık:** Ortalama ve en kötü **O(√n)**.

```csharp
using System;

static int AtlamaliArama(int[] dizi, int aranan)
{
    int n = dizi.Length;
    if (n == 0) return -1;

    int adim = (int)Math.Floor(Math.Sqrt(n)); // √n
    int onceki = 0;

    // Arananı geçen bloğu bul; dizi sınırı için Math.Min
    while (onceki < n && dizi[Math.Min(adim, n) - 1] < aranan)
    {
        onceki = adim;
        adim += (int)Math.Floor(Math.Sqrt(n));
        if (onceki >= n)
            return -1;
    }

    for (int i = onceki; i < Math.Min(adim, n); i++)
    {
        if (dizi[i] == aranan)
            return i; // Bulundu
    }

    return -1; // Bulunamadı
}
```

---

## EXPONENTIAL SEARCH (Üssel Arama)

Çok büyük **sıralı** dizilerde kullanılır. **2, 4, 8, 16…** gibi üssel adımlarla aralık bulunur; sonra o aralıkta **ikili arama** yapılır.

Aranan değer dizinin **başlarına** yakınsa özellikle hızlıdır.

```csharp
static int UsselArama(int[] dizi, int aranan)
{
    if (dizi.Length == 0)
        return -1;

    if (dizi[0] == aranan)
        return 0;

    int i = 1;
    while (i < dizi.Length && dizi[i] <= aranan)
        i *= 2; // 2, 4, 8, 16... sıçrayarak ilerle

    return YardimciBinaryArama(dizi, i / 2, Math.Min(i, dizi.Length - 1), aranan);
}

static int YardimciBinaryArama(int[] dizi, int sol, int sag, int aranan)
{
    while (sol <= sag)
    {
        int orta = (sol + sag) / 2;

        if (dizi[orta] == aranan)
            return orta; // Bulundu

        if (dizi[orta] < aranan)
            sol = orta + 1;
        else
            sag = orta - 1;
    }

    return -1; // Bulunamadı
}
```

---

## Karşılaştırma tablosu

| Algoritma | Zaman karmaşıklığı | Sıralı dizi gerekir mi? | Ne zaman kullanılır? |
|-----------|-------------------|-------------------------|----------------------|
| Linear Search | O(n) | Hayır | Sıralı değilse veya çok küçük dizide |
| Binary Search | O(log n) | Evet | Sıralı veride hızlı arama |
| Jump Search | O(√n) | Evet | Sıralı veride blok blok tarama |
| Exponential Search | O(log i)* | Evet | Çok büyük sıralı dizide; aranan başlardaysa |

\* **i** = aranan elemanın (veya ondan büyük ilk elemanın) indeksi.

---

## GRAFLAR — Dijkstra

**Amaç:** Başlangıç düğümünden (şehir) diğer tüm düğümlere **en kısa yol** mesafelerini bulmak.

**Negatif kenar yok** varsayılır (negatif ağırlık için Bellman-Ford → `Lab_Recursive_Bitwise_Tatli_BellmanFord.md`).

### Veri yapıları

- **`graf[N, N]`:** Komşuluk matrisi; `graf[i, j] = 0` → doğrudan yol yok (bu örnekte).
- **`mesafe[]`:** Başlangıçtan her düğüme en kısa mesafe.
- **`ziyaretEdildi[]`:** İşlenmiş (kesinleşmiş) düğümler.

**Başlangıç:** Tüm mesafeler `int.MaxValue` (sonsuz), başlangıç düğümü **0**.

### Algoritma özeti

1. **N − 1** tur: Henüz ziyaret edilmemiş, mesafesi en küçük düğümü seç.
2. Seçilen düğümü ziyaret edildi işaretle.
3. Komşuları için: `mesafe[secilen] + graf[secilen, j] < mesafe[j]` ise güncelle (relax).
4. Son turda son düğüm de dolaylı olarak belirlenmiş olur.

**Örnek yorum:** 0 → 2 doğrudan 10; 0 → 4 → 2 yolu 0+10=10 ile aynı olabilir; algoritma en kısa olanı tutar.

---

### Örnek 1 — 0 numaralı şehirden tüm mesafeler

**5 şehir** komşuluk matrisi:

```
       0    1    2    3    4
0      0    0   10    0   10
1      0    0    0    0    0
2     10    0    0   20   60
3     30    0   20    0   10
4     10    0   60   10    0
```

```csharp
using System;

class Program
{
    static void Main()
    {
        int N = 5;

        int[,] graf = {
            //  0   1   2   3   4
            {  0,  0, 10,  0, 10 }, // 0. şehirden
            {  0,  0,  0,  0,  0 }, // 1. şehirden (bağlantı yok)
            { 10,  0,  0, 20, 60 }, // 2. şehirden
            { 30,  0, 20,  0, 10 }, // 3. şehirden
            { 10,  0, 60, 10,  0 }  // 4. şehirden
        };

        int[] mesafe = new int[N];
        bool[] ziyaretEdildi = new bool[N];

        for (int i = 0; i < N; i++)
        {
            mesafe[i] = int.MaxValue;
            ziyaretEdildi[i] = false;
        }

        mesafe[0] = 0; // Başlangıç: 0 numaralı şehir

        for (int sayac = 0; sayac < N - 1; sayac++)
        {
            int enKisa = int.MaxValue;
            int secilen = -1;

            for (int i = 0; i < N; i++)
            {
                if (!ziyaretEdildi[i] && mesafe[i] < enKisa)
                {
                    enKisa = mesafe[i];
                    secilen = i;
                }
            }

            if (secilen == -1)
                break;

            ziyaretEdildi[secilen] = true;

            for (int j = 0; j < N; j++)
            {
                if (graf[secilen, j] > 0 && !ziyaretEdildi[j])
                {
                    int yeniMesafe = mesafe[secilen] + graf[secilen, j];
                    if (yeniMesafe < mesafe[j])
                        mesafe[j] = yeniMesafe;
                }
            }
        }

        Console.WriteLine("0 numaralı şehirden diğerlerine en kısa mesafeler:");
        for (int i = 0; i < N; i++)
            Console.WriteLine("Şehir {0} -- Mesafe: {1}", i, mesafe[i]);
    }
}
```

---

### Örnek 2 — 4 numaralı şehirden 2 numaralı şehre en kısa yol

Bu örnekte **graf matrisi farklıdır** (özellikle 2↔3, 2↔4, 3↔4 kenarları). Başlangıç **4**, hedef mesafe **2** için yazdırılır.

```
       0    1    2    3    4
0      0    0   10    0   10
1      0    0    0    0    0
2     10    0    0   20    0
3      0    0   20    0   60
4     10    0    0   10    0
```

```csharp
using System;

class Program
{
    static void Main()
    {
        int N = 5;

        int[,] graf = {
            //  0   1   2   3   4
            {  0,  0, 10,  0, 10 }, // 0. şehir
            {  0,  0,  0,  0,  0 }, // 1. şehir
            { 10,  0,  0, 20,  0 }, // 2. şehir
            {  0,  0, 20,  0, 60 }, // 3. şehir
            { 10,  0,  0, 10,  0 }  // 4. şehir
        };

        int[] mesafe = new int[N];
        bool[] ziyaretEdildi = new bool[N];

        for (int i = 0; i < N; i++)
        {
            mesafe[i] = int.MaxValue;
            ziyaretEdildi[i] = false;
        }

        mesafe[4] = 0; // Başlangıç: 4 numaralı şehir

        for (int sayac = 0; sayac < N - 1; sayac++)
        {
            int enKisa = int.MaxValue;
            int secilen = -1;

            for (int i = 0; i < N; i++)
            {
                if (!ziyaretEdildi[i] && mesafe[i] < enKisa)
                {
                    enKisa = mesafe[i];
                    secilen = i;
                }
            }

            if (secilen == -1)
                break;

            ziyaretEdildi[secilen] = true;

            for (int j = 0; j < N; j++)
            {
                if (graf[secilen, j] > 0 && !ziyaretEdildi[j])
                {
                    int yeniMesafe = mesafe[secilen] + graf[secilen, j];
                    if (yeniMesafe < mesafe[j])
                        mesafe[j] = yeniMesafe;
                }
            }
        }

        if (mesafe[2] == int.MaxValue)
            Console.WriteLine("4 numaralı şehirden 2 numaralı şehre yol bulunamadı.");
        else
            Console.WriteLine("4 numaralı şehirden 2 numaralı şehre en kısa mesafe: " + mesafe[2]);
    }
}
```

**Bu graf ile:** 4 → 0 (10) → 2 (10) toplam **20**; doğrudan 4 → 2 yok (`graf[4,2]=0`). Sonuç çalıştırınca **20** görülür.

---

## Dijkstra — kısa notlar

| Konu | Açıklama |
|------|----------|
| Zaman (bu kod) | O(V²) — her turda tüm düğümler taranır |
| Zaman (min-heap) | O((V + E) log V) |
| Negatif kenar | Dijkstra güvenilir değil → Bellman-Ford |
| `secilen == -1` | Ulaşılamayan bileşen; döngü kırılır |

İlgili lab: negatif kenar ve döngü için `Lab_Recursive_Bitwise_Tatli_BellmanFord.md` içindeki **Bellman-Ford** bölümü.
