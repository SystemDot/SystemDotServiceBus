using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
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
        static TestMessageReciever reciever;

        Establish context = () =>
        {
            reciever = new TestMessageReciever();
                    
            Components.Registration = () =>
            {
                IocContainer.Register<IBus>(An<IBus>());
                IocContainer.Register(new SubscriptionRequestHandler(new ChannelBuilder()));
                IocContainer.Register<IMachineIdentifier>(new TestMachineIdentifier(MachineName));
                IocContainer.Register(new EndpointAddressBuilder(IocContainer.Resolve<IMachineIdentifier>()));
                IocContainer.Register<IMessageReciever>(reciever);
                IocContainer.Register<ITaskStarter>(new TestTaskStarter());
                IocContainer.Register<ITaskLooper>(An<ITaskLooper>());
            };
        };

        Because of = () => initialisedBus = 
            Configuration.Configure
                .WithLocalMessageServer()
                .OpenChannel(ChannelName)
                .AsRequestReplyReciever()
                .Initialise();

        It should_create_a_bus = () => initialisedBus.ShouldBeTheSameAs(IocContainer.Resolve<IBus>());

        It should_register_to_listen_for_the_address_on_the_reciever = () =>
            reciever.ListeningAddresses.First().Channel.ShouldEqual(string.Concat(ChannelName, "@", MachineName));
    }
}