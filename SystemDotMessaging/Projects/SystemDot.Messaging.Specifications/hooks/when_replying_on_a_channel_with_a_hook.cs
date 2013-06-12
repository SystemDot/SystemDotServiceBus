using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel_with_a_hook : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderChannelName = "TestSender";

        
        static int message;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .WithReplyHook(hook)
                .Initialise();

            Server.ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1,
                SenderChannelName,
                ChannelName,
                PersistenceUseType.RequestSend));

            message = 1;
        };

        Because of = () => Bus.Reply(message);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(message);
    }
}