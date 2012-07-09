using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Publishing;

namespace SystemDot.Messaging.Messages.Packaging.Headers
{
    public class SubscriptionRequestHeader : IMessageHeader 
    {
        public SubscriptionSchema Schema { get; set; }
        
        public SubscriptionRequestHeader() {}

        public SubscriptionRequestHeader(SubscriptionSchema schema) 
        {
            Contract.Requires(schema != null);
            Schema = schema;
        }
    }
}