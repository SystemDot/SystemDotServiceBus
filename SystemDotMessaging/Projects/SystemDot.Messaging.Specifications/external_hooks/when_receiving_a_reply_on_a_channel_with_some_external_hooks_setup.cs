using System;
using SystemDot.Messaging.Hooks.External;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using FluentAssertions;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.external_hooks
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_on_a_channel_with_some_external_hooks_setup : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static Int64 message;
        static MessagePayload payload;
        static TestExternalHook<long> hook1;
        static TestExternalHook<long> hook2;
        static TestExternalHookLoader hookLoader;

        Establish context = () =>
        {
            hookLoader = new TestExternalHookLoader();
            
            hook1 = new TestExternalHook<long>();
            hookLoader.AddHook(hook1);
            
            hook2 = new TestExternalHook<long>();
            hookLoader.AddHook(hook2);

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

        It should_run_the_message_through_the_first_hook = () => hook1.Message.Should().Be(message);

        It should_run_the_message_through_the_second_hook = () => hook2.Message.Should().Be(message);
    }
}