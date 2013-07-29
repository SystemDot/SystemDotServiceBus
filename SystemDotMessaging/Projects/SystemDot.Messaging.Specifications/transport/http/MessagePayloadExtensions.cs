using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.transport.http
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeSequencedReceivable(
            this MessagePayload payload,
            object message,
            EndpointAddress from,
            EndpointAddress to,
            PersistenceUseType useType)
        {
            payload.SetMessageBody(message);
            payload.SetFromAddress(from);
            payload.SetToAddress(to);
            payload.SetChannelType(useType);
            payload.Sequenced();
            
            return payload;
        }
    }
}