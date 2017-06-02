namespace ExternalCommunicationLibrary
{
    public abstract class ClientMessage
    {
        protected abstract SocketClientMessageType GetMessageType();
        protected abstract string GetMessageContent();

        public string GetStringData()
        {
            var output = "";
            var content = GetMessageContent();
            output += "Type:" + GetMessageType() + "\n";
            output += "ContentLength:" + content.Length + "\n";
            output += "\n";
            output += content + "\n";
            return output;
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