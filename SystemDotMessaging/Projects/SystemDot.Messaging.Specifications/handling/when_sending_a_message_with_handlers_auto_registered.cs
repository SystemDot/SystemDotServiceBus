using SystemDot.Ioc;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Specifications.handling.Fakes;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.handling
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_handlers_auto_registered : WithConfigurationSubject
    {
        static Message1 message1;
        static Message2 message2;
        static FirstHandlerOfMessage1 firstHandlerOfMessage1;
        static SecondHandlerOfMessage1 secondHandlerOfMessage1;
        static FirstHandlerOfMessage2 firstHandlerOfMessage2;
        static SecondHandlerOfMessage2 secondHandlerOfMessage2;

        Establish context = () =>
        {
            var container = IocContainerLocator.Locate();

            firstHandlerOfMessage1 = new FirstHandlerOfMessage1();
            Register(container, firstHandlerOfMessage1);

            secondHandlerOfMessage1 = new SecondHandlerOfMessage1();
            Register(container, secondHandlerOfMessage1);

            firstHandlerOfMessage2 = new FirstHandlerOfMessage2();
            Register(container, firstHandlerOfMessage2);

            secondHandlerOfMessage2 = new SecondHandlerOfMessage2();
            Register(container, secondHandlerOfMessage2);

            message1 = new Message1();
            message2 = new Message2();

            Configuration.Configure.Messaging()
                .ResolveReferencesWith(container)
                .RegisterHandlersFromContainer().BasedOn<IHandleMessage>()
                .UsingInProcessTransport()
                .OpenDirectChannel()
                .Initialise();
        };

        Because of = () =>
        {
            Bus.SendDirect(message1);
            Bus.SendDirect(message2);
        };

        It should_send_the_first_message_to_its_first_handler =
            () => firstHandlerOfMessage1.LastHandledMessage.ShouldBeEquivalentTo(message1);

        It should_send_the_first_message_to_its_second_handler =
            () => secondHandlerOfMessage1.LastHandledMessage.ShouldBeEquivalentTo(message1);

        It should_send_the_second_message_to_its_first_handler =
            () => firstHandlerOfMessage2.LastHandledMessage.ShouldBeEquivalentTo(message2);

        It should_send_the_second_message_to_its_second_handler =
            () => secondHandlerOfMessage2.LastHandledMessage.ShouldBeEquivalentTo(message2);
    }
}