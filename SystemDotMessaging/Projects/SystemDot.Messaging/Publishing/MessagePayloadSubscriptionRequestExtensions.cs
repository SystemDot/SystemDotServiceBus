using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Publishing
{
    public static class MessagePayloadSubscriptionRequestExtensions
    {
        public static void SetSubscriptionRequest(this MessagePayload payload, SubscriptionSchema schema)
        {
            Contract.Requires(schema != null);

            payload.AddHeader(new SubscriptionRequestHeader(schema));
        }

        public static bool IsSubscriptionRequest(this MessagePayload payload)
        {
            return payload.HasHeader<SubscriptionRequestHeader>();
        }

        public static SubscriptionSchema GetSubscriptionRequestSchema(this MessagePayload payload)
        {
            Contract.Requires(payload.IsSubscriptionRequest());

            return payload.GetHeader<SubscriptionRequestHeader>().Schema;
        }
    }
}