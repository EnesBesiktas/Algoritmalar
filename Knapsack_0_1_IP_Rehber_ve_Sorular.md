# 0/1 Knapsack, Tamsayılı Programlama ve Brute-Force Örnekleri

---

## 0/1 Knapsack Problemi

### Problem tanımı

**n** eşya var. Her eşyanın ağırlığı `w[i]` ve değeri `v[i]` var. **W** kapasiteli bir çanta var. Eşyalar bölünemez (**0/1**: ya tamamen alırsın ya hiç almazsın).

**Hedef:** Toplam ağırlık **W**’yi aşmadan maksimum değeri elde etmek.

### Örnek tablo

| Eşya    | Ağırlık (kg) | Değer | Seçildi? |
|---------|--------------|-------|----------|
| Laptop  | 3            | 4     | Evet     |
| Telefon | 1            | 3     | Evet     |
| Kitap   | 2            | 2     | Hayır    |
| Kamera  | 4            | 5     | Hayır    |

**Kapasite:** 6 kg → **Optimal değer:** 9 → **Seçilen:** Laptop + Telefon.

### DP tablosu (W = 6)

Satırlar: öğeler sırayla ekleniyor. Sütunlar: kapasite 0…6.

| i / w → | 0 | 1 | 2 | 3 | 4 | 5 | 6 |
|---------|---|---|---|---|---|---|---|
| ∅ (boş) | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| Laptop (3, 4) | 0 | 0 | 0 | 4 | 4 | 4 | 4 |
| Telefon (1, 3) | 0 | 3 | 3 | 4 | 7 | 7 | 7 |
| Kitap (2, 2) | 0 | 3 | 3 | 5 | 7 | 7 | 9 |
| Kamera (4, 5) | 0 | 3 | 3 | 5 | 7 | 8 | 9 |

**Optimal cevap:** `dp[4][6] = 9` (son satır, son sütun).

**Traceback (hangi eşya alındı?):** `dp[i][w] != dp[i-1][w]` ise **i**. eşya seçilmiştir; aksi halde seçilmemiştir ve bir üst satıra (`i-1`, `w`) gidersin. Seçildiyse `w -= w[i]` ile geri gider.

### Özyinelemeli C# (kavramsal)

```csharp
static int CantaMaksimumDeger(int kapasite, int[] agirliklar, int[] degerler, int n)
{
    if (n == 0 || kapasite == 0)
        return 0;

    if (agirliklar[n - 1] > kapasite)
        return CantaMaksimumDeger(kapasite, agirliklar, degerler, n - 1);

    return Math.Max(
        degerler[n - 1] + CantaMaksimumDeger(kapasite - agirliklar[n - 1], agirliklar, degerler, n - 1),
        CantaMaksimumDeger(kapasite, agirliklar, degerler, n - 1));
}
```

**Klasik örnek (kitaptan):** `degerler = { 60, 100, 120 }`, `agirliklar = { 10, 20, 30 }`, `kapasite = 50` → cevap **220**.

---

## Integer Programming (IP) vs Linear Programming (LP)

- **Doğrusal programlama (LP):** Değişkenler genelde **reel** olabilir (ör. “3.2 birim üret”).
- **Tamsayılı programlama (IP / ILP):** Bazı veya tüm değişkenler **tam sayı** olmak zorundadır (ör. “3 veya 4 sandalye”, kesir üretim yok).

### Nerelerde kullanılır?

- Üretim planlaması, kaynak dağılımı, fabrika optimizasyonu  
- Vardiya çizelgeleme, taşıma / rota seçimleri  
- **Knapsack** ve benzeri seçim problemleri  
- “N makineden k tanesini seç” gibi **ayık seçim** modelleri  

Küçük boyutlarda **tüm tamsayı çiftlerini / maskeleri denemek** (brute-force) mümkün olabilir; büyüyünce **DP**, **dal-sınır (branch & bound)**, **LP gevşetmesi + yuvarlama** gibi yöntemler devreye girer.

---

## Brute-force örnekleri

### SORU 0 — Sandalye ve masa (2 değişken, tamsayı)

**Kaynaklar:** 24 ahşap, 16 çivi.

- Sandalye: 4 ahşap, 2 çivi → **20 TL** kar  
- Masa: 6 ahşap, 4 çivi → **30 TL** kar  

**Amaç:** `20·S + 30·M` maksimum.

