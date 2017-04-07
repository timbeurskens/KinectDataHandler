using System;
using System.Runtime.Serialization;

namespace KinectDataHandler.BodyAnalyzer
{
    [Serializable]
    public class NotTrackedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NotTrackedException()
        {
        }

        public NotTrackedException(string message) : base(message)
        {
        }

        public NotTrackedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NotTrackedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {

        }
    }
}