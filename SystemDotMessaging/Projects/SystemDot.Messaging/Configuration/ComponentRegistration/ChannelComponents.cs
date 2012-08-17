using System;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Handling;
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
            IocContainer.Register<IBus>(new MessageBus());
            
            IocContainer.Register(new MessagePayloadCopier(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register<IDistributor>(() => new Distributor(IocContainer.Resolve<MessagePayloadCopier>()));

            IocContainer.Register(() => new ChannelStartPoint());

            IocContainer.Register(() => new MessagePayloadPackager(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register<MessageAddresser, EndpointAddress, EndpointAddress>((f, t) => new MessageAddresser(f, t));
            IocContainer.Register(() => new MessagePayloadUnpackager(IocContainer.Resolve<ISerialiser>()));
            IocContainer.Register(new MessageHandlerRouter());
            
            IocContainer.Register<MessageRepeater, TimeSpan>((t) => new MessageRepeater(t, IocContainer.Resolve<ITaskScheduler>()));
            IocContainer.Register<SubscriptionRequestor, EndpointAddress>(a => new SubscriptionRequestor(a));
        }
    }
}