using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Messaging.Channels.Messages.Consuming;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class UsingDefaultsConfiguration
    {
        public MessageHandlerConfiguration HandlingMessagesWith<T>(IMessageHandler<T> toRegister)
        {
            Contract.Requires(toRegister != null);
            return new MessageHandlerConfiguration(toRegister);
        }
    }
}