using System;
using System.IO;

namespace ExternalCommunicationLibrary.Messages
{
    public class ControlMessage : Message
    {
        public Command Command;

        public ControlMessage(Command command)
        {
            Command = command;
        }

        public ControlMessage() : this(new Command(CommandType.Null, 0, 0))
        {
        }

        public override MessageType GetMessageType()
        {
            return MessageType.Control;
        }

        protected override string GetMessageContent()
        {
            return Command.ToString();
        }

        public override Message ParseMessage(string content)
        {
            var sr = new StringReader(content);

            string line;

            var t = CommandType.Null;
            int? id = null;
            double? val = null;

            while ((line = sr.ReadLine()) != null)
            {
                if (t == CommandType.Null)
                {
                    Enum.TryParse(line, out t);
                }
                else if (id == null)
                {
                    id = int.Parse(line);
                }
                else if (val == null)
                {
                    val = double.Parse(line);
                }
            }

            if (t == CommandType.Null || id == null || val == null)
            {
                throw new Exception("could not parse control message");
            }

            Command = new Command(t, (int) id, (double) val);
            
            return this;
        }
    }
}
