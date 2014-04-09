using SystemDot.Messaging.Hooks.Upgrading;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.upgrade_hooks
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_another_message_through_upgrade_hooks : WithHttpConfigurationSubject
    {
        const string ServerName = "ServerName";
        static AnotherMessage message;

        Establish context = () =>
        {
            message = new AnotherMessage();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ServerName)
                .OpenChannel("TestSender")
                .ForRequestReplySendingTo("TestReceiver")
                .WithSendHook(UpgradeMessageHook.LoadUp())
                .Initialise();
        };

        Because of = () => Bus.Send(message);

        It should_send_it_as_the_upgraded_message = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .DeserialiseTo<AnotherMessage>().Field.ShouldBeTrue();
    }
}