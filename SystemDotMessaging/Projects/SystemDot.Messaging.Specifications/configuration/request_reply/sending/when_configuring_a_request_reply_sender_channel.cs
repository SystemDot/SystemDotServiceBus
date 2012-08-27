using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_sender_channel : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        private const string RecieverAddress = "TestRecieverAddress";
        static IBus bus;

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(new MachineIdentifier()));
            ConfigureAndRegister<IRequestSendChannelBuilder>(new TestRequestSendChannelBuilder());
            ConfigureAndRegister<IReplyRecieveChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
            .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
            .Initialise();

        It should_build_the_send_channel_with_the_default_pass_through_message_filter = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().MessageFilter
                .ShouldBeOfType<PassThroughMessageFilterStategy>();

        It should_build_the_send_channel_with_the_correct_from_address = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().From.ShouldEqual(
                GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()));

        It should_build_the_send_channel_with_the_correct_reciever_address = () =>
            The<IRequestSendChannelBuilder>().As<TestRequestSendChannelBuilder>().Reciever.ShouldEqual(
                GetEndpointAddress(RecieverAddress, The<IMachineIdentifier>().GetMachineName()));

        It should_build_the_recieve_channel = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => 
                b.Build(
                    GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()), 
                    new IMessageProcessor<object, object>[0]));

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r =>
                r.StartPolling(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
    }
}