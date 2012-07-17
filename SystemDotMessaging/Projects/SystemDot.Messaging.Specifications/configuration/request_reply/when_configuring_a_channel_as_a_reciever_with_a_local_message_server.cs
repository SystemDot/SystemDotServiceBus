using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    [Subject("Request reply configuration")]
    public class when_configuring_a_channel_as_a_reciever_with_a_local_message_server : WithSubject<object>
    {
        const string MachineName = "TestMachine";
        const string ChannelName = "TestChannel";
        static IBus initialisedBus;
        static TestChannelBuilder channelBuilder;
        static TestMessageReciever reciever;

        Establish context = () =>
        {
            channelBuilder = new TestChannelBuilder();
            reciever = new TestMessageReciever();
                    
            Components.Registration = () =>
            {
                IocContainer.Register<IBus>(An<IBus>());
                IocContainer.Register(new RequestReplySubscriptionHandler());
                IocContainer.Register<IMachineIdentifier>(new TestMachineIdentifier(MachineName));
                IocContainer.Register<IMessageReciever>(reciever);
                IocContainer.Register<IChannelBuilder>(channelBuilder);
            };
        };

        Because of = () => initialisedBus = 
            Configuration.Configure
                .WithLocalMessageServer()
                .OpenChannel(ChannelName)
                .AsRequestReplyReciever()
                .Initialise();

        It should_create_a_bus = () => initialisedBus.ShouldBeTheSameAs(IocContainer.Resolve<IBus>());

        It should_build_a_request_reply_subscription_handler_channel_with_the_message_reciever_as_its_start_point = () =>
            channelBuilder.StartPoint.ShouldBeTheSameAs(reciever);

        It should_register_to_listen_for_the_address_on_the_reciever = () =>
            reciever.ListeningAddresses.First().Address.ShouldEqual(string.Concat(ChannelName, "@", MachineName));

        It should_build_a_request_reply_subscription_handler_channel_with_the_subscription_handler_as_its_end_point = () =>
            channelBuilder.EndPoint.ShouldBeTheSameAs(IocContainer.Resolve<RequestReplySubscriptionHandler>());
    }
}