using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ExternalCommunicationLibrary
{
    public class Server : IDisposable
    {
        private readonly TcpListener _server;
        private bool _acceptingConnections = true;
        private readonly List<SocketConnection> _connections;


        public Server(IPAddress addr, int port)
        {
            _connections = new List<SocketConnection>();
            _server = new TcpListener(addr, port);
            _server.Start();

            var t = new Thread(() =>
            {
                while (_acceptingConnections)
                {
                    if (_server.Pending())
                    {
                        var socket = _server.AcceptTcpClient();

                        do
                        {
                            Thread.Sleep(10);
                        } while (!socket.Connected);

                        Console.WriteLine("Socket connected: " + socket.Client.RemoteEndPoint);

                        var sc = new SocketConnection(socket);

                        _connections.Add(sc);
                    }

                    Thread.Sleep(10);
                }
            });
            t.Start();

            Console.WriteLine("Now accepting connections...");
        }

        private void Send(int i, Message msg)
        {
            var s = _connections[i];

            if (!s.IsActive)
            {
                StopConnection(i);
                return;
            }

            try
            {
                s.Send(msg);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                StopConnection(i);
            }
        }

        public void Send(Message msg)
        {
            for (var i = _connections.Count - 1; i >= 0; i--)
            {
                Send(i, msg);
            }
        }

        private void StopConnection(int i)
        {
            var connection = _connections[i];
            connection.Close();
            connection.Dispose();
            _connections.RemoveAt(i);
            Console.WriteLine("Connection closed (" + i + ")");
        }

        public void Dispose()
        {
            for (var i = _connections.Count - 1; i >= 0; i--)
            {
                StopConnection(i);
            }
            _acceptingConnections = false;
            _server.Stop();
        }
    }
}