**Kısıtlar:**

- `4·S + 6·M ≤ 24`  
- `2·S + 4·M ≤ 16`  
- `S, M ∈ ℕ` (genelde üretim sorularında 0 dahil doğal sayı)

**Fikir:** Makul üst sınırlar seçip iç içe `for` ile tüm `(S, M)` çiftlerini dene; kısıtları sağlayanlardan en büyük karı seç.

*(Örnek uygun çözümlerden biri: toplam kar **120** TL’ye çıkabilir; örneğin `S = 6, M = 0` veya `S = 3, M = 2` gibi farklı optimum noktalar olabilir — kısıtları elle veya kodla doğrula.)*

---

### SORU 1 — Gözlük ve saat

Bir fabrika **gözlük (G)** ve **saat (S)** üretiyor.

| Ürün   | Cam | Metal | Kazanç |
|--------|-----|-------|--------|
| Gözlük | 2   | 1     | 15 TL  |
| Saat   | 3   | 2     | 25 TL  |

**Toplam:** 18 cam, 12 metal.

**Amaç:** `15·G + 25·S` maksimum.

**Kısıtlar:**

- `2·G + 3·S ≤ 18` (cam)  
- `1·G + 2·S ≤ 12` (metal)  
- `G, S ≥ 0` tam sayı  

**Üst sınır (döngü için):**  
Gözlük: `min(18/2, 12/1) = 9`. Saat: `min(18/3, 12/2) = 6`.

#### Çözüm (brute-force, doğru)

```csharp
using System;

class Program
{
    static void Main()
    {
        int maxKar = 0;
        int enIyiGozluk = 0, enIyiSaat = 0;

        for (int g = 0; g <= 9; g++)
        {
            for (int s = 0; s <= 6; s++)
            {
                int cam = g * 2 + s * 3;
                int metal = g * 1 + s * 2;

                if (cam <= 18 && metal <= 12)
                {
                    int kar = g * 15 + s * 25;
                    if (kar > maxKar)
                    {
                        maxKar = kar;
                        enIyiGozluk = g;
                        enIyiSaat = s;
                    }
                }
            }
        }

        Console.WriteLine("En iyi üretim planı:");
        Console.WriteLine("Gözlük adedi: " + enIyiGozluk);
        Console.WriteLine("Saat adedi: " + enIyiSaat);
        Console.WriteLine("Toplam Kar: " + maxKar + " TL");
    }
}
```

Her `(g, s)` için kaynak yeterli mi diye bakılır; en iyi kar saklanır.

---

### SORU 2 — Hediye kutusu seçimi (0/1 knapsack, 3 kutu)

**Kutular:** A: 3 kg, 9 TL — B: 4 kg, 13 TL — C: 2 kg, 6 TL.  
**Kapasite:** 9 kg. Her kutudan en fazla **1** adet (0 veya 1).

**Model:** `a, b, c ∈ {0,1}` → `2³ = 8` kombinasyon.

#### İç içe döngü ile (düzeltilmiş ağırlık/değer ve `max` karşılaştırması)

Yaygın hata: ağırlık/değer formüllerinin problemdeki sayılarla uyuşmaması veya `if` içinde `>` kontrolü olmadan `maxDeger` güncellemek.

```csharp
using System;

class Program
{
    static void Main()
    {
        int maxDeger = 0;
        int secilenA = 0, secilenB = 0, secilenC = 0;

        for (int a = 0; a <= 1; a++)
        for (int b = 0; b <= 1; b++)
        for (int c = 0; c <= 1; c++)
        {
            int agirlik = a * 3 + b * 4 + c * 2; // A=3, B=4, C=2
            int deger = a * 9 + b * 13 + c * 6;

            if (agirlik <= 9)
            {
                if (deger > maxDeger)
                {
                    maxDeger = deger;
                    secilenA = a; secilenB = b; secilenC = c;
                }
            }
        }

        Console.WriteLine("En iyi kutu kombinasyonu:");
        if (secilenA == 1) Console.WriteLine("Kutu A");
        if (secilenB == 1) Console.WriteLine("Kutu B");
        if (secilenC == 1) Console.WriteLine("Kutu C");
        Console.WriteLine("Toplam Değer: " + maxDeger + " TL");
    }
}
```

#### “3 yerine 30 kutu olsaydı?”

