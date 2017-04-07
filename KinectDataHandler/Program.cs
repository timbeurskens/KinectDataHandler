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

            do
            {
                ;
            } while (Console.ReadKey().KeyChar != 'q');

            kl.Close();
        }
    }
}
