using System;
using System.Windows.Forms;
using KinectDataHandler.BodyAnalyzer;
using KinectDataHandler.Properties;

namespace KinectDataHandler
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var kl = new KinectLink();
            kl.Open();

            var sm = new AnalyzerStateManager(kl);
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'r':
                        sm.Reset();
                        break;
                }
            } while (key.KeyChar != 'q');

            //disposing
            Console.WriteLine(Resources.Program_Main_Closing);
            kl.Close();
            sm.Dispose();
            Console.WriteLine(Resources.Program_Main_Closed);

            return 0;
        }
    }
}
