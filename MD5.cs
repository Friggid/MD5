using System.Collections;

namespace MD5
{
    public static class Md5
    {
        // Algoritmas veikias 128-bitu busenoje, 
        // algoritmas teigia, kad busena reikia padalinti i keturias dalis po 32 bitus.
        private static uint _a = 0x67452301;
        private static uint _b = 0xefcdab89;
        private static uint _c = 0x98badcfe;
        private static uint _d = 0x10325476;

        // Galima naudoti funkcija
        /*
            for i from 0 to 63
                K[i] := floor(232 × abs(sin(i + 1)))
            end for
        */
        // Arba galima nustatyti nustatyti jau suskaiciuotus K zodzius
        static readonly uint[] K = new uint[64]
        {
            0xd76aa478,0xe8c7b756,0x242070db,0xc1bdceee,
            0xf57c0faf,0x4787c62a,0xa8304613,0xfd469501,
            0x698098d8,0x8b44f7af,0xffff5bb1,0x895cd7be,
            0x6b901122,0xfd987193,0xa679438e,0x49b40821,
            0xf61e2562,0xc040b340,0x265e5a51,0xe9b6c7aa,
            0xd62f105d,0x2441453,0xd8a1e681,0xe7d3fbc8,
            0x21e1cde6,0xc33707d6,0xf4d50d87,0x455a14ed,
            0xa9e3e905,0xfcefa3f8,0x676f02d9,0x8d2a4c8a,
            0xfffa3942,0x8771f681,0x6d9d6122,0xfde5380c,
            0xa4beea44,0x4bdecfa9,0xf6bb4b60,0xbebfbc70,
            0x289b7ec6,0xeaa127fa,0xd4ef3085,0x4881d05,
            0xd9d4d039,0xe6db99e5,0x1fa27cf8,0xc4ac5665,
            0xf4292244,0x432aff97,0xab9423a7,0xfc93a039,
            0x655b59c3,0x8f0ccc92,0xffeff47d,0x85845dd1,
            0x6fa87e4f,0xfe2ce6e0,0xa3014314,0x4e0811a1,
            0xf7537e82,0xbd3af235,0x2ad7d2bb,0xeb86d391
        };

        // Konstantos nusakancios, kiek bitu reikes pasukti transformaciju metu per kiekviena raunda.
        // s[0..15]
        private const int S11 = 7;
        private const int S12 = 12;
        private const int S13 = 17;
        private const int S14 = 22;
        // s[16..31]
        private const int S21 = 5;
        private const int S22 = 9;
        private const int S23 = 14;
        private const int S24 = 20;
        // s[32..47]
        private const int S31 = 4;
        private const int S32 = 11;
        private const int S33 = 16;
        private const int S34 = 23;
        // s[48..63]
        private const int S41 = 6;
        private const int S42 = 10;
        private const int S43 = 15;
        private const int S44 = 21;

        // Gauname nuskaitytus is failo baitus kaip input
        public static string ComputeHash(byte[] input)
        {
            byte[] digest = Md5Array(input);
            return ArrayToHexString(digest);
        }

