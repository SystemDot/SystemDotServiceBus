using System;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    [Serializable]
    public class BodyMessageHeader : IMessageHeader
    {
        public BodyMessageHeader(byte[] body)
        {
            Body = body;
        }

        public byte[] Body { get; private set; }
    }
}