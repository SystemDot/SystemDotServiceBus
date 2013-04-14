using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.point_to_point.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_twenty_messages_that_are_not_yet_acknowledged_and_four_seconds_have_passed 
        : WithMessageConfigurationSubject
    {
        static TestTaskScheduler scheduler;
        
        Establish context = () =>
        {
            scheduler = new TestTaskScheduler();
            ConfigureAndRegister<ITaskScheduler>(scheduler);

            IBus bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            var messages = Enumerable.Range(1, 20).ToList();

            messages.ForEach(m => bus.Send(m));
        };

        Because of = () => scheduler.PassTime(TimeSpan.FromSeconds(4));

        It should_repeat_the_messages_to_feel_out_the_connection = () => Server.SentMessages.Count.ShouldEqual(40);
    }
}