        private static byte[] Md5Array(byte[] input)
        {
            uint[] block = MD5_Append(input);
            uint[] bits = MD5_Trasform(block);

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

        private static uint[] MD5_Append(byte[] input)
        {
            int zeros;
            int ones = 1;
            int size;
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

            var N = (ulong)n * 8;
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

            uint[] output = new uint[size / 4];
            for (long i = 0, j = 0; i < size; j++, i += 4)
            {
                output[j] = (uint)(ts[i] | ts[i + 1] << 8 | ts[i + 2] << 16 | ts[i + 3] << 24);
            }
            return output;
        }

        private static uint[] MD5_Trasform(uint[] x)
        {
            for (int k = 0; k < x.Length; k += 16)
            {
                var a = _a;
                var b = _b;
                var c = _c;
                var d = _d;
                /* Raundas 1, transformacija 1 - 16 */
                FF(ref a, b, c, d, x[k + 0], S11, K[0]);
                FF(ref d, a, b, c, x[k + 1], S12, K[1]);
                FF(ref c, d, a, b, x[k + 2], S13, K[2]);
                FF(ref b, c, d, a, x[k + 3], S14, K[3]);
                FF(ref a, b, c, d, x[k + 4], S11, K[4]);
                FF(ref d, a, b, c, x[k + 5], S12, K[5]);
                FF(ref c, d, a, b, x[k + 6], S13, K[6]);
                FF(ref b, c, d, a, x[k + 7], S14, K[7]);
                FF(ref a, b, c, d, x[k + 8], S11, K[8]);
                FF(ref d, a, b, c, x[k + 9], S12, K[9]);
                FF(ref c, d, a, b, x[k + 10], S13, K[10]);
                FF(ref b, c, d, a, x[k + 11], S14, K[11]);
                FF(ref a, b, c, d, x[k + 12], S11, K[12]);
                FF(ref d, a, b, c, x[k + 13], S12, K[13]);
                FF(ref c, d, a, b, x[k + 14], S13, K[14]);
                FF(ref b, c, d, a, x[k + 15], S14, K[15]);
                /* Raundas 2, transformacija 17 - 32 */
                GG(ref a, b, c, d, x[k + 1], S21, K[16]);
                GG(ref d, a, b, c, x[k + 6], S22, K[17]);
                GG(ref c, d, a, b, x[k + 11], S23, K[18]);
                GG(ref b, c, d, a, x[k + 0], S24, K[19]);
                GG(ref a, b, c, d, x[k + 5], S21, K[20]);
                GG(ref d, a, b, c, x[k + 10], S22, K[21]);
                GG(ref c, d, a, b, x[k + 15], S23, K[22]);
                GG(ref b, c, d, a, x[k + 4], S24, K[23]);
                GG(ref a, b, c, d, x[k + 9], S21, K[24]);
                GG(ref d, a, b, c, x[k + 14], S22, K[25]);
                GG(ref c, d, a, b, x[k + 3], S23, K[26]);
                GG(ref b, c, d, a, x[k + 8], S24, K[27]);
                GG(ref a, b, c, d, x[k + 13], S21, K[28]);
                GG(ref d, a, b, c, x[k + 2], S22, K[29]);
                GG(ref c, d, a, b, x[k + 7], S23, K[30]);
                GG(ref b, c, d, a, x[k + 12], S24, K[31]);
                /* Raundas 3, transformacija 33 - 48 */
                HH(ref a, b, c, d, x[k + 5], S31, K[32]);
                HH(ref d, a, b, c, x[k + 8], S32, K[33]);
                HH(ref c, d, a, b, x[k + 11], S33, K[34]);
                HH(ref b, c, d, a, x[k + 14], S34, K[35]);
                HH(ref a, b, c, d, x[k + 1], S31, K[36]);
                HH(ref d, a, b, c, x[k + 4], S32, K[37]);
                HH(ref c, d, a, b, x[k + 7], S33, K[38]);
                HH(ref b, c, d, a, x[k + 10], S34, K[39]);
                HH(ref a, b, c, d, x[k + 13], S31, K[40]);
                HH(ref d, a, b, c, x[k + 0], S32, K[41]);
                HH(ref c, d, a, b, x[k + 3], S33, K[42]);
                HH(ref b, c, d, a, x[k + 6], S34, K[43]);
                HH(ref a, b, c, d, x[k + 9], S31, K[44]);
                HH(ref d, a, b, c, x[k + 12], S32, K[45]);
                HH(ref c, d, a, b, x[k + 15], S33, K[46]);
                HH(ref b, c, d, a, x[k + 2], S34, K[47]);
                /* Raundas 4, transformacija 49 - 64 */
                II(ref a, b, c, d, x[k + 0], S41, K[48]);
                II(ref d, a, b, c, x[k + 7], S42, K[49]);
                II(ref c, d, a, b, x[k + 14], S43, K[50]);
                II(ref b, c, d, a, x[k + 5], S44, K[51]);
                II(ref a, b, c, d, x[k + 12], S41, K[52]);
                II(ref d, a, b, c, x[k + 3], S42, K[53]);
                II(ref c, d, a, b, x[k + 10], S43, K[54]);
                II(ref b, c, d, a, x[k + 1], S44, K[55]);
                II(ref a, b, c, d, x[k + 8], S41, K[56]);
                II(ref d, a, b, c, x[k + 15], S42, K[57]);
                II(ref c, d, a, b, x[k + 6], S43, K[58]);
                II(ref b, c, d, a, x[k + 13], S44, K[59]);
                II(ref a, b, c, d, x[k + 4], S41, K[60]);
                II(ref d, a, b, c, x[k + 11], S42, K[61]);
                II(ref c, d, a, b, x[k + 2], S43, K[62]);
                II(ref b, c, d, a, x[k + 9], S44, K[63]);
                _a += a;
                _b += b;
                _c += c;
                _d += d;
            }
            return new uint[] { _a, _b, _c, _d };
        }

        private static void FF(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + F(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas y kaire
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint F(uint x, uint y, uint z) { return (x & y) | (~x & z); }


        private static void GG(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + G(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas y kaire
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint G(uint x, uint y, uint z) { return (x & z) | (y & ~z); }


        private static void HH(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + H(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas y kaire
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint H(uint x, uint y, uint z) { return x ^ y ^ z; }

        private static void II(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + I(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas y kaire
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint I(uint x, uint y, uint z) { return y ^ (x | ~z); }

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
