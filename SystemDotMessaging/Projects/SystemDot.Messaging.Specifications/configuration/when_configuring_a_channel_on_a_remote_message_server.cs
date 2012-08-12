using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_channel_on_a_remote_message_server : WithConfiguationSubject
    {
        const string ChannelName = "TestChannel";
        const string ServerName = "TestServer";

        static IBus bus;
        
        Establish context = () =>
        {
            Components.Registration = () =>
            {
                ConfigureAndRegister<IMachineIdentifier>(new MachineIdentifier());
                ConfigureAndRegister(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                ConfigureAndRegister<ISubscriptionRequestorChannelBuilder>();
                ConfigureAndRegister<IMessageReciever>();
                ConfigureAndRegister<ITaskLooper>();
                ConfigureAndRegister<IBus>();
            };
        };

        Because of = () => bus = Configuration.Configure
            .UsingHttpMessaging()
            .WithRemoteMessageServer(ServerName)
            .OpenChannel(ChannelName)
            .ForRequestReplyRecieving()
            .Initialise();

        It should_register_the_listening_address_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => r.RegisterListeningAddress(GetEndpointAddress(ChannelName, ServerName)));

    }
}