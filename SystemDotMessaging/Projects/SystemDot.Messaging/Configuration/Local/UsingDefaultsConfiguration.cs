using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Channels.Local;
using SystemDot.Messaging.Configuration.Channels;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.Local
{
    public class UsingDefaultsConfiguration
    {
        public void Initialise()
        {
            ChannelBuilder
               .StartsWith(BuildMessageBus())
               .Pump()
               .ToProcessor(BuildPayloadPackager())
               .ThenToEndPoint(BuildMessageSender());
        }

        private static MessageBus BuildMessageBus()
        {
            return new MessageBus();
        }

        private static MessagePayloadPackager BuildPayloadPackager()
        {
            return new MessagePayloadPackager(MessagingEnvironment.GetComponent<ISerialiser>());
        }

        private MessageSender BuildMessageSender()
        {
            return new MessageSender(
                MessagingEnvironment.GetComponent<IFormatter>(), 
                MessagingEnvironment.GetComponent<IWebRequestor>());
        }
    }
}