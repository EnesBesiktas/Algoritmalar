# Atilla Hoca — Algoritmalar Ders Kodları (C#)

Bu doküman, derste kullanılan örnek kodların **konuya göre düzenlenmiş** halidir.  
Amaç: Özyineleme, ağaçlar, hash, kuyruk/yığın, bitwise, N-Queen ve eşzamanlılık konularını tek yerde görmek.

> Kaynak: `ConsoleApp2` ders notları. Kodlar öğretim amaçlıdır; bazı örneklerde sınırlar ve isim çakışmaları bilinçli bırakılmıştır.

---

## İçindekiler

1. [Özyineleme (Recursion)](#1-özyineleme-recursion)
2. [Paranın Ödenmesi (Coin Change)](#2-para-ödeme-coin-change)
3. [Alternating Toplam](#3-alternating-toplam)
4. [Bitwise — Alt Küme Yazdırma](#4-bitwise--alt-küme-yazdırma)
5. [Binary String → Sayı](#5-binary-string--sayı)
6. [Huffman Kodlama](#6-huffman-kodlama)
7. [Bağlı Liste, Yığın ve Kuyruk](#7-bağlı-liste-yığın-ve-kuyruk)
8. [İkili Ağaç — Dizi Temsili](#8-ikili-ağaç--dizi-temsili)
9. [İkili Ağaç — Pointer Temsili](#9-ikili-ağaç--pointer-temsili)
10. [Hash Tablosu](#10-hash-tablosu)
11. [FAT32 Cluster Zinciri](#11-fat32-cluster-zinciri)
12. [N-Queen (8 Vezir)](#12-n-queen-8-vezir)
13. [Eşzamanlılık (Thread / Lock)](#13-eşzamanlılık-thread--lock)
14. [3B Dizi ve Bağlı Liste Dönüşümü](#14-3b-dizi-ve-bağlı-liste-dönüşümü)
15. [Ders Fonksiyonları Özeti](#15-ders-fonksiyonları-özeti)

---

## 1. Özyineleme (Recursion)

### `inc` — İkili dallanma (Fibonacci benzeri çağrı ağacı)

```csharp
static void inc(int a)
{
    if (a <= 0) return;
    Console.WriteLine(a);
    inc(a - 1);
    inc(a - 2);
}
```

### `abc123` — Geri çağrı (post-order benzeri yazdırma)

```csharp
static void abc123(int a)
{
    if (a >= 100) return;
    abc123(a + 1);
    Console.WriteLine(a);
}
```

### `ebb` — Dizide en büyük eleman

```csharp
static int ebb(int[] x, int indis)
{
    if (indis >= 100) return 0;
    int eb = ebb(x, indis + 1);
    if (eb > x[indis]) return eb;
    else return x[indis];
}
```

### `recursivecozum` — Klasör ağacında gezinme

```csharp
static void recursivecozum(string path)
{
    Console.WriteLine(path);
    string[] dirs = Directory.GetDirectories(path);
    foreach (string item in dirs)
        recursivecozum(item);
}
```

---

## 2. Para Ödeme (Coin Change)

Kaç farklı şekilde `deger` ödenebilir? (Sınırsız tekrar — klasik özyinelemeli sayım.)

```csharp
static int odemebul(int[] coins, int coinadeti, int deger)
{
    if (deger == 0) return 1;
    if (deger < 0) return 0;
    if (coinadeti <= 0 && deger >= 1) return 0;

    return odemebul(coins, coinadeti - 1, deger)
         + odemebul(coins, coinadeti, deger - coins[coinadeti - 1]);
}
```

| Durum | Dönüş |
|--------|--------|
| `deger == 0` | Tam ödeme → `1` |
| `deger < 0` | Geçersiz → `0` |
| Para kalmadı, borç var | `0` |

---

## 3. Alternating Toplam

`1 - 2 + 3 - 4 + ...` (işaret `flag` ile değişir):

```csharp
static int bilg(int a, int flag)
{
    if (a >= 101) return 0;
    return a * flag + bilg(a + 1, flag * -1);
}
```

---

## 4. Bitwise — Alt Küme Yazdırma

`0..7` arası sayıları 3 elemanlı küme üzerinde bitmask ile yazdırır (`tmp` sırayla 4, 2, 1 bitleri):

```csharp
static void kumeyaz(int sayi, string kume)
{
    if (sayi > 7) return;

    int tmp = 0x4; // 100₂
    for (int j = 0; j < 3; j++)
    {
        if ((tmp & sayi) != 0)
            Console.Write(kume[j]);
        tmp >>= 1;
    }
    Console.WriteLine();
    kumeyaz(sayi + 1, kume);
}
```

---

## 5. Binary String → Sayı

Binary string’i onluk sayıya çevirir (özyinelemeli):

```csharp
static int bitsayisi(string s, int b, int leng)
{
    if (leng >= s.Length) return 0;
    return b * (s[leng] - '0') * bitsayisi(s, b * 10, leng + 1);
}
```

---

## 6. Huffman Kodlama

### Sınıflar

```csharp
class Btree
{
    public int data;
    public int ch;
    public string hufman;
    public Btree left, right;
}

class Hftree
{
    public int data;   // frekans
    public int ch;     // karakter
    public string hufman;
    public Hftree left, right;
}
```

### Ağaç birleştirme ve sıralama

```csharp
static Btree local(Btree a, Btree b)
{
    Btree bt = new Btree();
    bt.data = a.data + b.data;
    bt.left = a;
    bt.right = b;
    return bt;
}

static void tree(Btree[] bt)
{
    if (bt[1] == null) return;
    bt[0] = local(bt[0], bt[1]);
    bt[1] = null;

    Array.Sort(bt, (object x, object y) =>
    {
        if (x == null && y == null) return 0;
        if (x == null) return 1;
        if (y == null) return -1;
        int a = 0;
        if (((Btree)x).data < ((Btree)y).data) a = -1;
        if (((Btree)x).data > ((Btree)y).data) a = 1;
        return a;
    });
    tree(bt);
}
```

### Kodları yazdırma

```csharp
static void yazBtreeSonuc(Btree bt)
{
    if (bt == null) return;
    if (bt.left == null)
        Console.WriteLine("{0}      {1}", (char)bt.ch, bt.hufman);
    yazBtreeSonuc(bt.left);
    yazBtreeSonuc(bt.right);
}
```

### Alternatif Huffman (`Hftree` dizisi)

```csharp
static void HfCreate(Hftree[] hf)
{
    if (hf[1] == null) return;

    Hftree tmp = new Hftree();
    tmp.data = hf[0].data + hf[1].data;
    tmp.ch = -1;
    tmp.left = hf[0];
    tmp.right = hf[1];

    hf[0] = tmp;
    hf[1] = null;

    Array.Sort(hf, (a, b) =>
    {
        int sa = (a == null) ? int.MaxValue : a.data;
        int sb = (b == null) ? int.MaxValue : b.data;
        return sa.CompareTo(sb);
    });
    HfCreate(hf);
}

static void HfString(Hftree node, string st)
{
    if (node == null) return;
    if (node.right == null) node.hufman = st;
    HfString(node.left, st + '1');
    HfString(node.right, st + '0');
}
```

---

## 7. Bağlı Liste, Yığın ve Kuyruk

### Blok yapıları

```csharp
class Block
{
    public int data;
    public string hashdata;
    public Block next, prev;  // çift yönlü liste
}

class Blockt
{
    public int data;
    public Blockt next;
}
```

### Kuyruk (FIFO) — çift yönlü liste ile

```csharp
static Block rear = null, front = null;

static void Enqueue(int data)
{
    Block bl = new Block { data = data, next = null, prev = null };
    if (rear == null) front = bl;
    else { rear.next = bl; bl.prev = rear; }
    rear = bl;
}

static int Dequeue()
{
    int data = front.data;
    front = front.next;
    if (front == null) rear = null;
    return data;
}
```

### Yığın (LIFO) — dizi ile

```csharp
static int[] stack = new int[100];
static int sp = -1;

static void push(int data) { stack[++sp] = data; }
static int pop() { return stack[sp--]; }
static int peek() { return stack[sp]; }
static int stackcount() { return sp + 1; }
```

### Yığın — bağlı liste ile (`PUSH` / `POP`)

```csharp
static Block SP = null;

static void PUSH(int data)
{
    Block bl = new Block { data = data, next = SP };
    if (SP != null) SP.prev = bl;
    SP = bl;
}

static int POP()
{
    int data = SP.data;
    SP = SP.next;
    return data;
}
```

### Dairesel dizi kuyruk

```csharp
static int FRONT = 0, REAR = 0;
static int[] QUEUE = new int[100];

static void ENQUEUE(int data) { QUEUE[REAR++ % 100] = data; }
static int DEQUEUE() { return QUEUE[FRONT++ % 100]; }
static int QueueCount() { return REAR - FRONT + 1; }
```

### Bağlı liste — özyinelemeli oluşturma

```csharp
static Blockt listeyaz(Blockt bt, int adt)
{
    if (adt < 0) return bt;
    Blockt t = new Blockt { data = adt, next = bt };
    return listeyaz(t, adt - 1);
}

static void linkedyaz(Blockt bt)
{
    if (bt == null) return;
    Console.WriteLine(bt.data);
    linkedyaz(bt.next);
}
```

---

## 8. İkili Ağaç — Dizi Temsili

Kök indeks `0`; sol çocuk `2*i+1`, sağ `2*i+2`.

Örnek dizi:

```csharp
static int[] btree12 = { 50, 17, 72, 12, 23, 54, 76, 9, 14, 19, 0, 0, 67 };
```

### Pre-order / in-order / post-order (özyinelemeli)

```csharp
static void yaz(int[] btree, int i)
{
    if (i >= btree.Length) return;
    Console.WriteLine(btree[i]);
    yaz(btree, i * 2 + 1);
    yaz(btree, i * 2 + 2);
}

static void yazarmisiniz(int[] bt, int indis)  // post-order
{
    if (indis >= bt.Length) return;
    yazarmisiniz(bt, indis * 2 + 2);
    yazarmisiniz(bt, indis * 2 + 1);
    if (bt[indis] > 0) Console.WriteLine(bt[indis]);
}

static void ders7(int[] bt, int indis)  // in-order (sağ-root-sol varyantı)
{
    if (indis >= bt.Length) return;
    ders7(bt, 2 * indis + 2);
    if (bt[indis] != 0) Console.WriteLine(bt[indis]);
    ders7(bt, 2 * indis + 1);
}
```

### Stack ile gezinme (iteratif)

```csharp
static void ornek1(int[] bt)
{
    Stack<int> st = new Stack<int>();
    st.Push(0);
    while (st.Count > 0)
    {
        int indis = st.Pop();
        Console.WriteLine(bt[indis]);
        if (indis * 2 + 1 < bt.Length) st.Push(indis * 2 + 1);
        if (indis * 2 + 2 < bt.Length) st.Push(indis * 2 + 2);
    }
}
```

### Arama ve sayım

```csharp
static bool find(int[] btree, int i)
{
    if (i >= btree.Length) return;
    if (btree[i] == 76) bulundu = true;
    find(btree, i * 2 + 1);
    find(btree, i * 2 + 2);
}

static int find2(int[] btree, int i)  // 76 kaç kez var?
{
    if (i >= btree.Length) return 0;
    if (btree[i] == 76)
        return 1 + find2(btree, i * 2 + 1) + find2(btree, i * 2 + 2);
    return find2(btree, i * 2 + 1) + find2(btree, i * 2 + 2);
}

static int btders2(int[] bt, int indis)  // düğüm sayısı
{
    if (indis >= bt.Length) return 0;
    return 1 + btders2(bt, indis * 2 + 1) + btders2(bt, indis * 2 + 2);
}

static int ders2(int[] bt, int indis)  // yükseklik
{
    int a = 0, b = 0, c = indis * 2 + 1;
    if (c < bt.Length && bt[c] > 0) a = 1 + ders2(bt, c);
    c++;
    if (c < bt.Length && bt[c] > 0) b = 1 + ders2(bt, c);
    return a > b ? a : b;
}
```

### İki en büyük değer (post-order)

```csharp
static void ders4(int[] bt, int indis, ref int eb1, ref int eb2)
{
    if (indis >= bt.Length) return;
    ders4(bt, indis * 2 + 1, ref eb1, ref eb2);
    ders4(bt, indis * 2 + 2, ref eb1, ref eb2);
    if (bt[indis] > eb1) { eb2 = eb1; eb1 = bt[indis]; }
    else if (bt[indis] > eb2) eb2 = bt[indis];
}
```

### BST arama (dizi üzerinde)

```csharp
static int btders3(int[] bt, int indis, int search)
{
    if (indis >= bt.Length) return 0;
    if (bt[indis] == search) return 1;
    if (bt[indis] > search)
        return btders3(bt, 2 * indis + 1, search);
    return btders3(bt, 2 * indis + 2, search);
}
```

### İki ağacı karşılaştır

```csharp
static int comp(int[] bt1, int[] bt2, int indis)
{
    if (indis >= bt1.Length) return 0;
    if (bt1[indis] != bt2[indis]) return 1;
    return comp(bt1, bt2, indis * 2 + 1)
         + comp(bt1, bt2, indis * 2 + 2);
}
```

### Seviye 3’teki en büyük alt değer

```csharp
static int ders1(int[] bt, int indis, int seviye)
{
    if (seviye == 3) return bt[indis];
    int a = indis * 2 + 1, b = 0, c = 0;
    if (a < bt.Length && bt[a] != 0) b = ders1(bt, a, seviye + 1);
    if (++a < bt.Length && bt[a] != 0) c = ders1(bt, a, seviye + 1);
    if (b < c) b = c;
    return b;
}
```

---

## 9. İkili Ağaç — Pointer Temsili

```csharp
class BlockBtree
{
    public int data;
    public BlockBtree left, right;
}

static BlockBtree olustur(int[] btree, int i)
{
    if (i >= btree.Length) return null;
    return new BlockBtree
    {
        data = btree[i],
        left = olustur(btree, i * 2 + 1),
        right = olustur(btree, i * 2 + 2)
    };
}

static void ekle(BlockBtree btree, int data)
{
    if (btree.data < data)
    {
        if (btree.right != null) ekle(btree.right, data);
        else btree.right = new BlockBtree { data = data };
    }
    else
    {
        if (btree.left != null) ekle(btree.left, data);
        else btree.left = new BlockBtree { data = data };
    }
}

static int search(BlockBtree btree, int aranan)
{
    if (btree == null) return 0;
    if (btree.data == aranan) return 1;
    if (btree.data < aranan) return search(btree.right, aranan);
    return search(btree.left, aranan);
}
```

### `Btree` ile ekleme (`ref`)

```csharp
static void odev3Ekleme(ref Btree bt, int data)
{
    if (bt == null) { bt = new Btree { data = data }; return; }
    if (data > bt.data)
    {
        if (bt.right != null) odev3Ekleme(ref bt.right, data);
        else bt.right = new Btree { data = data };
    }
    else
    {
        if (bt.left != null) odev3Ekleme(ref bt.left, data);
        else bt.left = new Btree { data = data };
    }
}
```

---

## 10. Hash Tablosu

### Basit hash fonksiyonu (string → ASCII toplamı)

```csharp
static int hashfunction(string st)
{
    int t = 0;
    for (int i = 0; i < st.Length; i++)
        t += (byte)st[i];
    return t % 100;
}
```

### Linear probing (dizi)

```csharp
static string[] HASH = new string[100];

static void hashyaz(string[] hash, string st)
{
    int index = hashfunction(st);
    if (hash[index] == null) { hash[index] = st; return; }
    for (int i = 1; i < hash.Length; i++)
        if (hash[(index + i) % 100] == null)
        { hash[(index + i) % 100] = st; break; }
}

static int hashSearch(string[] hash, string st)
{
    int index = hashfunction(st);
    if (hash[index] == st) return 1;
    for (int i = 1; i < hash.Length; i++)
        if (hash[(index + i) % 100] == st) return 1;
    return 0;
}
```

### Zincirleme (chaining) — `Block` ile

```csharp
static Block[] HASHLinked = new Block[100];

static int Ders1_HashFunction(string st)
{
    int indis = 0;
    for (int i = 0; i < st.Length; i++)
        indis += st[i] - '0';
    return indis % HASH.Length;
}

static void Ders1_HashEkle(string st)
{
    int indis = Ders1_HashFunction(st);
    Block bl = new Block { hashdata = st, next = HASHLinked[indis] };
    if (HASHLinked[indis] != null)
        HASHLinked[indis].prev = bl;
    HASHLinked[indis] = bl;
}

static int Ders1_HashSearch(string st)
{
    int indis = Ders1_HashFunction(st);
    Block tmp = HASHLinked[indis];
    while (tmp != null)
    {
        if (tmp.hashdata == st) return 1;
        tmp = tmp.next;
    }
    return 0;
}

static void HASHDelete(string st)
{
    int indis = Ders1_HashFunction(st);
    if (HASHLinked[indis] == null) return;

    Block tmp = HASHLinked[indis];
    if (tmp.hashdata == st)
    {
        HASHLinked[indis] = HASHLinked[indis].next;
        return;
    }
    while (tmp != null)
    {
        if (tmp.hashdata == st)
        {
            if (tmp.next == null) tmp.prev.next = null;
            else { tmp.next.prev = tmp.prev; tmp.prev.next = tmp.next; }
            break;
        }
        tmp = tmp.next;
    }
}
```

### Sayısal hash (mod)

```csharp
static int[] Hash = new int[100];
static int[] HashColl = new int[100];

static int HashFunc(int data) => data % Hash.Length;

static void HashAdd(int data)
{
    int indis = HashFunc(data);
    if (HashColl[indis] == 0) { Hash[indis] = data; HashColl[indis]++; return; }
    if (Hash[indis] == data) return;
    for (int i = 0; i < Hash.Length; i++)
        if (HashColl[i] == 0) { Hash[i] = data; HashColl[i]++; return; }
}
```

---

## 11. FAT32 Cluster Zinciri

```csharp
static int getclusterid(int[] fat32, ref int link)
{
    int id = -1;
    for (int i = 1; i < fat32.Length; i++)
        if (fat32[i] == link) id = i;
    if (id != -1) link = id;
    return id;
}

static int dosyaCluster(int[] fat32, int link)
{
    if (fat32[link] == 0) return 0;
    int dosya = 1;
    while (fat32[link] != -1)
    { link = fat32[link]; dosya++; }
    return dosya;
}

static int getfilecluster(int[] fat32, int indis)
{
    if (fat32[indis] == 0) return 0;
    int dosya = 1;
    while (fat32[indis] != -1)
    {
        dosya++;
        indis = fat32[indis];
    }
    return dosya;
}
```

---

## 12. N-Queen (8 Vezir)

### Tahta kontrolü (satır / sütun / çapraz)

```csharp
static int[,] tahta = new int[8, 8];

static int kosegen(int a, int b, int yon)
{
    int adt = 0;
    for (int i = 0; i < 8; i++)
    {
        if (yon == 1)
        {
            if (a + i > 7 || b + i > 7) break;
            adt += tahta[a + i, b + i];
        }
        else
        {
            if (b - i < 0 || a + i > 7) break;
            adt += tahta[a + i, b - i];
        }
    }
    return adt;
}

static int kontrol()
{
    for (int i = 0; i < 8; i++)
    {
        int satir = 0, sutun = 0;
        for (int j = 0; j < 8; j++)
        {
            satir += tahta[i, j];
            sutun += tahta[j, i];
        }
        if (satir > 1 || sutun > 1) return 1;
        // çapraz kontroller ...
    }
    return 0;
}
```

### Backtracking — saldırı sayacı matrisi

`tahtayaz` / `tahtaDelete`: Vezir koyunca satır, sütun ve dört çaprazdaki hücrelere sayaç artırır/azaltır. `tahtakontrol(a,b)` → `t[a,b]==0` ise güvenli.

```csharp
static void tahtayaz(int[,] t, int a, int b)
{
    for (int i = 0; i < 8; i++) { t[a, i]++; t[i, b]++; }
    // dört çapraz yön için sayaç artır ...
    t[a, b] = 1;
}

static void tahtaDelete(int[,] t, int a, int b)
{
    for (int i = 0; i < 8; i++) { t[a, i]--; t[i, b]--; }
    // çaprazlar azalt ...
    t[a, b] = 0;
}

static bool tahtakontrol(int[,] t, int a, int b) => t[a, b] == 0;
```

### `Main` — 8 iç içe döngü ile tüm çözümler

64 hücreden 8 vezir seçilir; çözümler sıralanıp tekrarlar elenir. `adet` benzersiz çözüm sayısıdır.

---

## 13. Eşzamanlılık (Thread / Lock)

### `lock` ile kritik bölge

```csharp
static int abcde = 0;
static object obj = new object();

static void ders12()
{
    for (int i = 0; i < 10000000; i++)
        lock (obj) { abcde++; }  // critical section
    // race condition olmadan artış
}
```

### Semaphore benzeri (`Interlocked`)

```csharp
static int d1 = 0, lck = 0, semaphoreS = 0;

static void onay()
{
    while (Interlocked.Exchange(ref lck, 1) == 1) ;
    while (semaphoreS >= 6) ;
    semaphoreS++;
    lck = 0;
}

static void serbest() { semaphoreS--; }

static void ders1()
{
    for (int i = 0; i < 100; i++)
    {
        onay();
        d1++;
        serbest();
    }
}
```

### π hesabı — çoklu thread + `lock`

```csharp
static double Pi = 0;

static void pi2()
{
    double pi = 0;
    int flg = 1;
    for (int i = 300000001; i < 600000000; i += 2)
    {
        pi += (double)1 / i * flg;
        flg *= -1;
    }
    lock (obj) Pi += 4 * pi;
}
```

**Kavramlar:** race condition, monitor (`lock`), mutex, semaphore, `Interlocked`, kritik alan.

---

## 14. 3B Dizi ve Bağlı Liste Dönüşümü

```csharp
static void diziyeyz(int[,,] x, Blockt bt, int a, int b, int c)
{
    if (bt == null) return;
    if (c == 3) { c = 0; b++; if (b == 3) { b = 0; a++; } }
    x[a, b, c] = bt.data;
    diziyeyz(x, bt.next, a, b, c + 1);
}

static void listekopya(int[,,] x, int a, int b, int c, Blockt t)
{
    if (t == null) return;
    x[a, b, c] = t.data;
    c++;
    if (c == 3) { c = 0; b++; if (b == 3) { b = 0; a++; } }
    listekopya(x, a, b, c, t.next);
}
```

### 2B matris — özyinelemeli toplam / gezinme

```csharp
static int ders1(int[,] x, int satir, int sutun)
{
    int a = x[satir, sutun];
    sutun++;
    if (sutun >= 20) { satir++; if (satir >= 14) return a; }
    return a + ders1(x, satir, sutun);
}

static int ders11(int[,] x, int indis)
{
    int row = indis / 20, col = indis % 20;  // int[14,20]
    if (row >= 14) return 0;
    return x[row, col] + ders11(x, indis + 1);
}
```

---

## 15. Ders Fonksiyonları Özeti

| Fonksiyon | Amaç |
|-----------|------|
| `inc` | İkili özyineleme (Fibonacci ağacı) |
| `odemebul` | Coin change sayımı |
| `bilg` | Alternating toplam |
| `kumeyaz` | Bitmask alt küme |
| `tree` / `HfCreate` | Huffman ağacı |
| `Enqueue` / `Dequeue` | Kuyruk |
| `push` / `pop` | Yığın |
| `find2`, `btders2` | Ağaçta arama / sayım |
| `ders4` | İki en büyük |
| `hashyaz`, `Ders1_HashEkle` | Hash ekleme |
| `dosyaCluster` | FAT zincir uzunluğu |
| `tahtayaz`, `tahtaDelete` | N-Queen saldırı matrisi |
| `ders12`, `onay` | Thread senkronizasyonu |

---

## Notlar ve Sonraki Konular

Ders notlarında işaretlenen konular:

- **Backtracking:** N-Queen (bu dosyada `Main` ile)
- **Dynamic programming** — ayrı rehberlerde
- **Integer programming** — knapsack rehberi
- **Huffman, binary tree, hash** — yukarıda özetlendi
- **Şifreleme / steganografi** — metin veya görüntüye mesaj gömme
- **Sort:** `2;23;55;44;14;60;21;49` → `2-14-21-23-...`

---

## Delegate Örneği

```csharp
public delegate int Calculator(int x, int y);

static int topla(int a, int b) => a + b;
static int fark(int a, int b) => a - b;
// Calculator calc = topla;
```

---

*Son güncelleme: ders kodlarının konuya göre gruplanmış özeti.*
