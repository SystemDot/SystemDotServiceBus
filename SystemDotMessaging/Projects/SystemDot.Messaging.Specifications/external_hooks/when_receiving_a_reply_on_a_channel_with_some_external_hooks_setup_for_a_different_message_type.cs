using System;
using SystemDot.Messaging.Hooks.External;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.external_hooks
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel_with_some_external_hooks_setup_for_a_different_message_type : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static Int64 message;
        static MessagePayload payload;
        static TestExternalInspector<string> inspector;
        static TestExternalInspectorLoader inspectorLoader;

        Establish context = () =>
        {
            inspectorLoader = new TestExternalInspectorLoader();

            inspector = new TestExternalInspector<string>();
            inspectorLoader.AddHook(inspector);

            Register<IExternalInspectorLoader>(inspectorLoader);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(RecieverAddress)
                .WithReceiveHook(ExternalInspectorHook.LoadUp())
                .Initialise();

            message = 1;

            payload = new MessagePayload().MakeSequencedReceivable(
                message,
                RecieverAddress,
                ChannelName,
                PersistenceUseType.ReplySend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_run_the_message_through_the_hook = () => inspector.Message.ShouldBeNull();
    }
}