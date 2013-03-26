using SystemDot.Ioc;
using SystemDot.Specifications.ioc.TestTypes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_registering_an_instance_in_the_container : WithSubject<IocContainer>
    {
        static ITestComponent resolvedByGenericParamater;
        static ITestComponent resolvedByTypeOf;

        Establish context = () => Subject.RegisterInstance<ITestComponent, TestComponent>();

        Because of = () =>
        {
            resolvedByGenericParamater = Subject.Resolve<ITestComponent>();
            resolvedByTypeOf = Subject.Resolve(typeof(ITestComponent)).As<ITestComponent>();
        };

        It should_be_able_to_be_resolved_by_generic_parameter = () => resolvedByGenericParamater.ShouldBeOfType<TestComponent>();

        It should_be_able_to_be_resolved_by_type_of = () =>  resolvedByTypeOf.ShouldBeOfType<TestComponent>();
    }
}