using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_a_message_that_is_time_expired : WithMessageConfigurationSubject
    {
        

        Establish context = () =>
        {
            var systemTime = new TestSystemTime(DateTime.Now);
            ConfigureAndRegister<ISystemTime>(systemTime);

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForPointToPointSendingTo("SenderAddress")
                .WithMessageExpiry(MessageExpiry.ByTime(TimeSpan.FromSeconds(3)))
                .Initialise();

            Bus.Send(1);

            systemTime.AddToCurrentDate(TimeSpan.FromSeconds(4));
            Server.SentMessages.Clear();
        };

        Because of = () => The<ITaskRepeater>().Start();

        It should_pass_the_message_through = () => Server.SentMessages.ShouldBeEmpty();
    }
}