using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MD5
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Opening file selection...");
            Console.WriteLine();

            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.ShowDialog();
                Console.WriteLine("Selected file: " + fileDialog.FileName);
                Console.WriteLine();

                byte[] byteArray = File.ReadAllBytes(fileDialog.FileName);

                PrintByteArray(byteArray);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Huston, we have a problem..." + "\n" + e.Message);
                DelayedShutdown();
            }

            int userInput = 0;
            while (userInput != 2)
            {
                userInput = DisplayMenu();
            }

            DelayedShutdown();
        }

        

        //Rodyti pasirinkimu meniu 
        private static int DisplayMenu()
        {
            Console.WriteLine("Pasirinkite sifravimo algoritma ir spauskite ENTER: ");
            Console.WriteLine();
            Console.WriteLine("1. MD5");
            Console.WriteLine("2. Exit");
            Console.Write("Pasirinkimas: ");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }

        //Bitu israsymas is nuskaityto failo y string 
        private static void PrintByteArray(byte[] bytes)
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
        private static void Loading()
        {
            Console.Write("Loading");
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(1000);
                Console.Write(".");
            }
        }

        //Programos uzdarymas
        static void DelayedShutdown()
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
        static void Shutdown()
        {
            Console.WriteLine();
            Console.WriteLine("Shutting down...");
            Environment.Exit(0);
        }
    }
}
