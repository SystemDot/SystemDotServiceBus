using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.hooks_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_channel_with_a_receive_hook : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSenderAddress";
        const Int64 Message = 1;
            
        static MessagePayload payload;
        static TestMessageProcessorHook hook;

        Establish context = () =>
        {
            hook = new TestMessageProcessorHook();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                    .ForRequestReplyReceiving()
                    .WithReceiveHook(hook)
                .Initialise();

            payload = new MessagePayload().MakeSequencedReceivable(
                Message,
                SenderAddress,
                ChannelName,
                PersistenceUseType.RequestSend);
        };

        Because of = () => GetServer().ReceiveMessage(payload);

        It should_run_the_message_through_the_hook = () => hook.Message.ShouldEqual(Message);
    }
}