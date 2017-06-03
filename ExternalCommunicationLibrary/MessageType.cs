using System;
using System.Reflection;

namespace ExternalCommunicationLibrary
{
    public enum MessageType
    {
        [EnumTypeValue(typeof(Message))]
        Ping, //ping message
        Body, //raw body data
        Close,
        Handshake,
        Acknowledge,
        [EnumTypeValue(typeof(ControlMessage))]
        Control,
        Null
    }

    public class EnumTypeValue : System.Attribute
    {
        public EnumTypeValue(Type val)
        {
            Value = val;
        }

        public Type Value { get; }
    }

    public static class TypedEnum
    {
        public static Type GetTypeValue(Enum e)
        {
            var enumType = e.GetType();

            var fi = enumType.GetField(e.ToString());
            var attrs = fi.GetCustomAttributes(typeof(EnumTypeValue), false) as EnumTypeValue[];

            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0].Value;
            }

            return null;
        }
    }
}