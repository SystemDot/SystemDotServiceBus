using System;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class ChannelComponents
    {
        public static void Register()
        {
            MessagingEnvironment.RegisterComponent(new MessagePayloadCopier());

            MessagingEnvironment.RegisterComponent<IDistributor>(
                () => new Distributor(MessagingEnvironment.GetComponent<MessagePayloadCopier>()));
            
            MessagingEnvironment.RegisterComponent(() => new MessageBus());
            
            MessagingEnvironment.RegisterComponent(() => new MessagePayloadPackager(
                MessagingEnvironment.GetComponent<ISerialiser>()));

            MessagingEnvironment.RegisterComponent<MessageAddresser, EndpointAddress>(
                a => new MessageAddresser(a));
            
            MessagingEnvironment.RegisterComponent(
                () => new MessagePayloadUnpackager(
                    MessagingEnvironment.GetComponent<ISerialiser>()));

            MessagingEnvironment.RegisterComponent(() => new MessageHandlerRouter());

            MessagingEnvironment.RegisterComponent<MessageRepeater, TimeSpan>(
                (t) => new MessageRepeater(t, MessagingEnvironment.GetComponent<ITaskScheduler>()));

            MessagingEnvironment.RegisterComponent<SubscriptionRequestor, EndpointAddress>(
                a => new SubscriptionRequestor(a));

        }
    }
}