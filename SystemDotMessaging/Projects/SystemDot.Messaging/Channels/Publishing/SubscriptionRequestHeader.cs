using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Publishing
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

        public override string ToString()
        {
            return string.Concat(this.GetType(), ": ", Schema.ToString());
        }
    }
}