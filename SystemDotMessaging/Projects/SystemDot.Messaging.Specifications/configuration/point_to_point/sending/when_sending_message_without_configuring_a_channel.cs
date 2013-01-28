using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.point_to_point.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_message_without_configuring_a_channel : WithMessageConfigurationSubject
    {
        static IBus bus;
        static Exception exception;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .Initialise();
        };

        Because of = () => exception = Catch.Exception(() => bus.Send(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}