using SystemDot.Ioc;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    public class WithRecieverSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister(new EndpointAddressBuilder());
            ConfigureAndRegister<IRequestRecieveChannelBuilder>();
            ConfigureAndRegister<IReplySendChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };
    }
}