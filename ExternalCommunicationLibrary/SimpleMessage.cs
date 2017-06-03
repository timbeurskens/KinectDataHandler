namespace ExternalCommunicationLibrary
{
    public class SimpleMessage : Message
    {
        private readonly MessageType _type;
        private readonly string _content;

        public SimpleMessage(MessageType type, string content)
        {
            _type = type;
            _content = content;
        }

        public SimpleMessage()
        {
            _type = MessageType.Null;
            _content = "";
        }

        protected override MessageType GetMessageType()
        {
            return _type;
        }

        protected override string GetMessageContent()
        {
            return _content;
        }

        public override Message ParseMessage(string content)
        {
            return this;
        }
    }
}