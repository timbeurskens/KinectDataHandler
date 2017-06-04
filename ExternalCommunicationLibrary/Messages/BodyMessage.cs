namespace ExternalCommunicationLibrary.Messages
{
    public class BodyMessage : Message
    {
        private string _serializedBody;

        public BodyMessage(string serializedBody)
        {
            _serializedBody = serializedBody;
        }

        public BodyMessage()
        {
            
        }

        public override MessageType GetMessageType()
        {
            return MessageType.Body;
        }

        protected override string GetMessageContent()
        {
            return _serializedBody;
        }

        public override Message ParseMessage(string content)
        {
            _serializedBody = content;
            return this;
        }
    }
}
