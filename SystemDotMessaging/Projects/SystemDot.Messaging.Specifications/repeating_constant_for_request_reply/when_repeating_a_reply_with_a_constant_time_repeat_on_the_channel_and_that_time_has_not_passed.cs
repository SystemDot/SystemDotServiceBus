using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.repeating_constant_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_reply_with_a_constant_time_repeat_on_the_channel_and_that_time_has_not_passed
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
                .RepeatMessages().Every(TimeSpan.FromSeconds(10))
                .Initialise();

            GetServer().ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            Bus.Reply(Reply);

            SystemTime.AdvanceTime(TimeSpan.FromSeconds(10).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_not_repeat_the_message = () => GetServer().SentMessages.ExcludeAcknowledgements().Count.ShouldBeEquivalentTo(1);
    }
}