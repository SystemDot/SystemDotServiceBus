using SystemDot.Ioc;
using SystemDot.Messaging.UnitOfWork;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.unitofwork
{
    [Subject("Unit of work")]
    public class when_running_a_unit_of_work : WithSubject<UnitOfWorkRunner<TestUnitOfWorkFactory>>
    {
        static object message;
        static object processedMessage;
        static TestUnitOfWork unitOfWork;

        Establish context = () =>
        {
            message = new object();
            Configure<IIocContainer>(new IocContainer());
            
            unitOfWork = new TestUnitOfWork();
            The<IIocContainer>().RegisterInstance<TestUnitOfWorkFactory>(() => new TestUnitOfWorkFactory(unitOfWork));

            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_begin_the_unit_of_work = () => unitOfWork.HasBegun().ShouldBeTrue();

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_end_the_unit_of_work = () => unitOfWork.HasEnded().ShouldBeTrue();
    }
}