- **3 iç içe `for`:** 30 kutu için **30 iç içe döngü** yazmak pratik değil.  
- **Kombinasyon sayısı:** Her biri 0/1 ise `2^30` (yaklaşık **1 milyar**) durum — brute-force çoğu ortamda çok yavaş / uygunsuz.  
- **Bitmask:** `n` kutu için `mask` ∈ `[0, 2^n)`. Pratikte `int` ile güvenli üst sınır genelde **~20–24** civarıdır (`1 << n` taşmasın diye `n ≤ 30` için `long` mask veya DP tercih edilir).  

#### Bitmask ile (düzgün döngü sınırı ve çıktı)

```csharp
using System;

class Program
{
    static void Main()
    {
        int[] agirliklar = { 3, 4, 2 };
        int[] degerler = { 9, 13, 6 };
        string[] isimler = { "A", "B", "C" };
        int maxAgirlik = 9;
        int n = agirliklar.Length;

        int maxDeger = 0;
        string secilen = "";

        for (int mask = 0; mask < (1 << n); mask++)
        {
            int w = 0, v = 0;
            var sb = new System.Text.StringBuilder();

            for (int i = 0; i < n; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    w += agirliklar[i];
                    v += degerler[i];
                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(isimler[i]);
                }
            }

            if (w <= maxAgirlik && v > maxDeger)
            {
                maxDeger = v;
                secilen = sb.ToString();
            }
        }

        Console.WriteLine("En iyi kutu kombinasyonu: " + secilen);
        Console.WriteLine("Toplam değer: " + maxDeger + " TL");
    }
}
```

**Mask tablosu (n = 3, bit sırası i = 0→A, 1→B, 2→C):**

| mask | binary | Anlamı (bu eşlemede) |
|------|--------|----------------------|
| 0    | 000    | Hiçbiri              |
| 1    | 001    | Sadece A             |
| 2    | 010    | Sadece B             |
| 3    | 011    | A + B                |
| 4    | 100    | Sadece C             |
| 5    | 101    | A + C                |
| 6    | 110    | B + C                |
| 7    | 111    | A + B + C            |

Bit–isim eşlemesini kodda değiştirirsen tablodaki harfler yer değiştirir; önemli olan döngüdeki `i` ile `agirliklar[i]`, `degerler[i]`, `isimler[i]` sırasının **aynı kalmasıdır**.

---

### SORU 3 — Lale ve gül

**Lale:** 2 su, 3 gübre → **12 TL** kar.  
**Gül:** 3 su, 2 gübre → **16 TL** kar.  
**Toplam:** 18 su, 18 gübre. Tam sayı adet.

**Amaç:** `12·L + 16·G` maksimum.

**Kısıtlar:**

- `2·L + 3·G ≤ 18`  
- `3·L + 2·G ≤ 18`  
- `L, G ≥ 0` tam sayı  

**Üst sınır:** `maxLale = min(18/2, 18/3) = 6`, `maxGul = min(18/3, 18/2) = 6` — döngüyü gereksiz büyütmemek için; yine de **mutlaka kısıt kontrolü** yapılmalıdır.

#### Çözüm (düzeltilmiş)

```csharp
using System;

class Program
{
    static void Main()
    {
        int maxKar = 0, enIyiLale = 0, enIyiGul = 0;
        int maxLale = Math.Min(18 / 2, 18 / 3);
        int maxGul = Math.Min(18 / 3, 18 / 2);

        for (int l = 0; l <= maxLale; l++)
        for (int g = 0; g <= maxGul; g++)
        {
            int su = l * 2 + g * 3;
            int gubre = l * 3 + g * 2;
            if (su > 18 || gubre > 18) continue;

            int kar = l * 12 + g * 16;
            if (kar > maxKar)
            {
                maxKar = kar;
                enIyiLale = l;
                enIyiGul = g;
            }
        }

        Console.WriteLine("En karlı üretim:");
        Console.WriteLine("Lale: " + enIyiLale);
        Console.WriteLine("Gül: " + enIyiGul);
        Console.WriteLine("Toplam kar: " + maxKar + " TL");
    }
}
```

**Tam sayı / kesir:** `int` ile adet tuttuğun sürece “yarım çiçek” üretimi zaten modellenmez; yine de kısıtları açıkça kontrol etmek en güvenlisidir.

---

### SORU 4 — Kargo alanı (sınırsız stok, tam sayı adet)

