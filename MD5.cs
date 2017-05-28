using System.Collections;

namespace MD5
{
    // Algoritmas paremtas RFC 1321 ir Wikipedia pseudo implementacija
    // RFC 1321 - https://tools.ietf.org/html/rfc1321
    // Wikipedia MD5 - https://en.wikipedia.org/wiki/MD5#Algorithm
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

        // Gauname nuskaitytus is failo baitus kaip input masyva
        public static string ComputeHash(byte[] input)
        {
            byte[] digest = MainCalc(input);
            return ArrayToHexString(digest);
        }

        // Perduodame pagrindiam skaičiavimų metodui nuskaitytą baitų masyvą.
        // Pirmiausia bus vykdomas Append metodas.
        // Toliau bus vykdomas Transformacijų metodas.
        // Galiausiai sutvarkytas masyvas bus perduotas atgal į ComputeHash metodą.
        private static byte[] MainCalc(byte[] input)
        {
            //var shit = TestAppend(input);
            uint[] block = Append(input);
            uint[] bits = Trasform(block);

            // Sukuriame tuščia byte masyvą išvedimui. Kuriam paduodame keturias transformuotas reikšmes.
            /*
             * word A: 01 23 45 67
             * word B: 89 ab cd ef
             * word C: fe dc ba 98
             * word D: 76 54 32 10
             * */
             // Sukame masyvą 4 kartus po keturis elementus ir viską išrašome į output bitais.
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

        // Input yra padidinamas taip, kad jo ilgis butu lygus 448 % 512.
        // Padidinimas vyksta visada, net kai ilgis iškart yra lygus 448 % 512.
        private static uint[] Append(byte[] input)
        {
            // Cia bus laikomas padidintas input masyvas.
            int size;
            // Nuskaitomas paduoto input masyvo dydis.
            int n = input.Length;
            // Liekana dalijant input masyvo dydį.
            // Ją naudoju naudosime tam, kad nusakyti kokio dydžio išeities masyvas turėtų būti.
            // Pvz jeigu liekana yra mažesnė nei 56 tilpsime į 64 baitų masyvą.
            // Jeigu liekana didesnė už 56 tilpsime į 128 ar 256 ir t.t. masyvą.
            int m = n % 64;

            if (m < 56)
            {
                size = n - m + 64;
            }
            else
            {
                size = n + 64 - m + 64;
            }

            ArrayList paddedInput = new ArrayList(input);

            // Vienetas yra pridedamas į input masyvo pabaigą.
            paddedInput.Add((byte)0x80);

            // Po vieneto yra pridedama tiek nuliu, kiek reikia, kad
            // input masyvo ilgis taptų lygus 448 % 512.
            while (paddedInput.Count % 64 != 56)
            {
                paddedInput.Add((byte)0x00);
            }

            // Prie praeito padding'o pridedamas 64 bitų nepakeisto dydžio input masyvas, kurio ilgis, kaip prieš padding'ą.
            var oldSize = (ulong)n * 8;
            byte h1 = (byte)(oldSize & 0xFF);
            byte h2 = (byte)((oldSize >> 8) & 0xFF);
            byte h3 = (byte)((oldSize >> 16) & 0xFF);
            byte h4 = (byte)((oldSize >> 24) & 0xFF);
            byte h5 = (byte)((oldSize >> 32) & 0xFF);
            byte h6 = (byte)((oldSize >> 40) & 0xFF);
            byte h7 = (byte)((oldSize >> 48) & 0xFF);
            byte h8 = (byte)(oldSize >> 56);
            paddedInput.Add(h1);
            paddedInput.Add(h2);
            paddedInput.Add(h3);
            paddedInput.Add(h4);
            paddedInput.Add(h5);
            paddedInput.Add(h6);
            paddedInput.Add(h7);
            paddedInput.Add(h8);
            byte[] paddingComplete = (byte[])paddedInput.ToArray(typeof(byte));

            uint[] output = new uint[size / 4];
            for (long i = 0, j = 0; i < size; j++, i += 4)
            {
                output[j] = (uint)(paddingComplete[i] | paddingComplete[i + 1] << 8 | paddingComplete[i + 2] << 16 | paddingComplete[i + 3] << 24);
            }

            // Rezultate turime naują žinutę, kurios ilgis yra tam tikras kiekis 16 (32 bitų) žodžių.
            return output;
        }

        // Transformacijos funkcija, kuriai paduodamas padidintas input masyvas.
        // Toliau yra vykdomos transformacijos su paduotu masyvu. 
        private static uint[] Trasform(uint[] appendedInputBlock)
        {
            var x = appendedInputBlock;

            for (int k = 0; k < x.Length; k += 16)
            {
                // Gauname statinius šešiolikaitnius žodžius, kuriuos naudosime bitų maskavimui.
                var a = _a;
                var b = _b;
                var c = _c;
                var d = _d;

                // Kiekvieno transformacijos raundo metu paduodame statinius kintamuosius(a, b, c,d),
                // kartu su padidintu input masyvu(x). Kiek kartų reikės sukti - S kintamieji.
                // Ir taip pat paduodame konstantas žodžių, kurie maskuoja elementus - K masyvas. 

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

        // Aprašomos keturios funkcijos F, G, H, I, 
        // kurios priima tris 32 bitų žodžius, ir kartu išveda vieną 32 bitų žodį.
        /*
         * F(X,Y,Z) = XY v not(X) Z
         * G(X,Y,Z) = XZ v Y not(Z)
         * H(X,Y,Z) = X xor Y xor Z
         * I(X,Y,Z) = Y xor (X v not(Z))
         */
        private static void FF(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            // Šis funkcionalumas aprašytas RFC 1321, kaip
            // a = b + ((a + F(b,c,d) + X[k] + T[i]) <<< s)
            a = a + F(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas į kairę(leftRotate)
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint F(uint x, uint y, uint z) { return (x & y) | (~x & z); }


        private static void GG(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + G(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas į kairę(leftRotate)
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint G(uint x, uint y, uint z) { return (x & z) | (y & ~z); }


        private static void HH(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + H(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas į kairę(leftRotate)
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint H(uint x, uint y, uint z) { return x ^ y ^ z; }

        private static void II(ref uint a, uint b, uint c, uint d, uint x, int s, uint t)
        {
            a = a + I(b, c, d) + x + t;
            // Dokumentacijoje nurodytas pasukimas į kairę(leftRotate)
            a = a << s | a >> (32 - s);
            a += b;
        }

        private static uint I(uint x, uint y, uint z) { return y ^ (x | ~z); }

        // Paduodamas jau pertvarkytas input masyvas ir pagal pasirinktą formatą (x2 - mažosiomis raidėmis),
        // išvedamas hash rezultatas atgal į main funkciją.
        private static string ArrayToHexString(byte[] digestedInput)
        {
            string hexString = "";
            // x(šešioliktainis) 2(dvi raidės) išveda string'ą, kaip dvi mažasias šešioliktaines raides.
            string format = "x2";

            // Einame per sutvarkyta masyva 
            foreach (byte b in digestedInput)
            {
                hexString += b.ToString(format);
            }

            return hexString;
        }
    }
}
