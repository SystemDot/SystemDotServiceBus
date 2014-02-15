using SystemDot.Messaging.Configuration;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.filtering_by_name
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_wrong_name_down_a_channel 
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                    .ForPointToPointSendingTo("TestRecieverAddress")
                    .OnlyForMessages().WithNamePattern("SomethingElse")
                .Initialise();

        Because of = () => Bus.Send(new TestNamePatternMessage());

        It should_not_pass_the_message_through = () => GetServer().SentMessages.Should().BeEmpty();
    }
}