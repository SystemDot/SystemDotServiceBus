using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    public class WithSenderSubject : WithConfiguationSubject
    {
        Establish context = () =>
        {
            ConfigureAndRegister(new EndpointAddressBuilder());
            ConfigureAndRegister<IAcknowledgementChannelBuilder>();
            ConfigureAndRegister<IRequestSendChannelBuilder>(new TestRequestSendChannelBuilder());
            ConfigureAndRegister<IReplyRecieveChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IPersistence>();
            ConfigureAndRegister<ITaskRepeater>();
            ConfigureAndRegister<IBus>();
        };
    }
}