using System;
using System.Linq;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Specifications;
using Machine.Specifications;

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
            originDate = DateTime.Now;

            ConfigureAndRegister<ISystemTime>(new TestSystemTime(originDate));

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName).ForPointToPointSendingTo(ReceiverName)
                .Initialise();

            message = 1;
        };

        Because of = () => Bus.Send(message);

        It should_mark_the_message_with_the_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequence().ShouldEqual(1);

        It should_mark_the_message_with_first_sequence = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetFirstSequence().ShouldEqual(1);

        It should_mark_the_message_with_sequence_origin_date = () =>
            Server.SentMessages.ExcludeAcknowledgements().First().GetSequenceOriginSetOn().ShouldEqual(originDate);
    }
}