using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;
using SystemDot.Messaging.Specifications.configuration.publishing;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.replies.repeating
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_reply_for_the_third_time_and_sixteen_seconds_have_not_passed
        : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string SenderAddress = "TestSender";
        const int Request = 1;
        const int Reply = 2;

        static IBus bus;
        static TestSystemTime systemTime;

        Establish context = () =>
        {
            systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            bus = Configuration.Configure.Messaging()
                 .UsingInProcessTransport()
                 .OpenChannel(ChannelName)
                 .ForRequestReplyRecieving()
                 .Initialise();

            MessageReciever.ReceiveMessage(
                new MessagePayload().MakeReceiveable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            bus.Reply(Reply);


            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));

            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(8));

            The<ITaskRepeater>().Start();

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(16).Subtract(TimeSpan.FromTicks(1)));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => MessageSender.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(3);
    }
}