using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    public static class MessagePayloadToAddressExtensions
    {
        public static void SetToAddress(this MessagePayload payload, Address address)
        {
            Contract.Requires(address != null);
            payload.AddHeader(new ToAddressHeader(address));
        }

        public static Address GetToAddress(this MessagePayload payload)
        {
            return payload.Headers.OfType<ToAddressHeader>().Single().Address;
        }
    }
}