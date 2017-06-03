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
        private readonly TcpClient _client;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private readonly BackgroundWorker _senderWorker = new BackgroundWorker();
        private readonly List<Message> _messageQueue = new List<Message>();

        public bool IsActive { get; private set; }

        public SocketConnection(TcpClient client)
        {
            _client = client;
            _reader = new StreamReader(client.GetStream(), Encoding.ASCII);
            _writer = new StreamWriter(client.GetStream(), Encoding.ASCII);

            _senderWorker.DoWork += SenderWorker_DoWork;

            IsActive = false;

            var socketReceive = new Thread(() =>
            {
                while (client.Connected)
                {
                    IsActive = true;

                    string line = null;

                    try
                    {
                        line = _reader.ReadLine();
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e);
                        Close();
                    }

                    //todo: parse message contents and return valid message object
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
            if (!_client.Connected) return;

            //todo: fix modification issue
            foreach (var t in _messageQueue)
            {
                var data = t.GetStringData();
                //Console.WriteLine(data);

                try
                {
                    _writer.Write(data);
                }
                catch (IOException exception)
                {
                    Console.WriteLine(exception);
                    Close();
                }
            }
            _writer.Flush();
        }

        public void Send(Message msg)
        {
            if (!_client.Connected)
            {
                return;
            }
            _messageQueue.Add(msg);
            if(!_senderWorker.IsBusy)
                _senderWorker.RunWorkerAsync();
        }

        public void Close()
        {
            try
            {
                _client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable) _client)?.Dispose();
            _reader?.Dispose();
            _writer?.Dispose();
        }
    }
}