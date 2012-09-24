using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class WithPublisherSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
            ConfigureAndRegister<IAcknowledgementChannelBuilder>();
            ConfigureAndRegister<IPublisherRegistry>();
            ConfigureAndRegister<IPublisherChannelBuilder>(new TestPublisherChannelBuilder());
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<ITaskRepeater>();
            ConfigureAndRegister<IPersistence>();
            ConfigureAndRegister<IBus>();
        };
    }
}