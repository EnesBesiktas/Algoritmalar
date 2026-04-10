# Dinamik Programlama — Rod Cutting, Coin Change, Minimum Yol

---

## 1) Rod Cutting Problemi

Elimizde bir çubuk var. Bu çubuğun uzunluğu **n**.

Ayrıca her parça uzunluğu için bir fiyat tablomuz var.

**Amaç:** Bu çubuğu ister kes, ister hiç kesme; öyle bir şekilde parçala ki toplam kazanç maksimum olsun.

### Örnek

- **Çubuk uzunluğu:** 4  
- **Fiyatlar:**
  - 1 uzunluk → 2 TL  
  - 2 uzunluk → 5 TL  
  - 3 uzunluk → 7 TL  
  - 4 uzunluk → 8 TL  

4 uzunluğundaki çubuğu nasıl satabiliriz?

- 4 olarak direkt sat → 8  
- 1 + 3 yap → 2 + 7 = 9  
- 2 + 2 yap → 5 + 5 = 10  
- 1 + 1 + 2 yap → 2 + 2 + 5 = 9  
- 1 + 1 + 1 + 1 yap → 8  

**En iyi sonuç:** 10 — **2 + 2** şeklinde kesmek.

### Yaklaşım

Bir uzunluk için en iyi sonucu bulurken, o uzunluğu **ilk parçaya** ayırıp geriye kalan kısmın en iyi sonucunu kullanırız.

**Uzunluk = 4 için:**

- İlk parçayı 1 alırsam: `fiyat[1] + enIyi[3]`
- İlk parçayı 2 alırsam: `fiyat[2] + enIyi[2]`
- İlk parçayı 3 alırsam: `fiyat[3] + enIyi[1]`
- İlk parçayı 4 alırsam: `fiyat[4] + enIyi[0]`

Bunların en büyüğünü seçeriz:

`enIyi[4] = max(fiyat[1]+enIyi[3], fiyat[2]+enIyi[2], fiyat[3]+enIyi[1], fiyat[4]+enIyi[0])`

Elimde 4’lük çubuk var. Bu 4’lük çubuğun **ilk parçası** ne olsun?

- 1 olsun  
- 2 olsun  
- 3 olsun  
- 4 olsun  

Her biri için: **ilk parçanın fiyatı + geri kalan kısmın en iyi çözümü**.

Önce küçük uzunlukları çözüyoruz, sonra onları kullanarak büyükleri çözüyoruz:

- `enIyi[1]` bulundu  
- `enIyi[2]` bulundu  
- `enIyi[3]` bulundu  

Sonra `enIyi[4]` bulunurken bunları kullanıyoruz.

Algoritma, çubuğu her uzunluk için **“ilk parçayı kaç alayım?”** diye düşünür. İlk parçayı seçtikten sonra geriye kalan kısmın en iyi çözümünü yeniden hesaplamaz; çünkü onu zaten daha önce bulmuş olur. Böylece aynı işleri tekrar tekrar yapmadan en iyi kazancı bulur.

### Sorular (Rod Cutting)

1. Toplam çubuk boyu **1** ise en iyi kazanç ne?  
2. Toplam çubuk boyu **2** ise en iyi kazanç ne?  

*(Yukarıdaki örnek fiyat tablosu: `{ 0, 2, 5, 7, 8 }` ile düşünülebilir.)*

3. `enIyi[1]` — 1 boyundaki çubuktan en fazla kaç kazanırım?  
4. `enIyi[2]` — 2 boyundaki çubuktan en fazla kaç kazanırım?  
5. `enIyi[3]` — 3 boyundaki çubuktan en fazla kaç kazanırım?  
6. `enIyi[4]` — 4 boyundaki çubuktan en fazla kaç kazanırım?  

### Örnek kod (C#)

