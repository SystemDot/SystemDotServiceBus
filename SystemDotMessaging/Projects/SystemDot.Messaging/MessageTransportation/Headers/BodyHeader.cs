using System;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    [Serializable]
    public class BodyHeader : IMessageHeader
    {
        public BodyHeader(byte[] body)
        {
            Body = body;
        }

        public byte[] Body { get; private set; }
    }
}