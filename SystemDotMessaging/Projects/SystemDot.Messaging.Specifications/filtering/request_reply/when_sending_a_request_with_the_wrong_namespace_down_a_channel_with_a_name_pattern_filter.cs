using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering.request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_wrong_namespace_down_a_channel_with_a_namespace_pattern_filter 
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                    .ForRequestReplySendingTo("TestRecieverAddress")
                        .OnlyForMessages(FilteredBy.Namespace("SomethingElse"))
                .Initialise();

        Because of = () => Bus.Send(new TestNamePatternMessage());

        It should_not_pass_the_message_through = () => Server.SentMessages.ShouldBeEmpty();
    }
}