using System;
using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.publishing;
using FluentAssertions;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.sequencing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string ReceiverName = "TestReceiver";
        
        static int message;
        static DateTime originDate;

        Establish context = () =>
        {
            originDate = SystemTime.GetCurrentDate();

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_mark_the_message_with_the_sequence = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldBeEquivalentTo(1);

        It should_mark_the_message_with_first_sequence = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldBeEquivalentTo(1);

        It should_mark_the_message_with_sequence_origin_date = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().GetSequenceOriginSetOn().ShouldBeEquivalentTo(originDate);
    }
}