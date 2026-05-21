# Algoritmalar Laboratuvarı — Konu Bazlı Örnek Sorular ve Çözümleri

**Greedy • Brute Force • Recursive • Memoization • Dynamic Programming • Graf**

Bu dosya lab formatında beş konu ve çalışır C# örneklerini içerir. `Program.cs` ile çakışmaması için kodlar ayrı kopyalanıp denenebilir.

---

## 1) GREEDY (Aç Gözlü) — Para Üstü Problemi

### Problem tanımı

Bir kasiyersin. Müşteriye **N TL** para üstü vermen gerekiyor. Elinde sınırsız sayıda **50, 20, 10, 5, 1** TL banknotlar var.

**Hedef:** En az sayıda banknot kullanarak para üstünü ver.

**Örnek:** N = 87 TL → 50 + 20 + 10 + 5 + 1 + 1 = **6 banknot**

### Greedy mantığı

- Her adımda **en büyük** banknotu seçeriz (yerel olarak en iyi).
- Sığabildiği kadar o banknottan kullanırız, sonra bir alt banknota geçeriz.
- Türk Lirası banknot sistemi **katlı (canonical)** olduğu için greedy bu sistemde doğru sonucu garanti eder.

### Adım adım çözüm (N = 87)

| Banknot | Kalan tutar | Kaç adet? | Yeni kalan |
|---------|-------------|-----------|------------|
| 50      | 87          | 1         | 37         |
| 20      | 37          | 1         | 17         |
| 10      | 17          | 1         | 7          |
| 5       | 7           | 1         | 2          |
| 1       | 2           | 2         | 0          |

**Sonuç:** Toplam **6** banknot.

### C# çözümü

```csharp
using System;

class Program
{
    static void Main()
    {
        // Banknotları büyükten küçüğe sıralı tanımlıyoruz (greedy için şart)
        int[] banknotlar = { 50, 20, 10, 5, 1 };
        int paraUstu = 87;
        int toplamAdet = 0;

        for (int i = 0; i < banknotlar.Length; i++)
        {
            int adet = paraUstu / banknotlar[i];
            paraUstu = paraUstu % banknotlar[i];
            toplamAdet += adet;

            if (adet > 0)
                Console.WriteLine(banknotlar[i] + " TL: " + adet + " adet");
        }

        Console.WriteLine("Toplam banknot sayisi: " + toplamAdet);
    }
}
```

### Açıklama

- Diziyi **büyükten küçüğe** sıralı yazıyoruz; her döngüde o an için en iyi banknotu mümkün olduğunca çok kullanıyoruz.
- `paraUstu / banknotlar[i]` → kaç tane sığar?
- `paraUstu % banknotlar[i]` → kullanımdan sonra geriye ne kalır?

### Greedy ne zaman yanılır?

Örneğin banknotlar `{1, 3, 4}` ve N = 6:

- Greedy → 4 + 1 + 1 = **3** banknot  
- Optimum → 3 + 3 = **2** banknot  

Greedy **her zaman** optimum vermez; problemin yapısı uygun olmalıdır.

---

## 2) BRUTE FORCE (Kaba Kuvvet) — Hediye Sepeti Seçimi

### Problem tanımı

Bir kafede 4 farklı tatlı var. Cebinde **12 TL** var. Bütçeyi aşmadan toplam **mutluluğu** maksimize et. Her tatlıdan en fazla **1** tane (0/1 seçim).

| Tatlı     | Fiyat (TL) | Mutluluk |
|-----------|------------|----------|
| Baklava   | 7          | 9        |
| Sütlaç    | 4          | 5        |
| Künefe    | 5          | 7        |
| Kazandibi | 3          | 4        |

### Brute-force mantığı

- 4 tatlı için **2⁴ = 16** kombinasyon (her tatlı 0 veya 1).
- İç içe döngülerle **tüm** kombinasyonlar denenir.
- Bütçe aşılmıyorsa mutluluk hesaplanır; en iyisi saklanır.

### C# çözümü

