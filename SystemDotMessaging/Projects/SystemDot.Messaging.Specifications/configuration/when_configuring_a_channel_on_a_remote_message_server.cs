using SystemDot.Ioc;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Configuration")] 
    public class when_configuring_a_channel_on_a_remote_message_server : WithConfiguationSubject
    {
        const string ChannelName = "TestChannel";
        const string ServerName = "TestServer";

        static IBus bus;

        Establish context = () =>
        {
            IocContainerLocator.SetContainer(new IocContainer());

            ConfigureAndRegister<IRequestRecieveChannelBuilder>();
            ConfigureAndRegister<IReplySendChannelBuilder>();
            ConfigureAndRegister<IMessageReciever>();
            ConfigureAndRegister<IBus>();
        };

        Because of = () => bus = Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Named(ServerName))
            .OpenChannel(ChannelName).ForRequestReplyRecieving()
            .Initialise();

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => r.StartPolling(GetEndpointAddress(ChannelName, ServerName)));

    }
}