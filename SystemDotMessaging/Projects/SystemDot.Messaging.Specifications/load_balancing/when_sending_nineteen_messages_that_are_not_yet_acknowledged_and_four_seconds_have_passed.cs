﻿using System;
using System.Linq;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.load_balancing
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_nineteen_messages_that_are_not_yet_acknowledged_and_four_seconds_have_passed
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("SenderAddress")
                .ForPointToPointSendingTo("ReceiverAddress")
                .Initialise();

            var messages = Enumerable.Range(1, 19).ToList();

            messages.ForEach(m => Bus.Send(m));
        };

        Because of = () => SystemTime.AdvanceTime(TimeSpan.FromSeconds(4));

        It should_only_send_nineteen_messages = () => GetServer().SentMessages.Count.ShouldEqual(19);
    }
}