**Koli:** Küçük 3 desi / 6 TL, Orta 5 / 10, Büyük 7 / 13. **Kapasite:** 50 desi. Aynı koliden istenildiği kadar — **sınırsız çoklu seçim** (unbounded knapsack benzeri).

**Dikkat:** “Önce küçükleri doldur” gibi basit bir **greedy**, her zaman optimum vermez. Bu sayılarda küçük ve orta **kar/hacim = 2**; büyük daha düşük. Yine de **50 = 10×5** ile sadece **orta** koli kullanmak **100 TL** verirken, **16×3 = 48** desi küçük ile **96 TL** gibi greedy dolgu daha kötü kalabilir.

1. **Optimumu garanti istiyorsan:** unbounded knapsack DP (aşağıda).  
2. **Greedy:** hızlıdır; problem yapısı özel değilse **kanıtlı** kullanma.

#### Greedy örnek (eğitim amaçlı — optimum garantisi yok)

```csharp
// UYARI: Bu yaklaşım her kapasite / her koli seti için optimum değildir.
```

#### Sınırsız stok için DP (önerilen)

```csharp
using System;

class Program
{
    static void Main()
    {
        int kapasite = 50;
        int[] hacim = { 3, 5, 7 };
        int[] kar = { 6, 10, 13 };

        int[] dp = new int[kapasite + 1];

        for (int i = 0; i < hacim.Length; i++)
        {
            for (int j = hacim[i]; j <= kapasite; j++)
                dp[j] = Math.Max(dp[j], dp[j - hacim[i]] + kar[i]);
        }

        Console.WriteLine("Maksimum kar (sınırsız stok, DP): " + dp[kapasite] + " TL");
    }
}
```

**Stok sınırı gelirse:** Aynı koliden sınırlı adet → **bounded knapsack**; her tip için stok kadar katman veya “çoklu 0/1” gibi DP. Aşağıdaki desen, her kopyayı **ayrı bir 0/1 eşya** gibi düşünüp iç döngüyü **geriye** (`j: kapasite → hacim`) tarayarak aynı aşamada iki kez saymayı engeller:

```csharp
using System;

class Program
{
    static void Main()
    {
        int kapasite = 50;
        int[] hacim = { 3, 5, 7 };
        int[] kar = { 6, 10, 13 };
        int[] stok = { 5, 4, 2 };

        int[] dp = new int[kapasite + 1];

        for (int i = 0; i < hacim.Length; i++)
        {
            for (int s = 0; s < stok[i]; s++)
            {
                for (int j = kapasite; j >= hacim[i]; j--)
                    dp[j] = Math.Max(dp[j], dp[j - hacim[i]] + kar[i]);
            }
        }

        Console.WriteLine("Maksimum kar (stoklu DP): " + dp[kapasite] + " TL");
    }
}
```

#### Neden tek boyut yeterli?

`dp[j]` = **j** hacim için elde edilebilen en iyi kar. Yeni bir koli eklerken “**j** hacimde ya eski gibi kalırım ya da bir koliyi ekleyip `j - hacim`’deki optimuma `kar` eklerim” denklemi, **hangi kolilerin** seçildiğini ayrı tutmadan optimum **değeri** verir. **Hangi koliler** seçildiğini yazdırmak istersen geri izleme (parent dizi / 2 boyutlu tablo) gerekir.

**Geriden `j` döngüsü (bounded):** Aynı kopyayı aynı iç `s` turunda iki kez kullanmayı engellemek için klasik 0/1 güncellemesidir.

---

## Özet

| Konu | Kısa not |
|------|----------|
| 0/1 knapsack | Tablo DP veya özyineleme; traceback ile seçilen öğeler |
| IP vs LP | Tam sayı zorunluluğu birçok gerçek üretim/seçim modelini IP yapar |
| Brute-force | Küçük aralıkta iç içe `for` veya bitmask |
| Bitmask | `n` büyüyünce `2^n` patlar; pratik sınır düşün |
| Greedy | Her zaman optimum değildir; şüphede DP |
| Sınırsız / stoklu | Sınırsız: ileri `j`; stoklu: tip başına kopya + geri `j` |

Bu dosya, projedeki diğer DP notlarıyla (`DP_Rod_Coin_Grid_Sorular.md` vb.) birlikte kullanılmak üzere derlenmiştir.
