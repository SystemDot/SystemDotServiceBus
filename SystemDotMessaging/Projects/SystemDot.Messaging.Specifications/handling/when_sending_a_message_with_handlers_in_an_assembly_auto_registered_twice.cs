using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Specifications.handling.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_handlers_in_an_assembly_auto_registered_twice : WithConfigurationSubject
    {
        static FirstHandlerOfMessage1 messageHandler;

        Establish context = () =>
        {
            var container = IocContainerLocator.Locate();

            messageHandler = new FirstHandlerOfMessage1();
            Register(container, messageHandler);
            Register(container, new SecondHandlerOfMessage1() );

            Messaging.Configuration.Configure.Messaging()
                .RegisterHandlersFromAssemblyOf<when_sending_a_message_with_handlers_in_an_assembly_auto_registered_twice>()
                    .BasedOn<IHandleMessage>()
                .RegisterHandlersFromAssemblyOf<when_sending_a_message_with_handlers_in_an_assembly_auto_registered_twice>()
                    .BasedOn<IHandleMessage>()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .Initialise();
        };

        Because of = () => Bus.SendLocal(new Message1());

        It should_only_send_the_message_to_its_handler_once = () => messageHandler.NumberOfTimeHandleCalled.ShouldEqual(1);
    }
}