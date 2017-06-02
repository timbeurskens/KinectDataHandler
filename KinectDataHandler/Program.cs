using System;
using System.Net;
using System.Threading;
using ExternalCommunicationLibrary;
using KinectDataHandler.BodyAnalyzer;
using KinectDataHandler.Properties;

namespace KinectDataHandler
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            var s = new Server(IPAddress.Any, 12345);
            while (true)
            {
                s.Send(new SimpleClientMessage(SocketClientMessageType.Ping, "ping"));
                Thread.Sleep(500);
            }

//            var kl = new KinectLink();
//            kl.Open();
//
//            var sm = new AnalyzerStateManager(kl);
//            ConsoleKeyInfo key;
//            do
//            {
//                key = Console.ReadKey();
//                switch (key.KeyChar)
//                {
//                    case 'r':
//                        sm.Reset();
//                        break;
//                }
//            } while (key.KeyChar != 'q');
//
//            //disposing
//            Console.WriteLine(Resources.Program_Main_Closing);
//            kl.Close();
//            sm.Dispose();
//            Console.WriteLine(Resources.Program_Main_Closed);

            return 0;
        }
    }
}