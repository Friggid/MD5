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
                        Console.WriteLine("MD5 reikšmė: " + Md5.Out(_byteArray));
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
                                sw.WriteLine(Md5.Out(_byteArray));
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
                else
                {
                    if (File.Exists(args[0]))
                    {
                        _byteArray = File.ReadAllBytes(args[0]);
                        Console.WriteLine();
                        Console.WriteLine("MD5 reikšmė: " + Md5.Out(_byteArray));
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
