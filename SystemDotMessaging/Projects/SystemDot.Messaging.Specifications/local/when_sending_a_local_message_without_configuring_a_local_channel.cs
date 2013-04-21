using System;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_without_configuring_a_local_channel : WithMessageConfigurationSubject
    {
        static Exception exception;

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenChannel("Channel").ForRequestReplyRecieving()
            .Initialise();

        Because of = () => exception = Catch.Exception(() => Bus.SendLocal(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}