```csharp
using System;

class Program
{
    static void Main()
    {
        int butce = 12;
        int maxMutluluk = 0;
        int sB = 0, sS = 0, sK = 0, sZ = 0;

        for (int b = 0; b <= 1; b++)   // Baklava
        for (int s = 0; s <= 1; s++)   // Sutlac
        for (int k = 0; k <= 1; k++)   // Kunefe
        for (int z = 0; z <= 1; z++)   // Kazandibi
        {
            int fiyat    = b * 7 + s * 4 + k * 5 + z * 3;
            int mutluluk = b * 9 + s * 5 + k * 7 + z * 4;

            if (fiyat <= butce && mutluluk > maxMutluluk)
            {
                maxMutluluk = mutluluk;
                sB = b; sS = s; sK = k; sZ = z;
            }
        }

        Console.WriteLine("En iyi secim:");
        if (sB == 1) Console.WriteLine("- Baklava");
        if (sS == 1) Console.WriteLine("- Sutlac");
        if (sK == 1) Console.WriteLine("- Kunefe");
        if (sZ == 1) Console.WriteLine("- Kazandibi");
        Console.WriteLine("Toplam mutluluk: " + maxMutluluk);
    }
}
```

### Elle doğrulama

| Kombinasyon (b,s,k,z) | Seçim                         | Fiyat | Mutluluk | Geçerli? |
|------------------------|-------------------------------|-------|----------|----------|
| 1011                   | Baklava + Künefe + Kazandibi  | 15    | —        | Bütçe aşar |
| 1010                   | Baklava + Künefe              | 12    | 16       | Evet |
| 1101                   | Baklava + Sütlaç + Kazandibi | 14    | —        | Bütçe aşar |
| 0111                   | Sütlaç + Künefe + Kazandibi   | 12    | 16       | Evet |

**En iyi sonuç:** **16** mutluluk (ör. `1010` veya `0111`).

### Tatlı sayısı 30 olsaydı?

**2³⁰ ≈ 1 milyar** kombinasyon → brute-force çok yavaşlar. Bu durumda **bitmask** (küçük n, pratikte genelde ≤20–24) veya **DP (0/1 knapsack)** kullanılır. İlgili not: `Knapsack_0_1_IP_Rehber_ve_Sorular.md`, `SubsetSum_DP_Bitwise_Rehber.md`.

---

## 3) RECURSIVE (Özyineleme) — Fibonacci

### Problem tanımı

Fibonacci: 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, …

- F(0) = 0  
- F(1) = 1  
- F(n) = F(n−1) + F(n−2),  n ≥ 2  

**Hedef:** n'inci Fibonacci sayısını özyineleme ile bul.

### Özyineleme mantığı

- Fonksiyonun **kendi kendini** çağırması.
- **Temel durum (base case)** zorunlu; yoksa sonsuz çağrı.
- Burada: `n == 0` → 0, `n == 1` → 1.
- Diğer durumlarda problem `n−1` ve `n−2` ile küçültülür.

### Çağrı ağacı (n = 5) — özet

```
                    F(5)
                   /    \
                F(4)    F(3)
               /  \    /  \
            F(3) F(2) F(2) F(1)
            ...
```

F(3) ve F(2) **birden fazla** kez hesaplanır → saf recursive **yavaş** → çözüm: **memoization**.

### C# çözümü

```csharp
using System;

class Program
{
    static int Fibonacci(int n)
    {
        if (n == 0)
            return 0;

        if (n == 1)
            return 1;

        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    static void Main()
    {
        int n = 10;
        Console.WriteLine("F(" + n + ") = " + Fibonacci(n));
    }
}
```

### Açıklama

`Fibonacci(5)` → `Fibonacci(4)` + `Fibonacci(3)`; tabana inene kadar devam eder; değerler yukarı toplanır.

**Karmaşıklık:** **O(2ⁿ)** — n = 40 için bile çok büyük çağrı sayısı. **Çözüm:** Memoization.

---

## 4) MEMOIZATION (Hafızalama) — Fibonacci'yi hızlandırma

### Problem tanımı

Aynı Fibonacci; tekrar tekrar hesaplama yapılmaz. Örneğin F(3) bir kez hesaplanır, sonuç **saklanır**.

### Memoization mantığı

- Dizi veya sözlük: giriş → sonuç.
- Çağrıda önce **önbellekte var mı** bakılır.
- Varsa doğrudan döndürülür.
- Yoksa hesaplanır ve **kaydedilir**.
- Her alt problem **en fazla bir kez** hesaplanır.

### Karmaşıklık karşılaştırması

| Yöntem        | Zaman   | Bellek        |
|---------------|---------|---------------|
| Saf recursive | O(2ⁿ)   | O(n) — stack  |
| Memoization   | O(n)    | O(n) — dizi + stack |

### C# çözümü

