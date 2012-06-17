using SystemDot.Messaging.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Configuration")]
    public class when_setting_a_component_in_the_environment
    {
        static ITestComponent component;

        Establish context = () =>
        {
            component = new TestComponent();
        };

        Because of = () => MessagingEnvironment.SetComponent<ITestComponent>(component);

        It should_be_able_to_retrieved = () => 
            MessagingEnvironment.GetComponent<ITestComponent>().ShouldBeTheSameAs(component);

    }
}