using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_receiver_channel : WithConfiguationSubject
    {
        const string ChannelName = "Test";
        static IBus bus;

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());
            ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
            ConfigureAndRegister(new EndpointAddressBuilder(IocContainerLocator.Locate().Resolve<IMachineIdentifier>()));
            ConfigureAndRegister<IRequestRecieveChannelBuilder>();
            ConfigureAndRegister<IReplySendChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
            .OpenChannel(ChannelName)
            .ForRequestReplyRecieving()
            .Initialise();

        It should_build_the_request_recieve_channel = () =>
            The<IRequestRecieveChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        private It should_build_the_reply_send_channel = () => 
            The<IReplySendChannelBuilder>().WasToldTo(
                b => b.Build(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.StartPolling(GetEndpointAddress(ChannelName, The<IMachineIdentifier>().GetMachineName())));

        It should_return_the_bus = () => bus.ShouldBeTheSameAs(The<IBus>());
        
    }
}