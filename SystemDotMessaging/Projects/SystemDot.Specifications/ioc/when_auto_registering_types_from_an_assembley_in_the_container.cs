using SystemDot.Ioc;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_auto_registering_types_from_an_assembley_in_the_container : WithSubject<IocContainer>
    {
        Because of = () => Subject.RegisterFromAssemblyOf<when_auto_registering_types_from_an_assembley_in_the_container>();

        It should_auto_register_a_concrete_type_by_the_concrete_type = () =>
            Subject.Resolve<TestTypeNotImplementingAnInterface>().ShouldBeOfType<TestTypeNotImplementingAnInterface>();

        It should_auto_register_a_concrete_type_implementing_an_interface_by_the_interface = () =>
            Subject.Resolve<ITestInterface1>().ShouldBeOfType<TestTypeImplementingAnInterface>();
    }
}