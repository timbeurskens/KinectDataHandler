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
        private readonly List<Socket> _connections;
        

        public Server(IPAddress addr, int port)
        {
            _connections = new List<Socket>();
            _server = new TcpListener(addr, port);
            _server.Start();

            var t = new Thread((() =>
            {
                while (_acceptingConnections)
                {
                    if (_server.Pending())
                    {
                        var socket = _server.AcceptSocket();

                        do
                        {
                            Thread.Sleep(10);
                        } while (!socket.Connected);

                        Console.WriteLine("Socket connected: " + socket.RemoteEndPoint);

                        var socketReceive = new Thread((() =>
                        {
                            while (socket.Connected)
                            {
                                Thread.Sleep(10);
                            }
                        }));
                        socketReceive.Start();
                       
                        _connections.Add(socket);
                    }

                    Thread.Sleep(10);
                }
            }));
            t.Start();
            
        }

        private void Send(int i, ClientMessage msg)
        {
            var s = _connections[i];
            try
            {
                s.Send(msg.GetBuffer());
            }
            catch (SocketException e)
            {
                Console.WriteLine(e);
                StopConnection(i);
            }
        }

        public void Send(ClientMessage msg)
        {
            for (var i = _connections.Count - 1; i >= 0; i--)
            {
                Send(i, msg);
            }
        }

        private void StopConnection(int i)
        {
            var connection = _connections[i];
            connection.Shutdown(SocketShutdown.Both);
            connection.Close();
            _connections.RemoveAt(i);
            Console.WriteLine("Connection closed");
        }
        
        public void Dispose()
        {
            for(var i = _connections.Count - 1; i >= 0; i--)
            {
                StopConnection(i);
            }
            _acceptingConnections = false;
            _server.Stop();
        }
    }
}
