using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_auto_registering_types_in_the_container_from_an_assembly_the_second_time : WithSubject<IocContainer>
    {
        static ITestInterface1 instance1;
        static ITestInterface1 instance2;

        Establish on = () =>
        {
            Subject.RegisterFromAssemblyOf<when_auto_registering_types_in_the_container_from_an_assembly_the_second_time>();
            instance1 = Subject.Resolve<ITestInterface1>();
        };

        Because of = () =>
        {
            Subject.RegisterFromAssemblyOf<when_auto_registering_types_in_the_container_from_an_assembly_the_second_time>();
            instance2 = Subject.Resolve<ITestInterface1>();
        };

        It should_resolve_the_same_instance_as_with_the_first_registration = () =>
            instance1.ShouldBeTheSameAs(instance2);
    }
}