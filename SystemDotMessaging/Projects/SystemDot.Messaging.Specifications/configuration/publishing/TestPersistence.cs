using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class TestPersistence : InMemoryPersistence
    {
        public MessagePayload StoredMessage { get; private set; }

        public override void StoreMessage(Channels.Packaging.MessagePayload message, Channels.EndpointAddress address)
        {
            base.StoreMessage(message, address);
            StoredMessage = message;
        }
    }
}