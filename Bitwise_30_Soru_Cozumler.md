# Bitwise 30 Soru - C# Çözümler

Bu dosya, `Bitwise_30_Soru_Seti.md` içindeki 30 sorunun **doğrudan C# çözüm kodlarını** içerir.

---

## Kullanım

Aşağıdaki sınıfı bir `.cs` dosyasına kopyalayıp fonksiyonları tek tek çağırabilirsin.

```csharp
using System;
using System.Collections.Generic;

namespace Algoritmalar
{
    public static class BitwiseSoruCozumleri
    {
        // 1) Sayı çift mi tek mi?
        public static bool CiftMi(int n) => (n & 1) == 0;

        // 2) i'inci bit açık mı?
        public static bool BitAcikMi(int n, int i) => (n & (1 << i)) != 0;

        // 3) i'inci biti aç
        public static int BitAc(int n, int i) => n | (1 << i);

        // 4) i'inci biti kapat
        public static int BitKapat(int n, int i) => n & ~(1 << i);

        // 5) i'inci biti tersle
        public static int BitTersle(int n, int i) => n ^ (1 << i);

        // 6) 2'nin kuvveti mi?
        public static bool IkiKuvvetiMi(int n) => n > 0 && (n & (n - 1)) == 0;

        // 7) Sağdaki 1 bitini sil
        public static int SagdakiBiriSil(int n) => n & (n - 1);

        // 8) Sağdaki 1 bitini izole et
        public static int SagdakiBiriAl(int n) => n & -n;

        // 9) Swap (tuple ile pratik)
        public static void Swap(ref int a, ref int b) => (a, b) = (b, a);

        // XOR swap istersen:
        public static void XorSwap(ref int a, ref int b)
        {
            if (a == b) return; // aynı referans/value senaryolarına karşı güvenli
            a ^= b;
            b ^= a;
            a ^= b;
        }

        // 10) Tek kalan eleman (diğerleri 2 kez)
        public static int TekKalan(int[] arr)
        {
            int ans = 0;
            foreach (int v in arr) ans ^= v;
            return ans;
        }

        // 11) Popcount
        public static int Popcount(int n)
        {
            int count = 0;
            while (n != 0)
            {
                n &= (n - 1);
                count++;
            }
            return count;
        }

        // 12) Trailing zero sayısı (n > 0 varsayımı)
        public static int TrailingZeroCount(int n)
        {
            if (n == 0) return 32;
            int c = 0;
            while ((n & 1) == 0)
            {
                c++;
                n >>= 1;
            }
            return c;
        }

        // 13) En yüksek set bit indeksini bul (0-based)
        public static int HighestSetBitIndex(int n)
        {
            if (n <= 0) return -1;
            int idx = 0;
            while (n > 1)
            {
                n >>= 1;
                idx++;
            }
            return idx;
        }

        // 14) 32-bit bitleri ters çevir
        public static uint ReverseBits(uint n)
        {
            uint ans = 0;
            for (int i = 0; i < 32; i++)
            {
                ans = (ans << 1) | (n & 1);
                n >>= 1;
            }
            return ans;
        }

        // 15) XOR(1..n)
        public static int Xor1ToN(int n)
        {
            int r = n & 3; // n % 4
            if (r == 0) return n;
            if (r == 1) return 1;
            if (r == 2) return n + 1;
            return 0;
        }

        // 16) İki tekil eleman (diğerleri 2 kez)
        public static (int, int) IkiTekilEleman(int[] arr)
        {
            int xorAll = 0;
            foreach (int v in arr) xorAll ^= v;

            int mask = xorAll & -xorAll;
            int a = 0, b = 0;
            foreach (int v in arr)
            {
                if ((v & mask) == 0) a ^= v;
                else b ^= v;
            }
            return (a, b);
        }

        // 17) 4'ün kuvveti mi?
        public static bool DortKuvvetiMi(int n)
        {
            if (n <= 0 || (n & (n - 1)) != 0) return false;
            // 1 bitinin pozisyonu çift indexte olmalı (0,2,4,...)
            return (n & 0x55555555) != 0;
        }

        // 18) Tüm alt maskeleri üret
        public static List<int> AltMaskeler(int mask)
        {
            var list = new List<int>();
            for (int sub = mask; sub > 0; sub = (sub - 1) & mask)
                list.Add(sub);
            list.Add(0);
            return list;
        }

        // 19) Hamming distance
        public static int HammingDistance(int a, int b) => Popcount(a ^ b);

        // 20) Range AND [left..right]
        public static int RangeBitwiseAnd(int left, int right)
        {
            int shift = 0;
            while (left < right)
            {
                left >>= 1;
                right >>= 1;
                shift++;
            }
            return left << shift;
        }

        // 21) Her sayı 3 kez, biri 1 kez
        public static int TekKalan_UcTekrar(int[] arr)
        {
            int ans = 0;
            for (int bit = 0; bit < 32; bit++)
            {
                int sum = 0;
                foreach (int v in arr)
                {
                    if (((v >> bit) & 1) != 0) sum++;
                }
                if ((sum % 3) != 0) ans |= (1 << bit);
            }
            return ans;
        }

        // 22) Missing + Repeating (1..n)
        public static (int missing, int repeating) MissingRepeating(int[] arr)
        {
            int n = arr.Length;
            int xorAll = 0;
            for (int i = 1; i <= n; i++) xorAll ^= i;
            foreach (int v in arr) xorAll ^= v;

            int mask = xorAll & -xorAll;
            int a = 0, b = 0;

            for (int i = 1; i <= n; i++)
            {
                if ((i & mask) == 0) a ^= i;
                else b ^= i;
            }
            foreach (int v in arr)
            {
                if ((v & mask) == 0) a ^= v;
                else b ^= v;
            }

            // a ve b'den hangisi repeating?
            int countA = 0;
            foreach (int v in arr) if (v == a) countA++;

            if (countA == 2) return (b, a);
            return (a, b);
        }

        // 23) n-bit Gray code
        public static List<int> GrayCode(int n)
        {
            int size = 1 << n;
            var res = new List<int>(size);
            for (int i = 0; i < size; i++)
                res.Add(i ^ (i >> 1));
            return res;
        }

        // 24) Maksimum XOR çifti (bit-trie)
        private class TrieNode
        {
            public TrieNode Zero;
            public TrieNode One;
        }

        public static int MaxPairXor(int[] arr)
        {
            TrieNode root = new TrieNode();

            void Insert(int x)
            {
                TrieNode cur = root;
                for (int b = 31; b >= 0; b--)
                {
                    int bit = (x >> b) & 1;
                    if (bit == 0)
                    {
                        cur.Zero ??= new TrieNode();
                        cur = cur.Zero;
                    }
                    else
                    {
                        cur.One ??= new TrieNode();
                        cur = cur.One;
                    }
                }
            }

            int QueryBest(int x)
            {
                TrieNode cur = root;
                int ans = 0;
                for (int b = 31; b >= 0; b--)
                {
                    int bit = (x >> b) & 1;
                    if (bit == 0)
                    {
                        if (cur.One != null) { ans |= (1 << b); cur = cur.One; }
                        else cur = cur.Zero;
                    }
                    else
                    {
                        if (cur.Zero != null) { ans |= (1 << b); cur = cur.Zero; }
                        else cur = cur.One;
                    }
                }
                return ans;
            }

            foreach (int v in arr) Insert(v);
            int best = 0;
            foreach (int v in arr) best = Math.Max(best, QueryBest(v));
            return best;
        }

        // 25) Tam olarak 1 bit farklı mı?
        public static bool BirBitFarkliMi(int a, int b)
        {
            int x = a ^ b;
            return x > 0 && (x & (x - 1)) == 0;
        }

        // 26) Alt küme XOR = k var mı? (küçük n için bitmask brute force)
        public static bool SubsetXorVarMi(int[] arr, int k)
        {
            int n = arr.Length;
            int total = 1 << n;
            for (int mask = 0; mask < total; mask++)
            {
                int xr = 0;
                for (int i = 0; i < n; i++)
                {
                    if ((mask & (1 << i)) != 0) xr ^= arr[i];
                }
                if (xr == k) return true;
            }
            return false;
        }

        // 27) XOR basis ile maksimum elde edilebilir XOR
        public static int MaxSubsetXor(int[] arr)
        {
            int[] basis = new int[32];

            foreach (int x0 in arr)
            {
                int x = x0;
                for (int b = 31; b >= 0; b--)
                {
                    if (((x >> b) & 1) == 0) continue;
                    if (basis[b] == 0)
                    {
                        basis[b] = x;
                        break;
                    }
                    x ^= basis[b];
                }
            }

            int ans = 0;
            for (int b = 31; b >= 0; b--)
            {
                ans = Math.Max(ans, ans ^ basis[b]);
            }
            return ans;
        }

        // 28) Tüm alt kümelerin OR toplamı
        public static long SumOfSubsetOr(int[] arr)
        {
            int n = arr.Length;
            long totalSubsetsPow = 1L << (n - 1); // bir bit varsa katkı verdiği alt kümeler
            long sum = 0;

            for (int b = 0; b < 31; b++)
            {
                int countZero = 0;
                foreach (int v in arr)
                    if (((v >> b) & 1) == 0) countZero++;

                long subsetsAllZeroAtBit = 1L << countZero;
                long totalSubsets = 1L << n;
                long withBitOne = totalSubsets - subsetsAllZeroAtBit;
                sum += withBitOne * (1L << b);
            }

            return sum;
        }

        // 29) Tüm alt kümelerin XOR toplamı
        public static long SumOfSubsetXor(int[] arr)
        {
            int n = arr.Length;
            if (n == 0) return 0;
            int orAll = 0;
            foreach (int v in arr) orAll |= v;
            return (long)orAll * (1L << (n - 1));
        }

        // 30) Bitmask TSP (başlangıç 0)
        public static int TspBitmask(int[,] dist)
        {
            int n = dist.GetLength(0);
            int maxMask = 1 << n;
            const int INF = 1_000_000_000;

            int[,] dp = new int[maxMask, n];
            for (int m = 0; m < maxMask; m++)
                for (int i = 0; i < n; i++)
                    dp[m, i] = INF;

            dp[1, 0] = 0; // sadece 0 ziyaretli, son 0

            for (int mask = 1; mask < maxMask; mask++)
            {
                for (int last = 0; last < n; last++)
                {
                    int cur = dp[mask, last];
                    if (cur >= INF) continue;
                    for (int next = 0; next < n; next++)
                    {
                        if ((mask & (1 << next)) != 0) continue;
                        int nmask = mask | (1 << next);
                        dp[nmask, next] = Math.Min(dp[nmask, next], cur + dist[last, next]);
                    }
                }
            }

            int full = maxMask - 1;
            int ans = INF;
            for (int last = 0; last < n; last++)
                ans = Math.Min(ans, dp[full, last] + dist[last, 0]);

            return ans;
        }
    }
}
```

---

## Kısa Test Örnekleri

```csharp
Console.WriteLine(BitwiseSoruCozumleri.CiftMi(14)); // True
Console.WriteLine(BitwiseSoruCozumleri.IkiKuvvetiMi(16)); // True
Console.WriteLine(BitwiseSoruCozumleri.Popcount(29)); // 4
Console.WriteLine(BitwiseSoruCozumleri.TekKalan(new[] {4,1,2,1,2,4,9})); // 9
Console.WriteLine(BitwiseSoruCozumleri.HammingDistance(10, 14)); // 2
```

İstersen bir sonraki adımda bunu `.cs` dosyasına da dönüştürüp projeye direkt entegre edebilirim.

