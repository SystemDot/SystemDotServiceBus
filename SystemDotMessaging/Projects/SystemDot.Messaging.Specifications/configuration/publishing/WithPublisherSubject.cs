using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class WithPublisherSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<ISubscriptionHandlerChannelBuilder>();
            ConfigureAndRegister<IPublisherRegistry>();
            ConfigureAndRegister<IPublisherChannelBuilder>(new TestPublisherChannelBuilder());
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };
    }
}