using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalCommunicationLibrary.Messages
{
    class HandshakeMessage : Message
    {
        public override MessageType GetMessageType()
        {
            return MessageType.Handshake;
        }

        protected override string GetMessageContent()
        {
            return "handshake";
        }

        public override Message ParseMessage(string content)
        {
            return this;
        }
    }
}
