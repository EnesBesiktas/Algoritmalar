# Bitwise 30 Soru Çalışma Seti (C#)

Bu set 3 seviyeden oluşur:

- **Kolay (1-10):** temel operatör ve kalıplar
- **Orta (11-20):** birden fazla fikir birleştirme
- **Zor (21-30):** bitmask düşünme ve ileri seviye desenler

Her soruda:

- **Soru**
- **İpucu**
- **Çözüm fikri (kısa)**

---

## Kolay Seviye (1-10)

### 1) Sayı çift mi tek mi?
- **Soru:** Verilen `n` sayısının çift mi tek mi olduğunu bulun.
- **İpucu:** Son bit her şeyi söyler.
- **Çözüm fikri:** `(n & 1) == 0` ise çift.

### 2) i'inci bit açık mı?
- **Soru:** `n` sayısında `i` indeksli bitin 1 olup olmadığını kontrol edin.
- **İpucu:** `1 << i` ile maske üret.
- **Çözüm fikri:** `(n & (1 << i)) != 0`.

### 3) i'inci biti aç (1 yap)
- **Soru:** `n` içinde `i` bitini 1 yapın.
- **İpucu:** OR işlemi kullan.
- **Çözüm fikri:** `n |= (1 << i)`.

### 4) i'inci biti kapat (0 yap)
- **Soru:** `n` içinde `i` bitini 0 yapın.
- **İpucu:** Önce maskeyi tersle.
- **Çözüm fikri:** `n &= ~(1 << i)`.

### 5) i'inci biti tersle
- **Soru:** `n` sayısında `i` bitini toggle edin.
- **İpucu:** XOR farklıysa 1 üretir.
- **Çözüm fikri:** `n ^= (1 << i)`.

### 6) 2'nin kuvveti mi?
- **Soru:** `n` sayısı 2’nin kuvveti mi?
- **İpucu:** 2’nin kuvvetlerinde tek bir 1 biti vardır.
- **Çözüm fikri:** `n > 0 && (n & (n - 1)) == 0`.

### 7) Sağdaki 1 bitini sil
- **Soru:** `n` sayısının en sağdaki 1 bitini temizleyin.
- **İpucu:** Klasik bit hilesi.
- **Çözüm fikri:** `n &= (n - 1)`.

### 8) Sağdaki 1 bitini izole et
- **Soru:** `n` içindeki en düşük değerli 1 bitini bulun.
- **İpucu:** Negatifini kullan.
- **Çözüm fikri:** `lowbit = n & -n`.

### 9) İki sayıyı swap et (geçici değişken yok)
- **Soru:** `a` ve `b` değerlerini üçüncü değişken olmadan yer değiştirin.
- **İpucu:** XOR swap.
- **Çözüm fikri:** `a ^= b; b ^= a; a ^= b;`

### 10) Tek kalan elemanı bul (diğerleri çift tekrar)
- **Soru:** Dizide her eleman 2 kez, sadece biri 1 kez geçiyor. O elemanı bulun.
- **İpucu:** Aynı sayılar XOR’da yok olur.
- **Çözüm fikri:** Tüm diziyi XOR’la.

---

## Orta Seviye (11-20)

### 11) Bit sayısı (popcount)
- **Soru:** `n` sayısının binary gösterimindeki 1 bit sayısını bulun.
- **İpucu:** Her adımda bir 1 sil.
- **Çözüm fikri:** `while (n != 0) { n &= (n - 1); count++; }`

### 12) Son sıfır sayısı (trailing zeros)
- **Soru:** `n` sayısının binary sonunda kaç tane 0 var?
- **İpucu:** `lowbit` ile ilişki kur.
- **Çözüm fikri:** Döngüyle `(n & 1) == 0` kontrol et ya da `lowbit` üstünden hesapla.

### 13) Bir sayıdaki en yüksek set bitin indeksini bul
- **Soru:** `n > 0` için en soldaki 1 bitin pozisyonu nedir?
- **İpucu:** Sağa kaydırarak ilerle.
- **Çözüm fikri:** `while (n > 1) { n >>= 1; idx++; }`

### 14) n'in ikilikteki bitlerini ters çevir
- **Soru:** 32 bitlik bir sayının bit sırasını tersine çevirin.
- **İpucu:** Bir bit al, sonuca sola ekle.
- **Çözüm fikri:** 32 adım döngü ile `ans = (ans << 1) | (n & 1); n >>= 1;`

### 15) Aralık [1..n] XOR sonucu
- **Soru:** `1 ^ 2 ^ ... ^ n` değerini O(1) bulun.
- **İpucu:** `n % 4` desenini keşfet.
- **Çözüm fikri:** 4 duruma göre sabit dönüş.

### 16) Tekrarsız iki sayı (diğerleri çift tekrar)
- **Soru:** Dizide iki sayı birer kez, diğerleri ikişer kez geçiyor. İki sayıyı bulun.
- **İpucu:** Önce tümünü XOR’la, sonra ayırıcı bit kullan.
- **Çözüm fikri:** `xorAll`, `mask = xorAll & -xorAll`, iki gruba XOR.

