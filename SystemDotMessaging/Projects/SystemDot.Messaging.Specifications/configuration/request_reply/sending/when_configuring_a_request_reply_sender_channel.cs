using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages;
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
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<IRequestSendChannelBuilder>();
                ConfigureAndRegister<IReplyRecieveChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();

            };
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
            .OpenChannel(ChannelName).ForRequestReplySendingTo(RecieverAddress)
            .Initialise();

        It should_build_the_send_channel = () =>
            The<IRequestSendChannelBuilder>().WasToldTo(b =>
                b.Build(
                GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName()),
                GetEndpointAddress(RecieverAddress, The<IMachineIdentifier>().GetMachineName())));

        It should_build_the_recieve_channel = () =>
            The<IReplyRecieveChannelBuilder>().WasToldTo(b => b.Build(new IMessageProcessor<object, object>[0]));

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r =>
                r.RegisterListeningAddress(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_start_the_task_looper = () => The<ITaskLooper>().WasToldTo(l => l.Start());

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}