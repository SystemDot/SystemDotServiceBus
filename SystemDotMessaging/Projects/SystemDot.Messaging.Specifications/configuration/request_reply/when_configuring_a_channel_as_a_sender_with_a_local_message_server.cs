using SystemDot.Messaging.Configuration.ComponentRegistration;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    [Subject("Request reply configuration")]
    public class when_configuring_a_channel_as_a_sender_with_a_local_message_server : WithSubject<object>
    {
        const string MachineName = "TestServer";
        const string ChannelName = "TestChannel";
        const string RecieverName = "TestReciever";

        static IBus bus;
        
        Establish context = () =>
        {     
            Components.Registration = () =>
            {
                IocContainer.Register<IMachineIdentifier>(new TestMachineIdentifier(MachineName));
                IocContainer.Register<IBus>(An<IBus>());
            };
        };

        Because of = () => bus = Configuration.Configure
            .WithLocalMessageServer()
            .OpenChannel(ChannelName)
            .AsRequestReplySenderTo(RecieverName)
            .Initialise();

        It should_create_a_bus = () => bus.ShouldBeTheSameAs(IocContainer.Resolve<IBus>());
    }
}