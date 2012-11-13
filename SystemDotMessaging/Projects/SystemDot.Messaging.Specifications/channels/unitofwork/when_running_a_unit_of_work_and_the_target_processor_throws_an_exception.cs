using System;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.UnitOfWork;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.unitofwork
{
    [Subject("Unit of work")]
    public class when_running_a_unit_of_work_and_the_target_processor_throws_an_exception : WithSubject<UnitOfWorkRunner>
    {
        static object message;
        static Exception exception;
        static Exception thrownException;

        Establish context = () =>
        {
            message = new object();
            Configure<IIocContainer>(new IocContainer());
            The<IIocContainer>().RegisterInstance<IUnitOfWork, TestUnitOfWork>();

            exception = new Exception();

            Subject.MessageProcessed += _ => { throw exception; };
        };

        Because of = () => thrownException = Catch.Exception(() => Subject.InputMessage(message));

        It should_pass_the_exception_to_end_of_the_unit_of_work = () =>
            The<IIocContainer>()
                .Resolve<IUnitOfWork>()
                .As<TestUnitOfWork>()
                .GetException().ShouldBeTheSameAs(exception);

        It should_rethrow_the_exception = () => thrownException.ShouldBeTheSameAs(exception);
    }
}