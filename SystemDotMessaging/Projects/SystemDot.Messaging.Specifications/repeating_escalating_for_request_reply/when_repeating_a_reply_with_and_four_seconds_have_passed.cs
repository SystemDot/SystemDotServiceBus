using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating_escalating_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_reply_with_and_four_seconds_have_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSender";
        const int Request = 1;
        const int Reply = 2;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                 .UsingInProcessTransport()
                 .OpenChannel(ChannelName)
                 .ForRequestReplyReceiving()
                 .Initialise();

            GetServer().ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            Bus.Reply(Reply);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => GetServer().SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(2);
    }
}