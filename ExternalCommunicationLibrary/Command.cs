using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalCommunicationLibrary
{
    public class Command
    {
        public CommandType Type;
        public int ModifierId;
        public double ModifierValue;

        public Command(CommandType type, int modifierId, double modifierValue)
        {
            this.Type = type;
            this.ModifierId = modifierId;
            this.ModifierValue = modifierValue;
        }

        public override string ToString()
        {
            return Type + "\n" + ModifierId + "\n" + ModifierValue;
        }
    }
}
