using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_in_the_container : WithSubject<IocContainer>
    {
            
        Because of = () => Subject.RegisterInstance<ITestComponent, TestComponent>();

        It should_be_able_to_be_resolved = () => Subject.Resolve<ITestComponent>().ShouldBeOfType<TestComponent>();
    }
}