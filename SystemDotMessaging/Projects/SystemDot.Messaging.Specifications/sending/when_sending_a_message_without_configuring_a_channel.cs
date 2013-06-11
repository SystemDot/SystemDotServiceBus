using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_without_configuring_a_channel : WithMessageConfigurationSubject
    {
        static Exception exception;

        Establish context = () => Configuration.Configure.Messaging()
            .UsingInProcessTransport()
            .OpenLocalChannel()
            .Initialise();

        Because of = () => exception = Catch.Exception(() => Bus.Send(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}