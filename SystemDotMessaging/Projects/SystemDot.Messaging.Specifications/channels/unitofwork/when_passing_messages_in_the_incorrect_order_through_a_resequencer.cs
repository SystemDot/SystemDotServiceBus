using SystemDot.Ioc;
using SystemDot.Messaging.Channels.UnitOfWork;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.unitofwork
{
    [Subject("Unit of work")]
    public class when_running_a_unit_of_work : WithSubject<UnitOfWorkRunner>
    {
        static object message;
        static object processedMessage;
        
        Establish context = () =>
        {
            message = new object();
            Configure<IIocContainer>(new IocContainer());
            The<IIocContainer>().RegisterInstance<IUnitOfWork, TestUnitOfWork>();

            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_begin_the_unit_of_work = () => 
            The<IIocContainer>()
                .Resolve<IUnitOfWork>()
                .As<TestUnitOfWork>()
                .HasBegun().ShouldBeTrue();

        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

        It should_end_the_unit_of_work = () => 
            The<IIocContainer>()
                .Resolve<IUnitOfWork>()
                .As<TestUnitOfWork>()
                .HasEnded().ShouldBeTrue();
    }
}