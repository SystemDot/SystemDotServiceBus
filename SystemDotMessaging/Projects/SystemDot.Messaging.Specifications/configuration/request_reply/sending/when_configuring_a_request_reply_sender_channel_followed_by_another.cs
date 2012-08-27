using SystemDot.Ioc;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel_followed_by_another : WithConfiguationSubject
    {
        const string Channel2Name = "Channel2";
        const string Reciever2Address = "RecieverAddress2";

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer(new TypeExtender()));
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<IRequestSendChannelBuilder>(new TestRequestSendChannelBuilder());
            ConfigureAndRegister<IRequestRecieveChannelBuilder>();
            ConfigureAndRegister<IReplyRecieveChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => Configuration.Configure.Messaging(IocContainerLocator.Locate())
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
                .OpenChannel(Channel2Name).ForRequestReplySendingTo(Reciever2Address)
            .Initialise();

        It should_build_the_send_channel_with_the_default_pass_through_message_filter = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().MessageFilter
                .ShouldBeOfType<PassThroughMessageFilterStategy>();

        It should_register_the_listening_address_with_the_message_reciever_for_the_second_channel = () =>
            The<IMessageReciever>().WasToldTo(r =>
                r.StartPolling(GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName())));
        
    }
}