using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_message_message_without_configuring_a_request_reply_channel 
        : WithMessageConfigurationSubject
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

        Because of = () => exception = Catch.Exception(() => bus.Reply(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}