using SystemDot.Messaging.Configuration.ComponentRegistration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    [Subject("Configuration")]
    public class when_setting_a_transient_component_creator_with_one_constructor_argument_in_the_environment
    {
        Because of = () => 
            IocContainer.Register<ITestComponent, string>(s => new TestComponent(s));

        It should_be_created_and_able_to_retrieved = () => 
            IocContainer.Resolve<ITestComponent, string>("Test")
                .ConstructorArgument1.ShouldEqual("Test");

    }
}