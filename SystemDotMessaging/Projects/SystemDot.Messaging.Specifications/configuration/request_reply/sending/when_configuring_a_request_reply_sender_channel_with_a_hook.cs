using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel_with_a_hook : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<IRequestSendChannelBuilder>();
            ConfigureAndRegister<IReplyRecieveChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<ITaskLooper>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
                    .WithHook(The<IMessageProcessor<object, object>>())
            .Initialise();

        It should_build_the_recieve_channel_with_the_specified_hook = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => 
                b.Build( 
                    GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()), 
                    The<IMessageProcessor<object, object>>()));        
    }
}