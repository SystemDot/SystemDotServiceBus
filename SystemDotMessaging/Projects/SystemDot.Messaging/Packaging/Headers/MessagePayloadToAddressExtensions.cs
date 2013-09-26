using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Packaging.Headers
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
        
        public static bool HasFromAddress(this MessagePayload payload)
        {
            return payload.HasHeader<FromAddressHeader>();
        }

        public static void SetToAddress(this MessagePayload payload, EndpointAddress address)
        {
            Contract.Requires(address != null);
            payload.AddHeader(new ToAddressHeader(address));
        }

        public static bool HasToAddress(this MessagePayload payload)
        {
            return payload.HasHeader<FromAddressHeader>();
        }

        public static EndpointAddress GetToAddress(this MessagePayload payload)
        {
            return payload.GetHeader<ToAddressHeader>().Address;
        }
    }
}