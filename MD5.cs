using System;
using System.Collections;

namespace MD5
{
    public static class Md5
    {
        private static UInt32 _a = 0x67452301;
        private static UInt32 _b = 0xefcdab89;
        private static UInt32 _c = 0x98badcfe;
        private static UInt32 _d = 0x10325476;

        // Konstantos nusakancios, kiek bitu reikes pasukti transformaciju metu.
        private const int S11 = 7;
        private const int S12 = 12;
        private const int S13 = 17;
        private const int S14 = 22;
        private const int S21 = 5;
        private const int S22 = 9;
        private const int S23 = 14;
        private const int S24 = 20;
        private const int S31 = 4;
        private const int S32 = 11;
        private const int S33 = 16;
        private const int S34 = 23;
        private const int S41 = 6;
        private const int S42 = 10;
        private const int S43 = 15;
        private const int S44 = 21;

        public static string Out(byte[] input)
        {
            return MdString(input);
        }

        private static string MdString(byte[] input)
        {
            byte[] digest = Md5Array(input);
            return ArrayToHexString(digest);
        }

        private static byte[] Md5Array(byte[] input)
        {
            UInt32[] block = MD5_Append(input);
            UInt32[] bits = MD5_Trasform(block);

            byte[] output = new byte[bits.Length * 4];
            for (int i = 0, j = 0; i < bits.Length; i++, j += 4)
            {
                output[j] = (byte)(bits[i] & 0xff);
                output[j + 1] = (byte)((bits[i] >> 8) & 0xff);
                output[j + 2] = (byte)((bits[i] >> 16) & 0xff);
                output[j + 3] = (byte)((bits[i] >> 24) & 0xff);
            }
            return output;
        }

        private static UInt32[] MD5_Append(byte[] input)
        {
            int zeros = 0;
            int ones = 1;
            int size = 0;
            int n = input.Length;
            int m = n % 64;

            if (m < 56)
            {
                zeros = 55 - m;
                size = n - m + 64;
            }
            else if (m == 56)
            {
                zeros = 0;
                ones = 0;
                size = n + 8;
            }
            else
            {
                zeros = 63 - m + 56;
                size = n + 64 - m + 64;
            }

            ArrayList bs = new ArrayList(input);

            if (ones == 1)
            {
                bs.Add((byte)0x80);
            }
            for (int i = 0; i < zeros; i++)
            {
                bs.Add((byte)0);
            }

            UInt64 N = (UInt64)n * 8;
            byte h1 = (byte)(N & 0xFF);
            byte h2 = (byte)((N >> 8) & 0xFF);
            byte h3 = (byte)((N >> 16) & 0xFF);
            byte h4 = (byte)((N >> 24) & 0xFF);
            byte h5 = (byte)((N >> 32) & 0xFF);
            byte h6 = (byte)((N >> 40) & 0xFF);
            byte h7 = (byte)((N >> 48) & 0xFF);
            byte h8 = (byte)(N >> 56);
            bs.Add(h1);
            bs.Add(h2);
            bs.Add(h3);
            bs.Add(h4);
            bs.Add(h5);
            bs.Add(h6);
            bs.Add(h7);
            bs.Add(h8);
            byte[] ts = (byte[])bs.ToArray(typeof(byte));

            UInt32[] output = new UInt32[size / 4];
            for (Int64 i = 0, j = 0; i < size; j++, i += 4)
            {
                output[j] = (UInt32)(ts[i] | ts[i + 1] << 8 | ts[i + 2] << 16 | ts[i + 3] << 24);
            }
            return output;
        }

