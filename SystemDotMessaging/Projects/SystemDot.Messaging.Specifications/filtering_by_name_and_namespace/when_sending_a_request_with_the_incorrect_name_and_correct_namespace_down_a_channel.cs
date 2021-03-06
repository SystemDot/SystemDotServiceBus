using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_by_name_and_namespace
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_incorrect_name_and_correct_namespace_down_a_channel
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                .ForPointToPointSendingTo("TestRecieverAddress")
                .OnlyForMessages().WithNamespaceAndNamePattern("filtering_by_name_and_namespace", "other")
                .Initialise();

        Because of = () => Bus.Send(new TestNameAndNamespaceMessage());

        It should_not_pass_the_message_through = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}