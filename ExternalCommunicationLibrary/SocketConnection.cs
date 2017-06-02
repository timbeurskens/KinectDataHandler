using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ExternalCommunicationLibrary
{
    class SocketConnection : IDisposable
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private BackgroundWorker senderWorker = new BackgroundWorker();
        private List<ClientMessage> messageQueue = new List<ClientMessage>();

        public bool IsActive { get; private set; }

        public SocketConnection(TcpClient client)
        {
            this.client = client;
            reader = new StreamReader(client.GetStream(), Encoding.ASCII);
            writer = new StreamWriter(client.GetStream(), Encoding.ASCII);

            senderWorker.DoWork += SenderWorker_DoWork;

            IsActive = false;

            var socketReceive = new Thread(() =>
            {
                while (client.Connected)
                {
                    IsActive = true;

                    string line = null;

                    try
                    {
                        line = reader.ReadLine();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e);
                        Close();
                    }

                    if (line != null)
                    {
                        Console.WriteLine(line);
                    }
                    Thread.Sleep(10);
                }

                try
                {
                    client.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("Disconnected");
                IsActive = false;
            });
            socketReceive.Start();
        }

        private void SenderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!client.Connected) return;

            foreach (var t in messageQueue)
            {
                var data = t.GetStringData();
                //Console.WriteLine(data);

                try
                {
                    writer.Write(data);
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception);
                    Close();
                }
            }
            writer.Flush();
        }

        public void Send(ClientMessage msg)
        {
            if (!client.Connected)
            {
                return;
            }
            messageQueue.Add(msg);
            senderWorker.RunWorkerAsync();
        }

        public void Close()
        {
            try
            {
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable) client)?.Dispose();
            reader?.Dispose();
            writer?.Dispose();
        }
    }
}