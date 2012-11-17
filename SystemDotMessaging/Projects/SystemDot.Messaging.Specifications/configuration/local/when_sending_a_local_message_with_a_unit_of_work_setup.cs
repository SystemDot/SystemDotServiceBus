using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.UnitOfWork;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.local
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_local_message_with_a_unit_of_work_setup : WithMessageConfigurationSubject
    {
        static int message;
        static IBus bus;
        
        Establish context = () =>
        {
            ConfigureAndRegister<IUnitOfWork>(new TestUnitOfWork());
            
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Anything").ForRequestReplyRecieving()
                .Initialise();

            message = 1;

            var handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => bus.SendLocal(message);

        It should_begin_the_unit_of_work = () =>
            Resolve<IUnitOfWork>().As<TestUnitOfWork>().HasBegun().ShouldBeTrue();
    }
}