```csharp
using System;

class Program
{
    static int[] hafiza;

    static int Fibonacci(int n)
    {
        if (n <= 1)
            return n;

        if (hafiza[n] != -1)
            return hafiza[n];

        int sonuc = Fibonacci(n - 1) + Fibonacci(n - 2);
        hafiza[n] = sonuc;

        return sonuc;
    }

    static void Main()
    {
        int n = 50;
        hafiza = new int[n + 1];
        for (int i = 0; i <= n; i++)
            hafiza[i] = -1;

        Console.WriteLine("F(" + n + ") = " + Fibonacci(n));
    }
}
```

### Açıklama

- Saf recursive ile F(50) pratikte çok uzun sürer; memoization ile **milisaniyeler** içinde biter.
- Dizi **−1** ile başlar: F(0) = 0 geçerli bir sonuç olduğu için “henüz hesaplanmadı” işareti olarak 0 kullanılamaz.
- **Memoization = Recursive + önbellek** → **top-down DP** olarak da bilinir.

---

## 5) DYNAMIC PROGRAMMING — Yazı Tura Merdiveni (Min. Maliyet)

### Problem tanımı

Her basamakta bir **maliyet** var. Her adımda **1 veya 2** basamak çıkılabilir.

- Başlangıç: **0.** veya **1.** basamaktan **ücretsiz** (maliyet ödenmez).
- **Hedef:** n. basamağa (tepe) ulaşırken ödenen **minimum** toplam maliyet.

**Örnek:** `cost = [10, 15, 20]`

- 0'dan: 10 öde → 2 atla → tepe. Toplam: **10**
- 1'den: 15 öde → 1 atla → tepe. Toplam: **15**

**Minimum:** **10**

### DP mantığı

- `dp[i]` = **i.** basamağa ulaşmak için ödenen minimum toplam maliyet.
- **Bottom-up** doldurma.
- `dp[0] = 0`, `dp[1] = 0` (ücretsiz başlangıç)
- `dp[i] = min(dp[i−1] + cost[i−1], dp[i−2] + cost[i−2])`

1 önceki veya 2 önceki basamaktan gelirken, **atlanan** basamağın maliyeti ödenir.

### Örnek tablo

`cost = [1, 100, 1, 1, 1, 100, 1, 1, 100, 1]`

| i        | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9  |
|----------|---|---|---|---|---|---|---|---|---|---|
| cost[i]  | 1 |100| 1 | 1 | 1 |100| 1 | 1 |100| 1  |
| dp[i]    | 0 | 0 | 1 | 2 | 2 | 3 | 3 | 4 | 4 | 5  |

**Tepe (i = 10):**  
`min(dp[9] + cost[9], dp[8] + cost[8])` = `min(5+1, 4+100)` = **6**

### C# çözümü

```csharp
using System;

class Program
{
    static int MinMaliyet(int[] cost)
    {
        int n = cost.Length;
        int[] dp = new int[n + 1];

        dp[0] = 0;
        dp[1] = 0;

        for (int i = 2; i <= n; i++)
        {
            int birOnceden = dp[i - 1] + cost[i - 1];
            int ikiOnceden = dp[i - 2] + cost[i - 2];
            dp[i] = Math.Min(birOnceden, ikiOnceden);
        }

        return dp[n];
    }

    static void Main()
    {
        int[] cost = { 1, 100, 1, 1, 1, 100, 1, 1, 100, 1 };
        Console.WriteLine("Min. maliyet: " + MinMaliyet(cost));
    }
}
```

### Açıklama

- **Bottom-up DP:** küçük alt problemlerden tepeye.
- Her `dp[i]`, yalnızca `dp[i−1]` ve `dp[i−2]` kullanır (Fibonacci benzeri yapı; burada **min** alınır).
- **Zaman:** O(n). **Bellek:** O(n); yalnızca son iki `dp` değeri tutularak **O(1)** belleğe indirilebilir.

---

## İlgili dosyalar

| Konu | Dosya |
|------|--------|
| 0/1 knapsack, bitmask, IP | `Knapsack_0_1_IP_Rehber_ve_Sorular.md` |
| DP rod / coin / grid | `DP_Rod_Coin_Grid_Sorular.md` |
| Lab (recursive, BF, tatlı) | `Lab_Recursive_Bitwise_Tatli_BellmanFord.md` |
| Arama, Dijkstra | `Arama_Algoritmalari_ve_Dijkstra.md` |
