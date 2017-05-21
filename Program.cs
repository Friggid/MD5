using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace MD5
{
    class Program
    {
        private static byte[] _byteArray = null;
        private static OpenFileDialog _fileDialog;

        [STAThread]
        static void Main()
        {
            Console.WriteLine("Opening file selection...");
            Console.WriteLine();

            try
            {
                _fileDialog = new OpenFileDialog();
                _fileDialog.ShowDialog();
                Console.WriteLine("Selected file: " + _fileDialog.SafeFileName);
                Console.WriteLine();

                _byteArray = File.ReadAllBytes(_fileDialog.FileName);

                Helpers.PrintByteArray(_byteArray);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Huston, we have a problem..." + "\n" + e.Message);
                Helpers.DelayedShutdown();
            }

            int userInput = 0;
            while (userInput != 2)
            {
                userInput = Helpers.DisplayMenu();

                if (userInput == 0)
                {
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Wrong choice!");
                    Console.WriteLine("-----------------------");
                }
                else if (userInput == 1)
                {
                    Console.WriteLine();
                    Console.WriteLine("Intense calculations...");
                    Console.WriteLine();

                    MD5 md5 = new MD5();

                    md5.ValueAsByte = _byteArray;
                    Console.WriteLine("MD5 hash: " + md5.FingerPrint);

                    //Console.WriteLine(Md5.OutString(_byteArray.ToString()));
                    Console.ReadLine();
                    break;
                }
            }

            Helpers.DelayedShutdown();
        }
    }
}
