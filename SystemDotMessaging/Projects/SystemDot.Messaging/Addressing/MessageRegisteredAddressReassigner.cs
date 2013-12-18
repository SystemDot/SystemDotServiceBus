using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Addressing
{
    class MessageRegisteredAddressReassigner : MessageProcessor
    {
        readonly ServerAddressRegistry registry;

        public MessageRegisteredAddressReassigner(ServerAddressRegistry registry)
        {
            Contract.Requires(registry != null);

            this.registry = registry;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            ReassignServerAddressIfRegistered(toInput.GetFromAddress().Server);
            OnMessageProcessed(toInput);
        }

        void ReassignServerAddressIfRegistered(MessageServer serverToCheck)
        {
            if (serverToCheck.IsUnspecified || !ServerIsInRegistry(serverToCheck)) return;
            serverToCheck.Address = registry.Lookup(serverToCheck.Name);
        }

        bool ServerIsInRegistry(MessageServer serverToCheck)
        {
            return registry.Contains(serverToCheck.Name);
        }
    }
}