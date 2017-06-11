using System;
using System.Net;
using System.Threading;
using ExternalCommunicationLibrary;
using ExternalCommunicationLibrary.Messages;
using KinectDataHandler.BodyAnalyzer;
using KinectDataHandler.Properties;

namespace KinectDataHandler
{
    internal class Program
    {
        private static Server _server;

        private static int Main()
        {
            var i = 0;
            _server = new Server(IPAddress.Any, 12345);

            _server.MessageAvailable += S_MessageAvailable;

//            while (true)
//            {
//                s.Send(new SimpleMessage(MessageType.Ping, "ping"));
//                Thread.Sleep(1000);
//                s.Send(new ControlMessage(new Command(CommandType.ExerciseSelect, 0, 0)));
//                Thread.Sleep(1000);
//            }

            var kl = new KinectLink();
            kl.Open();

            var stateManager = new AnalyzerStateManager(kl, _server);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case 'r':
                        stateManager.Reset();
                        break;
                    case 's':
                        i++;
                        _server.Send(new ControlMessage(new Command(CommandType.ExerciseStatus, -1, i)));
                        break;
                    case 'i':
                        stateManager.BodyReset();
                        break;
                    case '1':
                        stateManager.StartSession();
                        break;
                    case '2':
                        stateManager.StopSession();
                        break;
                }
                Thread.Sleep(100);
            } while (key.KeyChar != 'q');

            //disposing
            Console.WriteLine(Resources.Program_Main_Closing);
            kl.Close();
            //sm.Dispose();
            _server.Dispose();
            Console.WriteLine(Resources.Program_Main_Closed);

            return 0;
        }

        private static void S_MessageAvailable(Message m)
        {
            Console.WriteLine(Resources.MessageBreak_Begin);
            Console.WriteLine(m.GetStringData());
            Console.WriteLine(Resources.MessageBreak_End);

            if (m.GetMessageType() == MessageType.Handshake)
            {
                _server.Send(new SimpleMessage(MessageType.Acknowledge, "ack"));
            }
        }
    }
}