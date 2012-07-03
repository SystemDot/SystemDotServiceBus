using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Channels.Publishing
{
    public class SubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        readonly EndpointAddress subscriberAddress;
        
        public event Action<MessagePayload> MessageProcessed;
        
        public SubscriptionRequestor(EndpointAddress subscriberAddress)
        {
            this.subscriberAddress = subscriberAddress;
        }

        public void Start()
        {
            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema(subscriberAddress));
            MessageProcessed(request);
        }
    }
}