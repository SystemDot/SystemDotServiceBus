using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_by_namespace
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_wrong_namespace_down_a_channel
        : WithMessageConfigurationSubject
    {
        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                    .ForRequestReplySendingTo("TestRecieverAddress")
                        .OnlyForMessages().WithNamespace("SomethingElse")
                .Initialise();

        Because of = () => Bus.Send(new TestNamespaceMessage());

        It should_not_pass_the_message_through = () => GetServer().SentMessages.ShouldBeEmpty();
    }
}