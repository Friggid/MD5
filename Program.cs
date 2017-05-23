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
        public static void Main()
        {
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;

            Console.WriteLine("Atidaromas failo pasirinkimas...");
            Console.WriteLine();

            try
            {
                _fileDialog = new OpenFileDialog();
                _fileDialog.ShowDialog();
                Console.WriteLine("Pasirinktas failas: " + _fileDialog.SafeFileName);
                Console.WriteLine();

                _byteArray = File.ReadAllBytes(_fileDialog.FileName);

                Helpers.PrintByteArray(_byteArray);
                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Pasirinktas blogas failas..." + "\n" + e.Message);
                Helpers.DelayedShutdown();
            }

            var userInput = 0;
            while (userInput != 2)
            {
                userInput = Helpers.DisplayMenu();

                if (userInput == 0)
                {
                    Console.WriteLine("-----------------------");
                    Console.WriteLine("Tokio pasirinkimo nėra!");
                    Console.WriteLine("-----------------------");
                }
                else if (userInput == 1)
                {
                    Console.WriteLine();

                    var userWriteout = Helpers.DisplayWrite();

                    while (userWriteout != 3)
                    {
                        Md5 md5 = new Md5();
                        md5.ValueAsByte = _byteArray;

                        if (userWriteout == 3)
                        {
                            Helpers.DelayedShutdown();
                        }
                        else if (userWriteout == 1)
                        {
                            // Write to console
                            Console.WriteLine("MD5 reikšmė: " + md5.FingerPrint);
                            Console.WriteLine();
                            Console.WriteLine("Spauskite ENTER, kad uždarytumėte programą.");
                            Console.ReadLine();
                            break;
                        }
                        else if (userWriteout == 2)
                        {
                            // Write to file
                            try
                            {
                                ostrm = new FileStream(".\\test.txt", FileMode.OpenOrCreate, FileAccess.Write);
                                writer = new StreamWriter(ostrm);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Neišeina atidaryti failo rašymui");
                                Console.WriteLine(e.Message);
                                return;
                            }

                            Console.SetOut(writer);
                            Console.WriteLine("MD5 reikšmė: " + md5.FingerPrint);
                            Console.SetOut(oldOut);

                            writer.Close();
                            ostrm.Close();

                            Console.WriteLine();
                            Console.WriteLine("Spauskite ENTER, kad uždarytumėte programą.");
                            Console.ReadLine();
                            break;
                        }

                        Console.WriteLine("-----------------------");
                        Console.WriteLine("Tokio pasirinkimo nėra!");
                        Console.WriteLine("-----------------------");

                        userWriteout = Helpers.DisplayWrite();
                    }

                    Helpers.DelayedShutdown();
                }
            }

            Helpers.DelayedShutdown();
        }
    }
}
