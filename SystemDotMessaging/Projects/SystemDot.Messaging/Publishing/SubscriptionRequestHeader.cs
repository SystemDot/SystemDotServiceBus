using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    public class SubscriptionRequestHeader : IMessageHeader 
    {
        public SubscriptionSchema Schema { get; set; }

        public SubscriptionRequestHeader() {}

        public SubscriptionRequestHeader(SubscriptionSchema schema) 
        {
            Contract.Requires(schema != null);
            this.Schema = schema;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType(), ": ", this.Schema.ToString());
        }
    }
}