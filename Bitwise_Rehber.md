# Bitwise İşlemler: Derinlemesine Rehber (C# Odaklı)

Bu doküman, bitwise (bit düzeyi) işlemleri **temelden ileri seviyeye** kadar anlatır.  
Amaç: Operatörleri ezberlemek değil, mantığını kavrayıp algoritma sorularında rahat kullanmak.

---

## 1) Bit Nedir?

Bilgisayarda en küçük veri birimi **bit**tir:

- `0` veya `1`
- 8 bit = 1 byte

Bir `int` (C#) 32 bittir. Örnek:

- `5` -> binary: `00000000 00000000 00000000 00000101`
- `10` -> binary: `00000000 00000000 00000000 00001010`

Bitwise işlemler, sayıları bu bit dizileri üzerinde işler.

---

## 2) Neden Bitwise Öğrenmeliyim?

- **Performans:** Bazı işlemler çok hızlıdır.
- **Algoritma soruları:** XOR, bit sayma, mask kullanımı sık çıkar.
- **Düşük seviye kontrol:** Bayraklar (flags), izin bitleri, sıkıştırılmış veri.
- **Matematiksel içgörü:** Sayıların ikilik yapısını anlamayı sağlar.

---

## 3) Binary Temel Mantık

Bir binary sayı, 2’nin kuvvetlerinin toplamıdır.

Örnek: `13`  

- binary: `1101`
- değer: `1*8 + 1*4 + 0*2 + 1*1 = 13`

Bit konumları sağdan sola artar:

- en sağ bit: `2^0`
- bir solu: `2^1`
- sonra `2^2`, `2^3`, ...

---

## 4) C# Bitwise Operatörleri

| Operatör | Adı | Ne yapar? |
|---|---|---|
| `&` | AND | İki bit de `1` ise `1`, aksi `0` |
| `|` | OR | Bitlerden biri `1` ise `1` |
| `^` | XOR | Bitler farklıysa `1`, aynıysa `0` |
| `~` | NOT | Bitleri tersler |
| `<<` | Left Shift | Bitleri sola kaydırır |
| `>>` | Right Shift | Bitleri sağa kaydırır |

> Not: C#'ta `>>>` (unsigned right shift) modern sürümlerde desteklenir; klasik kullanımda `>>` ile işaretli kaydırma yapılır.

---

## 5) Doğruluk Tabloları

### AND (`&`)

| a | b | a & b |
|---|---|---|
| 0 | 0 | 0 |
| 0 | 1 | 0 |
| 1 | 0 | 0 |
| 1 | 1 | 1 |

### OR (`|`)

| a | b | a \| b |
|---|---|---|
| 0 | 0 | 0 |
| 0 | 1 | 1 |
| 1 | 0 | 1 |
| 1 | 1 | 1 |

### XOR (`^`)

| a | b | a ^ b |
|---|---|---|
| 0 | 0 | 0 |
| 0 | 1 | 1 |
| 1 | 0 | 1 |
| 1 | 1 | 0 |

---

## 6) Operatörleri Adım Adım Anlama

### 6.1 AND (`&`)

Örnek: `12 & 10`

- `12` -> `1100`
- `10` -> `1010`
- sonuç -> `1000` -> `8`

Kullanım:

- Bir bit açık mı kontrol etme
- Maske ile seçme

### 6.2 OR (`|`)

Örnek: `12 | 10`

- `1100 | 1010 = 1110` -> `14`

Kullanım:

- Bir biti zorla `1` yapmak

### 6.3 XOR (`^`)

Örnek: `12 ^ 10`

- `1100 ^ 1010 = 0110` -> `6`

Kritik özellikler:

- `x ^ x = 0`
- `x ^ 0 = x`
- değişmeli/birleşmeli olduğu için sıra önemli değildir

### 6.4 NOT (`~`)

`~x`, x’in tüm bitlerini ters çevirir.

Örnek (`int` 32 bit):  
`x = 5` -> `000...0101`  
`~x = 111...1010` -> bu sayı `-6`'dır (two's complement sebebiyle).

### 6.5 Shift (`<<`, `>>`)

- `x << k`: bitleri sola kaydırır, düşük bitlere `0` doldurur.
- `x >> k`: işaretli sayıda sağa kaydırır (negatifse soldan `1` gelebilir).

Örnek:

- `5 << 1 = 10`
- `5 << 2 = 20`
- `20 >> 2 = 5`

> Pozitif sayılarda yaklaşık olarak:
> - sola kaydırma: `x * 2^k`
> - sağa kaydırma: `x / 2^k` (tam sayı bölmesi)

---

## 7) Signed Sayılar ve Two's Complement

Negatif sayılar çoğu sistemde **two's complement** ile tutulur.

Kısaca:

1. Sayının pozitif binary'sini al
2. Bitleri tersle
3. 1 ekle

Örnek: `-5`

- `5` -> `000...0101`
- tersle -> `111...1010`
- `+1` -> `111...1011` = `-5`

Sonuç:

- `~5 = -6`
- `~-1 = 0`

Bu yüzden `~` operatörü bazen yeni başlayanlara şaşırtıcı gelir.

---

## 8) Mask (Bit Mask) Kavramı

Mask: Belirli bitleri seçmek, açmak, kapatmak için kullanılan sayıdır.

### 8.1 i’inci biti kontrol et

```csharp
bool acikMi = (x & (1 << i)) != 0;
```

