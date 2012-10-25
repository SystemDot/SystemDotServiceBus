using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Caching
{
    public class MessageDecacher : IMessageInputter<MessagePayload>
    {
        readonly IPersistence persistence;

        public MessageDecacher(IPersistence persistence)
        {
            Contract.Requires(persistence != null);
            this.persistence = persistence;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.persistence.Delete(toInput.Id);
        }
    }
}