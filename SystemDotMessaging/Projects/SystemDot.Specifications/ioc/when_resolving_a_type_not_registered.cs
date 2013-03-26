using System;
using SystemDot.Ioc;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Specifications.ioc
{
    [Subject("Ioc")]
    public class when_resolving_a_type_not_registered : WithSubject<IocContainer>
    {
        static Exception exception;

        Because of = () => exception = Catch.Exception(() => Subject.Resolve<object>());

        It should_throw_a_type_not_registered_exception = () => exception.ShouldBeOfType<TypeNotRegisteredException>();

        It should_say_which_type_was_not_registered = () => exception.Message.ShouldEqual("Type \"Object\" has not been registered in the container.");
    }
}