```csharp
using System;

class Program
{
   static void Main()
   {
       // fiyatlar dizisinde indis = parça uzunluğu
       // 0. indis kullanılmıyor, kolaylık olsun diye başa 0 koyduk
       int[] fiyatlar = { 0, 2, 5, 7, 8 };

       int cubukUzunlugu = 4;

       int maksimumKazanc = RodKesme(fiyatlar, cubukUzunlugu);

       Console.WriteLine("Maksimum kazanç: " + maksimumKazanc);
   }

   static int RodKesme(int[] fiyatlar, int cubukUzunlugu)
   {
       // enIyi[i] = i uzunluğundaki çubuktan elde edilebilecek maksimum kazanç
       int[] enIyi = new int[cubukUzunlugu + 1];

       // 0 uzunlukta çubuk varsa kazanç 0'dır
       enIyi[0] = 0;

       // 1'den başlayarak tüm uzunluklar için maksimum kazancı hesaplıyoruz
       for (int uzunluk = 1; uzunluk <= cubukUzunlugu; uzunluk++)
       {
           // Başlangıçta çok küçük bir değer veriyoruz
           int enBuyukKazanc = -999999;

           // İlk kesilecek parçanın boyunu deniyoruz
           for (int ilkParca = 1; ilkParca <= uzunluk; ilkParca++)
           {
               // ilkParca kadar parça satılırsa onun fiyatı alınır
               // geriye kalan kısmın en iyi kazancı daha önce hesaplanmıştır
               int adayKazanc = fiyatlar[ilkParca] + enIyi[uzunluk - ilkParca];

               // Daha büyük bir kazanç bulursak güncelliyoruz
               if (adayKazanc > enBuyukKazanc)
               {
                   enBuyukKazanc = adayKazanc;
               }
           }

           // uzunluk için en iyi sonucu kaydediyoruz
           enIyi[uzunluk] = enBuyukKazanc;
       }

       // İstenen uzunluk için maksimum kazancı döndürüyoruz
       return enIyi[cubukUzunlugu];
   }
}
```

---

## 2) Coin Change Problemi

Elimizde bazı madeni paralar var. Örneğin: **1, 3, 4**.

**Amaç:** Verilen hedef tutarı oluşturmak için gereken **minimum madeni para sayısını** bulmak.

**Paralar:** 1, 3, 4  
**Hedef:** 6  

Olası çözümler:

- 1 + 1 + 1 + 1 + 1 + 1 → 6 tane  
- 3 + 3 → 2 tane  
- 4 + 1 + 1 → 3 tane  

**En az para sayısı:** 2.

### Yaklaşım

Her tutar için düşünüyoruz: **Bu tutarı oluşturmak için son kullandığım para ne olabilir?**

**6 için:**

- Son para 1 ise: `1 + enIyi[5]`  
- Son para 3 ise: `1 + enIyi[3]`  
- Son para 4 ise: `1 + enIyi[2]`  

Bunların en küçüğünü seçiyoruz:

`enIyi[6] = min(1+enIyi[5], 1+enIyi[3], 1+enIyi[2])`

6 lira oluşturmak istiyorum. **En son hangi parayı** kullanmış olabilirim?

- 1 lira  
- 3 lira  
- 4 lira  

Eğer son para 3 liraysa, geriye 3 lira kalır. 6’yı çözmek yerine 3’ün çözümünü kullanırım. Son para 4 liraysa geriye 2 kalır. Küçük alt problemlere iniyoruz; her tutar için en iyi sonucu saklıyoruz, tekrar tekrar aynı hesabı yapmıyoruz.

### Sorular (Coin Change)

1. Hedef 6 iken **son para** hangi değerler olabilir? (1, 3, 4 için ifade et.)  
2. `enIyi[6]` için hangi üç ifadenin minimumu alınır?

