using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel_with_a_hook : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";
        const int Message = 1;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyReceiving()
                    .WithReplyHook(hook)
                .Initialise();

            GetServer().ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                Message,
                SenderChannelName,
                ChannelName,
                PersistenceUseType.RequestSend));
        };

        Because of = () => Bus.Reply(Message);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(Message);
    }
}