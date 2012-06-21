using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.PubSub;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    [Serializable]
    public class SubscriptionRequestHeader : IMessageHeader 
    {
        public SubscriptionSchema Schema { get; private set; }

        public SubscriptionRequestHeader(SubscriptionSchema schema) 
        {
            Contract.Requires(schema != null);
            Schema = schema;
        }
    }
}