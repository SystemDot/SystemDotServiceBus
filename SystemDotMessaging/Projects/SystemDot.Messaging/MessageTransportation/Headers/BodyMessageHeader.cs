using System;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    [Serializable]
    public class BodyMessageHeader : IMessageHeader
    {
        public BodyMessageHeader(object body)
        {
            Body = body;
        }

        public object Body { get; private set; }
    }
}