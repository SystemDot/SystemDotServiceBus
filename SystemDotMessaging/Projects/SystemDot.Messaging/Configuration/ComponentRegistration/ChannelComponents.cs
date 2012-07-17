using System;
using SystemDot.Messaging.Channels;
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
            IocContainer.Register(new MessagePayloadCopier(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register<IDistributor>(() => new Distributor(IocContainer.Resolve<MessagePayloadCopier>()));
            IocContainer.Register<IBus>(new MessageBus());
            IocContainer.Register(() => new MessagePayloadPackager(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register<MessageAddresser, EndpointAddress>(a => new MessageAddresser(a));
            IocContainer.Register(() => new MessagePayloadUnpackager(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register(new MessageHandlerRouter());
            
            IocContainer.Register<MessageRepeater, TimeSpan>((t) => new MessageRepeater(t, IocContainer.Resolve<ITaskScheduler>()));
            IocContainer.Register<SubscriptionRequestor, EndpointAddress>(a => new SubscriptionRequestor(a));
            IocContainer.Register<IChannelBuilder>(new ChannelBuilder());

        }
    }
}