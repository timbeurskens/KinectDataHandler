using System;
using KinectDataHandler.BodyAnalyzer;

namespace KinectDataHandler
{
    internal class Program
    {
        private static void Main(string[] args)
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
                    default:
                        break;
                }
            } while (key.KeyChar != 'q');

            kl.Close();
        }
    }
}
