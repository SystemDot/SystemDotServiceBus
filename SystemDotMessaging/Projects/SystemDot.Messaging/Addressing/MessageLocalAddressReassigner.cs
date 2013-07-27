using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Addressing
{
    class MessageLocalAddressReassigner : MessageProcessor
    {
        readonly ServerAddressRegistry registry;

        public MessageLocalAddressReassigner(ServerAddressRegistry registry)
        {
            Contract.Requires(registry != null);

            this.registry = registry;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            ReassignServerAddressIfRegistered(toInput.GetFromAddress().Route.Proxy);
            OnMessageProcessed(toInput);
        }

        void ReassignServerAddressIfRegistered(MessageServer serverToCheck)
        {
            if (serverToCheck is NullMessageServer || !registry.Contains(serverToCheck.Name)) return;
                
            serverToCheck.Address = registry.Lookup(serverToCheck.Name);
        }
    }
}