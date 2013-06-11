using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;

namespace SystemDot.Messaging.Addressing
{
    public class MessageOriginServerAddressRegistrar : MessageProcessor
    {
        readonly ServerAddressRegistry serverAddressRegistry;

        public MessageOriginServerAddressRegistrar(ServerAddressRegistry serverAddressRegistry)
        {
            this.serverAddressRegistry = serverAddressRegistry;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            if(toInput.HasFromServerAddress())
                RegisterFromServerAddress(toInput);

            OnMessageProcessed(toInput);
        }

        void RegisterFromServerAddress(MessagePayload toInput)
        {
            this.serverAddressRegistry.Register(
                toInput.GetFromAddress().ServerPath.Server.Name,
                toInput.GetFromServerAddress().Address);
        }
    }
}