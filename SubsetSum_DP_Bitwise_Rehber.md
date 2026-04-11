# Subset Sum Problemi ve Çözüm Yöntemleri

---

## Subset Sum Problemi Nedir?

Elimizde bir sayı dizisi var.

**Örnek:**

- `A = { 3, 7, 4, 2 }`  
- **Hedef = 9**

Bu dizideki elemanların **bazılarını** seçerek toplamı hedef değere eşit yapabilir miyim?

- `7 + 2 = 9` → var  
- `3 + 4 + 2 = 9` → bu da var  

Her eleman için aslında **2 karar** vardır: **al** veya **alma**.  
`n` eleman varsa toplam olası alt küme sayısı: **2^n**.

---

## 1. Recursive Çözüm

Her eleman için iki seçenek var:

- elemanı **al**  
- elemanı **alma**  

`i` indisindeki elemana geldim.

- `dizi[i]` elemanını **alırsam** hedef ne olur?  
- **Almazsam** hedef ne olur?  

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] dizi = { 3, 7, 4, 2 };
       int hedef = 9;

       bool sonuc = SubsetSumRecursive(dizi, 0, hedef);

       if (sonuc)
           Console.WriteLine("Hedef toplamı veren alt küme vardır.");
       else
           Console.WriteLine("Hedef toplamı veren alt küme yoktur.");
   }

   static bool SubsetSumRecursive(int[] dizi, int indis, int hedef)
   {
       if (hedef == 0)
       {
           return true;
       }

       if (indis == dizi.Length)
       {
           return false;
       }

       if (SubsetSumRecursive(dizi, indis + 1, hedef - dizi[indis]))
       {
           return true;
       }

       if (SubsetSumRecursive(dizi, indis + 1, hedef))
       {
           return true;
       }

       return false;
   }
}
```

---

## 2. Recursive + Dinamik Programlama (Memoization)

Normal recursive çözümde aynı alt problemler tekrar tekrar çözülür. `F(3, 5)` durumu birden fazla farklı yoldan tekrar gelebilir. Bu gereksiz tekrarları önlemek için sonucu saklarız.

Bir durum şu iki bilgiyle belirlenir:

- hangi indisteyim  
- kalan hedef kaç  

Yani durum: **`(indis, hedef)`**  
Eğer daha önce `(indis, hedef)` durumunu çözdüysem, bir daha çözmem.

### Memoization mantığı

Bir tablo tutarız: `durum[indis, hedef]`

Boolean tablo tek başına yetmez çünkü:

- henüz hesaplanmamış  
- false çıkmış  
- true çıkmış  

üç durum lazım. Bu yüzden:

- **-1** → hesaplanmadı  
- **0** → false  
- **1** → true  

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] dizi = { 3, 7, 4, 2 };
       int hedef = 9;

       int[,] memo = new int[dizi.Length + 1, hedef + 1]; // 0,0 noktasını da bilmeliyiz

       int i, j;

       for (i = 0; i <= dizi.Length; i++)
       {
           for (j = 0; j <= hedef; j++)
           {
               memo[i, j] = -1;
           }
       }

       bool sonuc = SubsetSumMemo(dizi, 0, hedef, memo);

       if (sonuc)
           Console.WriteLine("Hedef toplamı veren alt küme vardır.");
       else
           Console.WriteLine("Hedef toplamı veren alt küme yoktur.");
   }

   static bool SubsetSumMemo(int[] dizi, int indis, int hedef, int[,] memo)
   {
       if (hedef == 0)
       {
           return true;
       }

       if (indis == dizi.Length)
       {
           return false;
       }

       if (hedef < 0)
       {
           return false;
       }

       if (memo[indis, hedef] != -1)
       {
           if (memo[indis, hedef] == 1)
               return true;
           else
               return false;
       }

       bool al = SubsetSumMemo(dizi, indis + 1, hedef - dizi[indis], memo);
       bool alma = SubsetSumMemo(dizi, indis + 1, hedef, memo);

       if (al || alma)
       {
           memo[indis, hedef] = 1;
           return true;
       }
       else
       {
           memo[indis, hedef] = 0;
           return false;
       }
   }
}
```

### Neden dynamic programming?

- problem alt problemlere ayrılıyor  
- aynı alt problem tekrar oluşuyor  
- sonucu saklıyoruz  
- tekrar hesap yapmıyoruz  

---

## 3. Recursive + Pruning

`kalanToplam[i]` dizisi tutalım:

- `i` indisinden **sona kadar** tüm elemanların toplamı  

**Örnek:** `dizi = { 3, 7, 4, 2 }`

| i | kalanToplam[i] |
|---|----------------|
| 0 | 16 |
| 1 | 13 |
| 2 | 6 |
| 3 | 2 |
| 4 | 0 |

