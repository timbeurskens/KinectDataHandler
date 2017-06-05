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
