﻿using System;
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

            s.MessageAvailable += S_MessageAvailable;

//            while (true)
//            {
//                s.Send(new SimpleMessage(MessageType.Ping, "ping"));
//                Thread.Sleep(1000);
//                s.Send(new ControlMessage(new Command(CommandType.ExerciseSelect, 0, 0)));
//                Thread.Sleep(1000);
//            }

            var kl = new KinectLink();
            kl.Open();

            var sm = new AnalyzerStateManager(kl, s);
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
                Thread.Sleep(100);
            } while (key.KeyChar != 'q');

            //disposing
            Console.WriteLine(Resources.Program_Main_Closing);
            kl.Close();
            //sm.Dispose();
            s.Dispose();
            Console.WriteLine(Resources.Program_Main_Closed);

            return 0;
        }

        private static void S_MessageAvailable(Message m)
        {
            Console.WriteLine(Resources.MessageBreak);
            Console.WriteLine(m.GetStringData());
            Console.WriteLine(Resources.MessageBreak);
        }
    }
}