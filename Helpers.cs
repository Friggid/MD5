using System;
using System.Text;
using System.Threading;

namespace MD5
{
    public static class Helpers
    {
        //Programos uzdarymas su delay ir animacija
        public static void DelayedShutdown()
        {
            Console.WriteLine();
            Console.Write("Programa uždaroma");
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(600);
                Console.Write(".");
            }
            Environment.Exit(0);
        }
    }
}