Eğer `hedef > kalanToplam[indis]` ise artık hedefe ulaşamayız.

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] dizi = { 3, 7, 4, 2 };
       int hedef = 9;

       int[] kalanToplam = new int[dizi.Length + 1];
       HazirlaKalanToplam(dizi, kalanToplam);

       bool sonuc = SubsetSumPruning(dizi, 0, hedef, kalanToplam);

       if (sonuc)
           Console.WriteLine("Hedef toplamı veren alt küme vardır.");
       else
           Console.WriteLine("Hedef toplamı veren alt küme yoktur.");
   }

   static void HazirlaKalanToplam(int[] dizi, int[] kalanToplam)
   {
       int i;
       kalanToplam[dizi.Length] = 0;

       for (i = dizi.Length - 1; i >= 0; i--)
       {
           kalanToplam[i] = kalanToplam[i + 1] + dizi[i];
       }
   }

   static bool SubsetSumPruning(int[] dizi, int indis, int hedef, int[] kalanToplam)
   {
       if (hedef == 0)
       {
           return true;
       }

       if (indis == dizi.Length)
       {
           return false;
       }

       if (hedef < 0)
       {
           return false;
       }

       if (hedef > kalanToplam[indis])
       {
           return false;
       }

       if (SubsetSumPruning(dizi, indis + 1, hedef - dizi[indis], kalanToplam))
       {
           return true;
       }

       if (SubsetSumPruning(dizi, indis + 1, hedef, kalanToplam))
       {
           return true;
       }

       return false;
   }
}
```

---

## 4. Maskeleme Yöntemi ile Çözüm (Bitwise)

Her alt kümeyi bir sayı ile temsil ederiz.

`dizi = { 3, 7, 4, 2 }` → 4 eleman → **4 bit** (aslında `n` eleman için `n` bit; `2^n` maske).

**Örnek maske:** `0101` (ikilik) — hangi bitin 1 olduğuna göre hangi indekslerin seçildiği belirlenir (`j` için `dizi[j]` seçili).

Bir bitin 1 olup olmadığını kontrol etmek için:

```text
(mask & (1 << j)) != 0
```

- `1 << j` → sadece `j`. bit 1  
- `mask & (1 << j)` → maskede o bit açık mı?  

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] dizi = { 3, 7, 4, 2 };
       int hedef = 9;

       bool sonuc = SubsetSumBitwise(dizi, hedef);

       if (sonuc)
           Console.WriteLine("Hedef toplamı veren alt küme vardır.");
       else
           Console.WriteLine("Hedef toplamı veren alt küme yoktur.");
   }

   static bool SubsetSumBitwise(int[] dizi, int hedef)
   {
       int n = dizi.Length;
       int toplamAltKume = 1 << n;

       int mask, j;

       for (mask = 0; mask < toplamAltKume; mask++)
       {
           int toplam = 0;

           for (j = 0; j < n; j++)
           {
               // 0, 1, 2, 3... maskeler: 0000, 0001, 0010, 0011, 0100, 0101, ... 1111
               if ((mask & (1 << j)) != 0)
               {
                   toplam = toplam + dizi[j];
               }
           }

           if (toplam == hedef)
           {
               return true;
           }
       }

       return false;
   }
}
```

---

## Recursive: Alt küme yazdırma

Hedef toplamına uyan **bir** alt küme bulunursa ekrana yazar (ilk bulduğu çözüm).

```csharp
using System;

class Program
{
   static void Main()
   {
       int[] dizi = { 3, 7, 4, 2 };
       int hedef = 9;
       int[] secim = new int[dizi.Length];

       bool sonuc = SubsetYazdirRecursive(dizi, 0, hedef, secim);

       if (!sonuc)
       {
           Console.WriteLine("Hedef toplamı veren alt küme yoktur.");
       }
   }

   static bool SubsetYazdirRecursive(int[] dizi, int indis, int hedef, int[] secim)
   {
       int i;

       if (hedef == 0)
       {
           Console.Write("Alt kume: { ");
           for (i = 0; i < dizi.Length; i++)
           {
               if (secim[i] == 1)
               {
                   Console.Write(dizi[i] + " ");
               }
           }
           Console.WriteLine("}");
           return true;
       }

       if (indis == dizi.Length)
       {
           return false;
       }

       secim[indis] = 1;
       if (SubsetYazdirRecursive(dizi, indis + 1, hedef - dizi[indis], secim))
       {
           return true;
       }

       secim[indis] = 0;
       if (SubsetYazdirRecursive(dizi, indis + 1, hedef, secim))
       {
           return true;
       }

       return false;
   }
}
```

---

## FOR’suz üç boyutlu indeks dolaşma

`for` kullanmadan `(x, y, z)` tüm kombinasyonlarını sırayla gezmek: önce `z` artar, sınırda sıfırlanıp `y` artar, o da sınırda `x` artar.

```csharp
using System;

class Program
{
   static void Main()
   {
       int xMax = 2;
       int yMax = 3;
       int zMax = 4;

       int x = 0, y = 0, z = 0;

       bool bitti = false;

       while (!bitti)
       {
           Console.WriteLine("[" + x + "," + y + "," + z + "]");

           z = z + 1;

           if (z == zMax)
           {
               z = 0;
               y = y + 1;

               if (y == yMax)
               {
                   y = 0;
                   x = x + 1;

                   if (x == xMax)
                   {
                       bitti = true;
                   }
               }
           }
       }
   }
}
```

---

*Dosya: Subset Sum (recursive, memoization, pruning, bitwise), alt küme yazdırma ve for’suz 3B dolaşma örnekleri.*
