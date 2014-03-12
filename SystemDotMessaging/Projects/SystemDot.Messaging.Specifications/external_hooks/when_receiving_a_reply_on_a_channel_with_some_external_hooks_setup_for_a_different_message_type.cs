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
        static TestExternalHook<string> hook;
        static TestExternalHookLoader hookLoader;

        Establish context = () =>
        {
            hookLoader = new TestExternalHookLoader();

            hook = new TestExternalHook<string>();
            hookLoader.AddHook(hook);

            Register<IExternalHookLoader>(hookLoader);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(RecieverAddress)
                .WithReceiveHook(ExternalHooker.LoadUp())
                .Initialise();

            message = 1;

            payload = new MessagePayload().MakeSequencedReceivable(
                message,
                RecieverAddress,
                ChannelName,
                PersistenceUseType.ReplySend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_not_run_the_message_through_the_hook = () => hook.Message.ShouldBeNull();
    }
}