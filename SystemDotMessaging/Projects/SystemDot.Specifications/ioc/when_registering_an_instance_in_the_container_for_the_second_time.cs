using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_in_the_container_for_the_second_time : WithSubject<IocContainer>
    {
        static ITestComponent instance;
        
        Because of = () =>
        {
            Subject.RegisterInstance<ITestComponent, TestComponent>();
            instance = Subject.Resolve<ITestComponent>();
            Subject.RegisterInstance<ITestComponent, TestComponent>();
        };

        It should_resolve_the_same_instance_as_with_the_first_registration = () => 
            Subject.Resolve<ITestComponent>().ShouldBeTheSameAs(instance);
    }
}