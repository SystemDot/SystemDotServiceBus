using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_without_configuring_a_local_channel : WithMessageConfigurationSubject
    {
        static IBus bus;
        static Exception exception;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Channel").ForRequestReplyRecieving()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => bus.SendLocal(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}