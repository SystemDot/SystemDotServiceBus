using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_resolving_an_instance_in_the_container_for_the_second_time : WithSubject<IocContainer>
    {
        static ITestComponent component1;
        static ITestComponent component2;

        Establish context = () =>
        {
            Subject.RegisterInstance<ITestComponent, TestComponent>();
            component1 = Subject.Resolve<ITestComponent>();
        };

        Because of = () => component2 = Subject.Resolve<ITestComponent>();

        It should_be_the_same_object = () => component2.ShouldBeTheSameAs(component1);
    }
}