using System;
using System.IO;

namespace MD5
{
    static class Program
    {
        static byte[] _byteArray;

        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (args[0] == "0")
                {
                    if (File.Exists(args[1]))
                    {
                        _byteArray = File.ReadAllBytes(args[1]);
                        Console.WriteLine();
                        Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Įvesties failas neegzistuoja");
                    }
                }
                else if (args[0] == "1")
                {
                    if (File.Exists(args[1]))
                    {
                        _byteArray = File.ReadAllBytes(args[1]);

                        if (args.Length > 2)
                        {
                            string path = @".\" + args[2];

                            using (StreamWriter sw = File.AppendText(path))
                            {
                                sw.WriteLine(Md5.ComputeHash(_byteArray));
                                Console.WriteLine();
                                Console.WriteLine("MD5 reikšmė įvesta į failą: " + args[2]);
                                sw.Close();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Išvesties failas neegzistuoja");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Įvesties failas neegzistuoja");
                    }
                }
                else if (args[0] == "2")
                {
                    if (File.Exists(args[1]))
                    {
                        var text = File.ReadAllText(args[1]);

                        if (text == "abc")
                        {
                            _byteArray = File.ReadAllBytes(args[1]);

                            Console.WriteLine("This is a test vector.");
                            Console.WriteLine();
                            Console.WriteLine("Test text: " + text);
                            Console.WriteLine();
                            Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                            Console.WriteLine("Test vector: " + "900150983CD24FB0D6963F7D28E17F72".ToLower());
                        }
                        else if (text == "The quick brown fox jumps over the lazy dog")
                        {
                            _byteArray = File.ReadAllBytes(args[1]);
                            Console.WriteLine("This is a test vector.");
                            Console.WriteLine();
                            Console.WriteLine("Test text: " + text);
                            Console.WriteLine();
                            Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                            Console.WriteLine("Test vector: 9e107d9d372bb6826bd81d3542a419d6");
                        }
                        else if (text == "")
                        {
                            _byteArray = File.ReadAllBytes(args[1]);
                            Console.WriteLine("This is a test vector.");
                            Console.WriteLine();
                            Console.WriteLine("Test text: " + text);
                            Console.WriteLine();
                            Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                            Console.WriteLine("Test vector: d41d8cd98f00b204e9800998ecf8427e");
                        }
                        else if (text == "12345678901234567890123456789012345678901234567890123456789012345678901234567890")
                        {
                            _byteArray = File.ReadAllBytes(args[1]);
                            Console.WriteLine("This is a test vector.");
                            Console.WriteLine();
                            Console.WriteLine("Test text: " + text);
                            Console.WriteLine();
                            Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                            Console.WriteLine("Test vector: 57edf4a22be3c955ac49da2e2107b67a");
                        }
                        else if (text == "abcdbcdecdefdefgefghfghighijhijkijkljklmklmnlmnomnopnopq")
                        {
                            _byteArray = File.ReadAllBytes(args[1]);
                            Console.WriteLine("This is a test vector.");
                            Console.WriteLine();
                            Console.WriteLine("Test text: " + text);
                            Console.WriteLine();
                            Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                            Console.WriteLine("Test vector: " + "8215EF0796A20BCAAAE116D3876C664A".ToLower());
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Tokio test vector nėra.");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Įvesties failas neegzistuoja");
                    }
                }
                else
                {
                    if (File.Exists(args[0]))
                    {
                        _byteArray = File.ReadAllBytes(args[0]);
                        Console.WriteLine();
                        Console.WriteLine("MD5 reikšmė: " + Md5.ComputeHash(_byteArray));
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Įvesties failas neegzistuoja");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
