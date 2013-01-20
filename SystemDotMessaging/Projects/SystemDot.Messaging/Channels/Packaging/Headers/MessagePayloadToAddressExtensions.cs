using System.Diagnostics.Contracts;
using System.Linq;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.Packaging.Headers
{
    public static class MessagePayloadToAddressExtensions
    {
        public static void SetFromAddress(this MessagePayload payload, EndpointAddress address)
        {
            Contract.Requires(address != null);
            payload.AddHeader(new FromAddressHeader(address));
        }

        public static EndpointAddress GetFromAddress(this MessagePayload payload)
        {
            return payload.GetHeader<FromAddressHeader>().Address;
        }

        public static void SetToAddress(this MessagePayload payload, EndpointAddress address)
        {
            Contract.Requires(address != null);
            payload.AddHeader(new ToAddressHeader(address));
        }

        public static EndpointAddress GetToAddress(this MessagePayload payload)
        {
            return payload.GetHeader<ToAddressHeader>().Address;
        }

        public static bool HasToAddress(this MessagePayload payload)
        {
            return payload.HasHeader<ToAddressHeader>();
        }
    }
}