using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExternalCommunicationLibrary
{
    public class MessageParser
    {
        private readonly Regex _pattern = new Regex("Type:(?<type>[a-zA-Z]+)|ContentLength:(?<length>\\d+)");

        private MessageType _type;
        private int _bufferSize;
        private string _messageContent = "";
        private bool _headerCompleted;

        public delegate void MessageAvailableDelegate(Message m);

        public event MessageAvailableDelegate MessageAvailable;

        public MessageParser()
        {
            Reset();
        }

        public void FeedLine(string line)
        {
            if (_headerCompleted)
            {
                ReadContentLine(line);
            }
            else
            {
                ReadHeaderLine(line);
            }
        }

        public void Reset()
        {
            _type = MessageType.Null;
            _bufferSize = 0;
            _messageContent = "";
            _headerCompleted = false;
        }

        public void ReadHeaderLine(string line)
        {
            line = line.Trim();

            if (line.Length == 0 && _type != MessageType.Null)
            {
                _headerCompleted = true;

                if (_bufferSize <= 0)
                {
                    ComposeMessage();
                }
            }
            else if(line.Length > 0)
            {
                var m = _pattern.Match(line);

                if (!m.Success)
                {
                    throw new Exception("could not parse header line");
                }

                var groupType = m.Groups["type"];
                var groupLength = m.Groups["length"];

                if (groupLength.Success)
                {
                    _bufferSize = int.Parse(groupLength.Value);
                }

                if (!groupType.Success) return;

                var res = Enum.TryParse(groupType.Value, out _type);

                if (!res)
                {
                    throw new Exception("could not parse message type");
                }
            }
        }

        public void ReadContentLine(string line)
        {
            var buf = Math.Min(_bufferSize, line.Length);
            _messageContent += line.Substring(0, buf) + "\n";
            _bufferSize -= (buf + 1);

            if (_bufferSize <= 0)
            {
                ComposeMessage();
            }
        }

        public void ComposeMessage()
        {
            try
            {
                var t = TypedEnum.GetTypeValue(_type);

                if (t == null)
                {
                    throw new Exception("type not defined");
                }

                var instance = Activator.CreateInstance(t) as Message;

                if (instance == null)
                {
                    throw new Exception("could not initialize message instance");
                }

                instance.ParseMessage(_messageContent);

                OnMessageAvailable(instance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Reset();
            }
        }

        protected virtual void OnMessageAvailable(Message m)
        {
            MessageAvailable?.Invoke(m);
        }
    }
}
