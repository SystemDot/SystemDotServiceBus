using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_a_request_without_configuring_a_request_reply_channel 
        : WithMessageConfigurationSubject
    {
        
        static Exception exception;

        Establish context = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .Initialise();

        Because of = () => exception = Catch.Exception(() => Bus.Reply(new object()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}