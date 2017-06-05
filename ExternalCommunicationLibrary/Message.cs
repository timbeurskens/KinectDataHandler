namespace ExternalCommunicationLibrary
{
    public abstract class Message
    {
        public abstract MessageType GetMessageType();
        protected abstract string GetMessageContent();
        public abstract Message ParseMessage(string content);

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
}