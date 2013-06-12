using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.repeating.escalating.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_reply_for_the_third_time_and_sixteen_seconds_have_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSender";
        const int Request = 1;
        const int Reply = 2;

        
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Messaging.Configuration.Configure.Messaging()
                 .UsingInProcessTransport()
                 .OpenChannel(ChannelName)
                 .ForRequestReplyRecieving()
                 .Initialise();

            Server.ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            Bus.Reply(Reply);


            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));

            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(8));

            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(16).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(3);
    }
}