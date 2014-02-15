using SystemDot.Ioc;
using SystemDot.Messaging.Specifications.handling.Fakes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.handling_with_external_ioc_resolution
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_handlers_in_an_assembly : WithConfigurationSubject
    {
        static FirstHandlerOfMessage1 messageHandler;
        static Message1 message;

        Establish context = () =>
        {
            var container = new IocContainer();

            messageHandler = new FirstHandlerOfMessage1();
            Register(container, messageHandler);
            Register(container, new SecondHandlerOfMessage1());

            Configuration.Configure.Messaging()
                .ResolveReferencesWith(container)
                .RegisterHandlersFromContainer().BasedOn<IHandleMessage>()
                .UsingInProcessTransport()
                .OpenDirectChannel()
                .Initialise();
            message = new Message1();
        };

        Because of = () => Bus.SendDirect(message);

        It should_send_the_message_to_its_handler = () => messageHandler.LastHandledMessage.Should().BeSameAs(message);
    }
}