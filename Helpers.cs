using System;
using System.Text;
using System.Threading;

namespace MD5
{
    public static class Helpers
    {
        //Rodyti pasirinkimu meniu 
        public static int DisplayMenu()
        {
            Console.WriteLine("Pasirinkite sifravimo algoritma ir spauskite ENTER: ");
            Console.WriteLine();
            Console.WriteLine("1. MD5");
            Console.WriteLine("2. Exit");
            Console.WriteLine();
            Console.Write("Pasirinkimas: ");
            var result = Console.ReadLine();

            if (result == "1")
            {
                return Convert.ToInt32(result);
            }
            if (result == "2")
            {
                return 2;
            }

            return 0;
        }

        //Bitu israsymas is nuskaityto failo y string 
        public static void PrintByteArray(byte[] bytes)
        {
            var sb = new StringBuilder("Byte array from file: [] { ");
            foreach (var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");
            Console.WriteLine(sb.ToString());
        }

        //Besikraunantis uzrasas
        public static void Loading()
        {
            Console.Write("Loading");
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
        }

        //Programos uzdarymas su delay ir animacija
        public static void DelayedShutdown()
        {
            Console.WriteLine();
            Console.Write("Shutting down");
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(600);
                Console.Write(".");
            }
            Environment.Exit(0);
        }

        //Programos uzdarymas
        public static void Shutdown()
        {
            Console.WriteLine();
            Console.WriteLine("Shutting down...");
            Environment.Exit(0);
        }
    }
}
