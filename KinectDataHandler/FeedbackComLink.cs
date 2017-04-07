using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace KinectDataHandler
{
    class FeedbackComLink
    {
        private readonly Socket _serverSocket;
        private readonly Thread _serverSocketThread;
        private readonly List<Socket> _clientSockets = new List<Socket>();
        public readonly IPEndPoint _endPoint = new IPEndPoint(IPAddress.Any, 18283);

        public FeedbackComLink()
        {
            _serverSocket = new Socket(SocketType.Stream, ProtocolType.Tcp) {Blocking = true};
            _serverSocket.Bind(_endPoint);
            _serverSocketThread = new Thread(() =>
            {
                while (true)
                {
                    var s = _serverSocket.Accept();
                    _clientSockets.Add(s);
                    new Thread(() =>
                    {
                        Console.WriteLine(s.RemoteEndPoint.ToString() + " connected");
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
            _serverSocket.Shutdown(SocketShutdown.Both);
            _serverSocket.Close();
            _serverSocket.Dispose();
        }

        public bool SendTo(Socket s, string msg)
        {
            if (!s.Connected)
            {
                return false;
            }

            var packet = System.Text.Encoding.UTF8.GetBytes(msg);

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
            Socket[] clientSockets = _clientSockets.ToArray();
            
            for(var i = 0; i < clientSockets.Length; i++)
            {
                var socket = clientSockets[i];

                if (!SendTo(socket, msg))
                {
                    CloseClientConnection(socket);
                }
                else
                {
                    Console.WriteLine("Packet sent");
                }
                
            }

        }
    }
}
