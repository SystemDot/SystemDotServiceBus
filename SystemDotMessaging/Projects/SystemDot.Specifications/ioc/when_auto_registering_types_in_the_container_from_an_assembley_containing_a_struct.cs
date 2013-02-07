using System;
using SystemDot.Ioc;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_auto_registering_types_in_the_container_from_an_assembley_containing_a_struct : WithSubject<IocContainer>
    {
        static Exception exception;

        Because of = () => exception =
                Catch.Exception(() => Subject.RegisterFromAssemblyOf<when_auto_registering_types_in_the_container_from_an_assembley_containing_a_struct>());

        It should_not_throw_any_exceptions = () => exception.ShouldBeNull();
    }
}