### 17) Tam sayı bölmeden üs alma kontrolü
- **Soru:** `n` sayısı `4`’ün kuvveti mi?
- **İpucu:** Önce 2’nin kuvveti olmalı, sonra bitin pozisyonu çift olmalı.
- **Çözüm fikri:** `n > 0`, `n&(n-1)==0`, ve set bit doğru maskede.

### 18) n'in alt kümelerini dolaş
- **Soru:** Bir `mask` için tüm alt maskeleri gez.
- **İpucu:** Bir satırlık döngü tekniği var.
- **Çözüm fikri:** `for (sub = mask; sub > 0; sub = (sub - 1) & mask)`.

### 19) Hamming distance
- **Soru:** İki sayının farklı bit sayısını bulun.
- **İpucu:** Önce XOR al.
- **Çözüm fikri:** `popcount(a ^ b)`.

### 20) Range AND (L..R)
- **Soru:** `L` ile `R` arasındaki tüm sayıların AND sonucu nedir?
- **İpucu:** Ortak prefix yaklaşımı.
- **Çözüm fikri:** `L` ve `R` eşitlenene kadar sağa kaydır, sonra geri kaydır.

---

## Zor Seviye (21-30)

### 21) Her sayı 3 kez, biri 1 kez
- **Soru:** Dizide her eleman 3 kez tekrar ediyor, yalnız bir eleman 1 kez. Onu bulun.
- **İpucu:** Her bit pozisyonunu mod 3 say.
- **Çözüm fikri:** 32 bit için sayaç tut, `%3` kalanlarla sonucu kur.

### 22) Missing + repeating (1..n)
- **Soru:** 1..n içinde bir sayı eksik, bir sayı 2 kez var. İkisini bulun.
- **İpucu:** XOR ile iki gruba ayrıştır.
- **Çözüm fikri:** `xorAll = arrayXor ^ rangeXor`, ayırıcı bit ile iki aday, sayım ile doğrula.

### 23) Gray code üretimi
- **Soru:** `n` bitlik Gray code dizisini üretin.
- **İpucu:** Formül tek satır.
- **Çözüm fikri:** `gray(i) = i ^ (i >> 1)`.

### 24) XOR maksimum çift (trie yaklaşımı)
- **Soru:** Dizideki iki sayının maksimum XOR değerini bulun.
- **İpucu:** Bit trie ile karşıt biti tercih et.
- **Çözüm fikri:** 31. bitten 0’a trie ekleme/arama.

### 25) Sadece bir bit farklı mı?
- **Soru:** `a` ve `b` sayıları tam olarak 1 bitte mi farklı?
- **İpucu:** XOR sonucu 2’nin kuvveti mi bak.
- **Çözüm fikri:** `x = a ^ b`, `x > 0 && (x & (x - 1)) == 0`.

### 26) Bir alt küme hedef XOR veriyor mu?
- **Soru:** Verilen dizide XOR toplamı `k` olan alt küme var mı?
- **İpucu:** DP/bitmask veya lineer baz yaklaşımı.
- **Çözüm fikri:** Küçük n’de bitmask dene; büyük n’de XOR basis düşün.

### 27) XOR basis (lineer cebir GF(2))
- **Soru:** Diziden seçilen sayıların XOR’uyla elde edilebilecek maksimum değeri bulun.
- **İpucu:** Gaussian elimination benzeri bit baz.
- **Çözüm fikri:** En yüksek bitten başlayarak basis kur, greedy ile maksimumu inşa et.

### 28) Tüm alt kümelerin OR toplamı
- **Soru:** Dizinin tüm alt kümelerinin OR değerlerinin toplamını bulun.
- **İpucu:** Her bitin kaç alt kümeye katkı verdiğini say.
- **Çözüm fikri:** Bit bazında kombinasyon sayımıyla O(32*n).

### 29) Tüm alt kümelerin XOR toplamı
- **Soru:** Dizinin tüm alt kümelerinin XOR toplamını bulun.
- **İpucu:** Bir bit en az bir elemanda varsa yarı alt kümelerde 1 olur.
- **Çözüm fikri:** `OR(arr) * 2^(n-1)` mantığı.

### 30) Bitmask TSP (Gezgin Satıcı - küçük n)
- **Soru:** Küçük `n` için en kısa turu bitmask DP ile çözün.
- **İpucu:** `dp[mask][last]`.
- **Çözüm fikri:** Geçiş: `dp[mask | (1<<next)][next] = min(...)`.

---

## Bonus: Çalışma Planı

- **Gün 1:** 1-10 (temel kalıpları ezber)
- **Gün 2:** 11-20 (kombine düşünce)
- **Gün 3-4:** 21-30 (zor seviye)
- **Gün 5:** Tüm soruları baştan, koda bakmadan çöz

---

## Bonus: C# Fonksiyon İskeleti

```csharp
public static class BitwiseSoruSeti
{
    // Örnek: Soru 6 - 2'nin kuvveti
    public static bool IsPowerOfTwo(int n)
    {
        return n > 0 && (n & (n - 1)) == 0;
    }
}
```

İstersen bir sonraki adımda bu 30 sorunun her biri için:

- tam C# fonksiyon çözümü,
- örnek input/output,
- zaman/alan karmaşıklığı

ekleyebilirim.

