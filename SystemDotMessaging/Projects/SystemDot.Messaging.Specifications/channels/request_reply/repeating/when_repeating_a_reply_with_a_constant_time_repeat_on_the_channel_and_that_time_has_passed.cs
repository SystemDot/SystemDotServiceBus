using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.channels.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.request_reply.repeating      
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_reply_with_a_constant_time_repeat_on_the_channel_and_that_time_has_passed 
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

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplyRecieving()
                .WithMessageRepeating(RepeatMessages.Every(TimeSpan.FromSeconds(10)))
                .Initialise();

            Server.ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    Request,
                    SenderAddress,
                    ChannelName,
                    PersistenceUseType.RequestSend));

            Bus.Reply(Reply);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(10));
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_repeat_the_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(2);
    }
}