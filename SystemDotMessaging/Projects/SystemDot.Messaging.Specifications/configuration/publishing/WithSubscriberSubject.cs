using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.publishing
{
    public class WithSubscriberSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister(new EndpointAddressBuilder());
            ConfigureAndRegister<IAcknowledgementChannelBuilder>();
            ConfigureAndRegister<ISubscriberChannelBuilder>();
            ConfigureAndRegister<ISubscriptionRequestChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };
    }
}