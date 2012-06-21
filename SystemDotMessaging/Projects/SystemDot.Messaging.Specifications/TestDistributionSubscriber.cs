using System.Collections.Generic;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Specifications
{
    public class TestDistributionSubscriber : IDistributionSubscriber
    {
        public List<MessagePayload> ProcessedMessages { get; private set; }

        public TestDistributionSubscriber()
        {
            ProcessedMessages = new List<MessagePayload>();
        }

        public void Recieve(MessagePayload message)
        {
            ProcessedMessages.Add(message);
        }
    }
}