### 8.2 i’inci biti 1 yap

```csharp
x |= (1 << i);
```

### 8.3 i’inci biti 0 yap

```csharp
x &= ~(1 << i);
```

### 8.4 i’inci biti tersle

```csharp
x ^= (1 << i);
```

---

## 9) Algoritma Sorularında Altın Kalıplar

### 9.1 Sayı çift mi?

```csharp
bool ciftMi = (n & 1) == 0;
```

### 9.2 2’nin kuvveti mi?

`n > 0 && (n & (n - 1)) == 0`

Neden?  
2’nin kuvvetlerinde tek bir `1` biti vardır:

- `8` -> `1000`
- `7` -> `0111`
- `1000 & 0111 = 0000`

### 9.3 En sağdaki 1 bitini sil

```csharp
n &= (n - 1);
```

### 9.4 En sağdaki 1 bitini izole et

```csharp
int lowbit = n & -n;
```

### 9.5 Dizide tek kalan sayıyı bul (diğerleri çift tekrar)

```csharp
int ans = 0;
foreach (int v in arr) ans ^= v;
```

---

## 10) Bit Sayısı (Population Count)

Bir sayının binary gösterimindeki `1` sayısını bulmak.

### 10.1 Basit yöntem

Her biti kontrol et (`n >> i` ile).

### 10.2 Brian Kernighan yöntemi (daha iyi)

```csharp
int count = 0;
while (n != 0)
{
    n &= (n - 1); // en sağdaki 1'i siler
    count++;
}
```

Döngü, bit sayısı kadar döner. Çok pratiktir.

---

## 11) C#’ta Dikkat Edilecek Noktalar

- `int` 32 bit signed, `long` 64 bit signed.
- `>>` signed right shift davranışı gösterir.
- Literal yazarken okunabilirlik için `_` kullanılabilir: `0b1010_1100`.
- Binary literal:

```csharp
int x = 0b1011; // 11
```

- Büyük shiftlerde taşma ve işaret etkilerine dikkat et.

---

## 12) Operatör Önceliği Tuzakları

Bitwise operatörlerde parantez kullanmak en güvenli yoldur.

Örnek:

```csharp
if ((x & (1 << i)) != 0)
{
    // ...
}
```

Parantezsiz yazım, yanlış yorumlanmaya çok açıktır.

---

## 13) Sık Yapılan Hatalar

- `&` ile `&&` karıştırmak:
  - `&` bitwise veya bool için non-short-circuit
  - `&&` mantıksal ve short-circuit
- `|` ile `||` karıştırmak.
- `~` sonucunun negatif olabileceğini unutmak.
- `>>` işleminin negatif sayılarda işaret biti taşıdığını göz ardı etmek.
- `XOR swap` tekniğini gereksiz yerde kullanmak (okunabilirlik düşebilir).

---

## 14) Uygulamalı Mini Örnekler

### 14.1 Bayrak (flags) sistemi

```csharp
[Flags]
public enum Permission
{
    None    = 0,
    Read    = 1 << 0, // 1
    Write   = 1 << 1, // 2
    Execute = 1 << 2  // 4
}
```

Kullanım:

```csharp
Permission p = Permission.Read | Permission.Write;
bool canWrite = (p & Permission.Write) != 0;
```

### 14.2 Alt kümeleri bitmask ile temsil etme

`n` elemanlı kümede her alt küme `0..(1<<n)-1` ile temsil edilir.

- `mask` içindeki `i` biti açıksa `i` elemanı alt kümede var.

Bu yaklaşım kombinasyon/sırt çantası benzeri sorularda çok güçlüdür.

---

## 15) Performans ve Gerçekçilik Notu

Eskiden çarpma/bölme yerine shift kullanmak büyük fark yaratırdı.  
Modern derleyiciler birçok optimizasyonu zaten yapar.

Yine de bitwise hâlâ çok değerlidir:

- Algoritmik desenlerde
- Düşük seviyeli/flag tabanlı sistemlerde
- Zaman ve bellek kritik kodlarda

---

## 16) Öğrenme Rotası (Öneri)

1. `&`, `|`, `^`, `~`, `<<`, `>>` operatörlerini tek tek dene.
2. Her sayı için hem decimal hem binary yaz.
3. Şu 5 kalıbı ezberle:
   - çift/tek
   - 2’nin kuvveti
   - bit aç/kapa/kontrol
   - bit sayma
   - XOR ile tek eleman bulma
4. 20-30 soru çöz.
5. Bitmask DP ve subset enumerate seviyesine geç.

---

## 17) Hızlı Referans Kartı

```text
Bit kontrol:      (x & (1 << i)) != 0
Bit aç:           x |= (1 << i)
Bit kapa:         x &= ~(1 << i)
Bit tersle:       x ^= (1 << i)
Çift mi:          (x & 1) == 0
2'nin kuvveti:    x > 0 && (x & (x - 1)) == 0
Sağdaki 1'i sil:  x &= (x - 1)
Sağdaki 1'i al:   x & -x
```

---

## 18) Kapanış

Bitwise dünyasında ustalaşmanın sırrı:

- binary düşünmek,
- bol örnek görmek,
- kalıpları farklı sorularda tekrar uygulamak.

İstersen bu dosyanın devamına bir sonraki adım olarak:

- **Kolay/Orta/Zor 30 bitwise soru listesi**
- her soru için kısa ipucu
- ardından adım adım C# çözümleri

ekleyebilirim.

