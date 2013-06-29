using System.Linq;
using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.filtering_by_name
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_request_with_the_correct_name_down_a_channel 
        : WithMessageConfigurationSubject
    {
        Establish context = () => 
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Test")
                    .ForPointToPointSendingTo("TestRecieverAddress")
                    .OnlyForMessages(FilteredBy.NamePattern("Name"))
                .Initialise();

        Because of = () => Bus.Send(new TestNamePatternMessage());

        It should_pass_the_message_through = () => 
           Server.SentMessages.First().DeserialiseTo<TestNamePatternMessage>().ShouldNotBeNull();
    }
}