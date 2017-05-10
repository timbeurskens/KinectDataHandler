using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalCommunicationLibrary
{
    public abstract class ClientMessage
    {
        protected abstract SocketClientMessageType GetMessageType();
        protected abstract string GetMessageContent();

        private string GetStringData()
        {
            var output = "";
            var content = GetMessageContent();
            output += "Type:" + GetMessageType() + "\n";
            output += "ContentLength:" + content.Length + "\n";
            output += "\n";
            output += content + "\n";
            return output;
        }

        public byte[] GetBuffer()
        {
            return Encoding.ASCII.GetBytes(GetStringData());
        }
    }

    public class SimpleClientMessage : ClientMessage
    {
        private readonly SocketClientMessageType _type;
        private readonly string _content;

        public SimpleClientMessage(SocketClientMessageType type, string content)
        {
            _type = type;
            _content = content;
        }

        protected override SocketClientMessageType GetMessageType()
        {
            return _type;
        }

        protected override string GetMessageContent()
        {
            return _content;
        }
    }
}
