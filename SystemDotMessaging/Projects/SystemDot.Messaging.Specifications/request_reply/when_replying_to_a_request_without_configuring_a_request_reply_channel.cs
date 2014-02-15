using System;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_request_without_configuring_a_request_reply_channel 
        : WithMessageConfigurationSubject
    {
        
        static Exception exception;

        Establish context = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenDirectChannel()
                .Initialise();

        Because of = () => exception = Catch.Exception(() => Bus.Reply(new object()));

        It should_not_fail = () => exception.Should().BeNull();
    }
}