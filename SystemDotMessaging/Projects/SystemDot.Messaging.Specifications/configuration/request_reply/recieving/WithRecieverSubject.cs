using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    public class WithRecieverSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister(new EndpointAddressBuilder());
            ConfigureAndRegister<IAcknowledgementChannelBuilder>();
            ConfigureAndRegister<IRequestRecieveChannelBuilder>();
            ConfigureAndRegister<IReplySendChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<ITaskRepeater>();
            ConfigureAndRegister<IBus>();
        };
    }
}