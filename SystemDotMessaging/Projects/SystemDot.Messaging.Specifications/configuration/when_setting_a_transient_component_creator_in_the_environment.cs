using SystemDot.Messaging.Configuration.ComponentRegistration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Configuration")]
    public class when_setting_a_transient_component_creator_in_the_environment
    {
        Because of = () => MessagingEnvironment.RegisterComponent<ITestComponent>(() => new TestComponent());

        It should_be_created_and_able_to_retrieved = () => 
            MessagingEnvironment.GetComponent<ITestComponent>().ShouldBeOfType<TestComponent>();

    }
}