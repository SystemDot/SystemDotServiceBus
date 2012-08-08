using System;
using SystemDot.Logging;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class SubscriptionRequestor : ISubscriptionRequestor
    {
        readonly EndpointAddress subscriberAddress;

        public SubscriptionRequestor(EndpointAddress subscriberAddress)
        {
            this.subscriberAddress = subscriberAddress;
        }

        public event Action<MessagePayload> MessageProcessed;
        
        public void Start()
        {
            Logger.Info("Sending request reply subscription request");

            var request = new MessagePayload();
            request.SetSubscriptionRequest(new SubscriptionSchema(this.subscriberAddress));
            MessageProcessed(request);
        }

    }
}