using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using KinectDataHandler.Properties;

namespace KinectDataHandler
{
    internal class FeedbackComLink : IDisposable
    {
        private readonly Socket _serverSocket;
        private readonly Thread _serverSocketThread;
        private readonly List<Socket> _clientSockets = new List<Socket>();
        public readonly IPEndPoint EndPoint = new IPEndPoint(IPAddress.Any, 18283);

        public FeedbackComLink()
        {
            _serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp) {Blocking = true};
            _serverSocket.Bind(EndPoint);
            _serverSocketThread = new Thread(() =>
            {
                while (true)
                {
                    var s = _serverSocket.Accept();
                    _clientSockets.Add(s);
                    new Thread(() =>
                    {
                        Console.WriteLine(Resources.FeedbackComLink_connected, s.RemoteEndPoint);
                        while (s.Connected)
                        {
                        }
                        CloseClientConnection(s);
                    }).Start();
                }
            });
        }

        public void Open()
        {
            _serverSocket.Listen(100);
            _serverSocketThread.Start();
        }

        public void Close()
        {
            foreach (var socket in _clientSockets)
            {
                CloseClientConnection(socket);
            }
            _serverSocketThread.Abort();
            try
            {
                _serverSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            _serverSocket.Close();
            _serverSocket.Dispose();
        }

        public bool SendTo(Socket s, string msg)
        {
            if (!s.Connected)
            {
                return false;
            }

            var packet = Encoding.UTF8.GetBytes(msg);

            try
            {
                s.Send(packet);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void CloseClientConnection(Socket s)
        {
            if (s.Connected)
            {
                s.Shutdown(SocketShutdown.Both);
                s.Close();
                s.Dispose();
            }
            _clientSockets.Remove(s);
        }

        public void SendToAll(string msg)
        {
            var clientSockets = _clientSockets.ToArray();

            foreach (var socket in clientSockets)
            {
                if (!SendTo(socket, msg))
                {
                    CloseClientConnection(socket);
                }
                else
                {
                    Console.WriteLine(Resources.FeedbackComLink_SendToAll_Packet_sent);
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}