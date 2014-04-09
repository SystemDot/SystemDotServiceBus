using System.Linq;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.filtering_by_namespace
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_correct_namespace_down_a_channel 
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                .ForRequestReplySendingTo("TestRecieverAddress")
                .OnlyForMessages().WithNamespace("SystemDot.Messaging.Specifications.filtering_by_namespace")
                .Initialise();

        Because of = () => Bus.Send(new TestNamespaceMessage());

        It should_pass_the_message_through = () =>
            GetServer().SentMessages.First().DeserialiseTo<TestNamespaceMessage>().Should().NotBeNull();
    }
}