### Örnek kod (C#)

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] paralar = { 1, 3, 4 };
       int hedefTutar = 6;

       int sonuc = MinimumMadeniPara(paralar, hedefTutar);

       Console.WriteLine("Minimum madeni para sayısı: " + sonuc);
   }

   static int MinimumMadeniPara(int[] paralar, int hedefTutar)
   {
       // enIyi[t] = t tutarını oluşturmak için gereken minimum para sayısı
       int[] enIyi = new int[hedefTutar + 1];

       // 0 tutarı oluşturmak için 0 para gerekir
       enIyi[0] = 0;

       // Diğer tüm tutarları başlangıçta çok büyük bir sayı yapıyoruz
       for (int tutar = 1; tutar <= hedefTutar; tutar++)
       {
           enIyi[tutar] = 999999;
       }

       // 1'den hedef tutara kadar bütün tutarları dolduruyoruz
       for (int tutar = 1; tutar <= hedefTutar; tutar++)
       {
           // Elimizdeki her parayı deniyoruz
           for (int i = 0; i < paralar.Length; i++)
           {
               // Eğer bu para şu anki tutardan büyük değilse kullanılabilir
               if (paralar[i] <= tutar)
               {
                   // Bu parayı son para olarak kullanırsak
                   // 1 tane para kullandık + kalan tutarın en iyi çözümü
                   int aday = 1 + enIyi[tutar - paralar[i]];

                   // Daha az sayıda para ile oluşturabiliyorsak güncelle
                   if (aday < enIyi[tutar])
                   {
                       enIyi[tutar] = aday;
                   }
               }
           }
       }

       // Eğer hâlâ çok büyük sayıysa oluşturulamıyor demektir
       if (enIyi[hedefTutar] == 999999)
       {
           return -1;
       }

       return enIyi[hedefTutar];
   }
}
```

---

## 3) Minimum Yol Toplamı (Grid Path)

Elimizde bir matris var. Her hücrede bir maliyet değeri var.

**Sadece:** sağa veya aşağı gidebiliyoruz.

**Amaç:** Sol üstten başlayıp sağ alta giderken toplam maliyeti **minimum** yapmak.

### Örnek matris

```
1   3   1
1   5   1
4   2   1
```

- **Başlangıç:** (0, 0)  
- **Bitiş:** (2, 2)  

**En iyi yol:** 1 → 3 → 1 → 1 → 1 = **7**

### Geçiş

Buraya nereden geldim? Sadece 2 ihtimal var:

- yukarıdan geldim  
- soldan geldim  

Bu hücrenin en iyi değeri = `min(yukarıdan gelen, soldan gelen) + kendi değeri`

### Örnek kod (C#)

```csharp
using System;

class Program
{
   static void Main()
   {
       int[,] matris = {
           {1, 3, 1},
           {1, 5, 1},
           {4, 2, 1}
       };

       int sonuc = MinimumYol(matris);

       Console.WriteLine("Minimum yol maliyeti: " + sonuc);
   }

   static int MinimumYol(int[,] matris)
   {
       int satir = matris.GetLength(0);
       int sutun = matris.GetLength(1);

       // dp[i,j] = (i,j) noktasına kadar minimum maliyet
       int[,] dp = new int[satir, sutun];

       // başlangıç noktası
       dp[0, 0] = matris[0, 0]; // yola ilk kareden baslanır

       // ilk satır (sadece soldan gelebilir)
       for (int j = 1; j < sutun; j++)
       {
           dp[0, j] = dp[0, j - 1] + matris[0, j];
       }

       // ilk sütun (sadece yukarıdan gelebilir)
       for (int i = 1; i < satir; i++)
       {
           dp[i, 0] = dp[i - 1, 0] + matris[i, 0];
       }

       // diğer hücreler
       for (int i = 1; i < satir; i++)
       {
           for (int j = 1; j < sutun; j++)
           {
               // yukarıdan gelme
               int yukari = dp[i - 1, j];

               // soldan gelme
               int sol = dp[i, j - 1];

               // en küçük yolu seçiyoruz
               if (yukari < sol)
               {
                   dp[i, j] = yukari + matris[i, j];
               }
               else
               {
                   dp[i, j] = sol + matris[i, j];
               }
           }
       }

       // sağ alt köşe sonucu verir
       return dp[satir - 1, sutun - 1];
   }
}
```

### Sorular (Grid — isteğe bağlı)

1. `(1,1)` hücresine minimum maliyetle gelmek için yukarı mı soldan mı gelinir?  
2. Sağ alt köşe için `dp` değeri kaçtır? (Örnek matris ile.)

---

*Dosya: Rod Cutting, Coin Change ve Minimum Grid Yolu konularındaki açıklamalar, örnekler ve sorular.*