        private static UInt32[] MD5_Trasform(UInt32[] x)
        {
            UInt32 a, b, c, d;
            for (int k = 0; k < x.Length; k += 16)
            {
                a = _a;
                b = _b;
                c = _c;
                d = _d;
                /* Raundas 1 */
                Ff(ref a, b, c, d, x[k + 0], S11, 0xd76aa478); /* 1 */
                Ff(ref d, a, b, c, x[k + 1], S12, 0xe8c7b756); /* 2 */
                Ff(ref c, d, a, b, x[k + 2], S13, 0x242070db); /* 3 */
                Ff(ref b, c, d, a, x[k + 3], S14, 0xc1bdceee); /* 4 */
                Ff(ref a, b, c, d, x[k + 4], S11, 0xf57c0faf); /* 5 */
                Ff(ref d, a, b, c, x[k + 5], S12, 0x4787c62a); /* 6 */
                Ff(ref c, d, a, b, x[k + 6], S13, 0xa8304613); /* 7 */
                Ff(ref b, c, d, a, x[k + 7], S14, 0xfd469501); /* 8 */
                Ff(ref a, b, c, d, x[k + 8], S11, 0x698098d8); /* 9 */
                Ff(ref d, a, b, c, x[k + 9], S12, 0x8b44f7af); /* 10 */
                Ff(ref c, d, a, b, x[k + 10], S13, 0xffff5bb1); /* 11 */
                Ff(ref b, c, d, a, x[k + 11], S14, 0x895cd7be); /* 12 */
                Ff(ref a, b, c, d, x[k + 12], S11, 0x6b901122); /* 13 */
                Ff(ref d, a, b, c, x[k + 13], S12, 0xfd987193); /* 14 */
                Ff(ref c, d, a, b, x[k + 14], S13, 0xa679438e); /* 15 */
                Ff(ref b, c, d, a, x[k + 15], S14, 0x49b40821); /* 16 */
                                                                /* Raundas 2 */
                Gg(ref a, b, c, d, x[k + 1], S21, 0xf61e2562); /* 17 */
                Gg(ref d, a, b, c, x[k + 6], S22, 0xc040b340); /* 18 */
                Gg(ref c, d, a, b, x[k + 11], S23, 0x265e5a51); /* 19 */
                Gg(ref b, c, d, a, x[k + 0], S24, 0xe9b6c7aa); /* 20 */
                Gg(ref a, b, c, d, x[k + 5], S21, 0xd62f105d); /* 21 */
                Gg(ref d, a, b, c, x[k + 10], S22, 0x2441453); /* 22 */
                Gg(ref c, d, a, b, x[k + 15], S23, 0xd8a1e681); /* 23 */
                Gg(ref b, c, d, a, x[k + 4], S24, 0xe7d3fbc8); /* 24 */
                Gg(ref a, b, c, d, x[k + 9], S21, 0x21e1cde6); /* 25 */
                Gg(ref d, a, b, c, x[k + 14], S22, 0xc33707d6); /* 26 */
                Gg(ref c, d, a, b, x[k + 3], S23, 0xf4d50d87); /* 27 */
                Gg(ref b, c, d, a, x[k + 8], S24, 0x455a14ed); /* 28 */
                Gg(ref a, b, c, d, x[k + 13], S21, 0xa9e3e905); /* 29 */
                Gg(ref d, a, b, c, x[k + 2], S22, 0xfcefa3f8); /* 30 */
                Gg(ref c, d, a, b, x[k + 7], S23, 0x676f02d9); /* 31 */
                Gg(ref b, c, d, a, x[k + 12], S24, 0x8d2a4c8a); /* 32 */
                                                                /* Raundas 3 */
                Hh(ref a, b, c, d, x[k + 5], S31, 0xfffa3942); /* 33 */
                Hh(ref d, a, b, c, x[k + 8], S32, 0x8771f681); /* 34 */
                Hh(ref c, d, a, b, x[k + 11], S33, 0x6d9d6122); /* 35 */
                Hh(ref b, c, d, a, x[k + 14], S34, 0xfde5380c); /* 36 */
                Hh(ref a, b, c, d, x[k + 1], S31, 0xa4beea44); /* 37 */
                Hh(ref d, a, b, c, x[k + 4], S32, 0x4bdecfa9); /* 38 */
                Hh(ref c, d, a, b, x[k + 7], S33, 0xf6bb4b60); /* 39 */
                Hh(ref b, c, d, a, x[k + 10], S34, 0xbebfbc70); /* 40 */
                Hh(ref a, b, c, d, x[k + 13], S31, 0x289b7ec6); /* 41 */
                Hh(ref d, a, b, c, x[k + 0], S32, 0xeaa127fa); /* 42 */
                Hh(ref c, d, a, b, x[k + 3], S33, 0xd4ef3085); /* 43 */
                Hh(ref b, c, d, a, x[k + 6], S34, 0x4881d05); /* 44 */
                Hh(ref a, b, c, d, x[k + 9], S31, 0xd9d4d039); /* 45 */
                Hh(ref d, a, b, c, x[k + 12], S32, 0xe6db99e5); /* 46 */
                Hh(ref c, d, a, b, x[k + 15], S33, 0x1fa27cf8); /* 47 */
                Hh(ref b, c, d, a, x[k + 2], S34, 0xc4ac5665); /* 48 */
                                                               /* Raundas 4 */
                Ii(ref a, b, c, d, x[k + 0], S41, 0xf4292244); /* 49 */
                Ii(ref d, a, b, c, x[k + 7], S42, 0x432aff97); /* 50 */
                Ii(ref c, d, a, b, x[k + 14], S43, 0xab9423a7); /* 51 */
                Ii(ref b, c, d, a, x[k + 5], S44, 0xfc93a039); /* 52 */
                Ii(ref a, b, c, d, x[k + 12], S41, 0x655b59c3); /* 53 */
                Ii(ref d, a, b, c, x[k + 3], S42, 0x8f0ccc92); /* 54 */
                Ii(ref c, d, a, b, x[k + 10], S43, 0xffeff47d); /* 55 */
                Ii(ref b, c, d, a, x[k + 1], S44, 0x85845dd1); /* 56 */
                Ii(ref a, b, c, d, x[k + 8], S41, 0x6fa87e4f); /* 57 */
                Ii(ref d, a, b, c, x[k + 15], S42, 0xfe2ce6e0); /* 58 */
                Ii(ref c, d, a, b, x[k + 6], S43, 0xa3014314); /* 59 */
                Ii(ref b, c, d, a, x[k + 13], S44, 0x4e0811a1); /* 60 */
                Ii(ref a, b, c, d, x[k + 4], S41, 0xf7537e82); /* 61 */
                Ii(ref d, a, b, c, x[k + 11], S42, 0xbd3af235); /* 62 */
                Ii(ref c, d, a, b, x[k + 2], S43, 0x2ad7d2bb); /* 63 */
                Ii(ref b, c, d, a, x[k + 9], S44, 0xeb86d391); /* 64 */
                _a += a;
                _b += b;
                _c += c;
                _d += d;
            }
            return new UInt32[] { _a, _b, _c, _d };
        }

        private static void Ff(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + F(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static UInt32 F(UInt32 x, UInt32 y, UInt32 z) { return (x & y) | (~x & z); }


        private static void Gg(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + G(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static UInt32 G(UInt32 x, UInt32 y, UInt32 z) { return (x & z) | (y & ~z); }


        private static void Hh(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + H(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static UInt32 H(UInt32 x, UInt32 y, UInt32 z) { return x ^ y ^ z; }

        private static void Ii(ref UInt32 a, UInt32 b, UInt32 c, UInt32 d, UInt32 mj, int s, UInt32 ti)
        {
            a = a + I(b, c, d) + mj + ti;
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static UInt32 I(UInt32 x, UInt32 y, UInt32 z) { return y ^ (x | ~z); }

        private static string ArrayToHexString(byte[] array)
        {
            string hexString = "";
            string format = "x2";
            foreach (byte b in array)
            {
                hexString += b.ToString(format);
            }
            return hexString;
        }
    }
}
