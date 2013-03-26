using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Specifications.channels.handling.Fakes;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.handling
{
    [Subject("Configuration")]
    public class when_sending_a_message_with_handlers_auto_registered : WithConfigurationSubject
    {
        static IBus bus;
        static Message1 message1;
        static Message2 message2;
        static FirstHandlerOfMessage1 firstHandlerOfMessage1;
        static SecondHandlerOfMessage1 secondHandlerOfMessage1;
        static FirstHandlerOfMessage2 firstHandlerOfMessage2;
        static SecondHandlerOfMessage2 secondHandlerOfMessage2;

        Establish context = () =>
        {
            firstHandlerOfMessage1 = new FirstHandlerOfMessage1();
            Register(firstHandlerOfMessage1);
            secondHandlerOfMessage1 = new SecondHandlerOfMessage1();
            Register(secondHandlerOfMessage1);
            firstHandlerOfMessage2 = new FirstHandlerOfMessage2();
            Register(firstHandlerOfMessage2);
            secondHandlerOfMessage2 = new SecondHandlerOfMessage2();
            Register(secondHandlerOfMessage2);

            message1 = new Message1();
            message2 = new Message2();

            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .RegisterHandlersFromAssemblyOf<when_sending_a_message_with_handlers_auto_registered>()
                .BasedOn<IHandleMessage>()
                .Initialise();
        };

        Because of = () =>
        {
            bus.SendLocal(message1);
            bus.SendLocal(message2);
        };

        It should_send_the_first_message_to_its_first_handler =
            () => firstHandlerOfMessage1.LastHandledMessage.ShouldEqual(message1);

        It should_send_the_first_message_to_its_second_handler =
            () => secondHandlerOfMessage1.LastHandledMessage.ShouldEqual(message1);

        It should_send_the_second_message_to_its_first_handler =
            () => firstHandlerOfMessage2.LastHandledMessage.ShouldEqual(message2);

        It should_send_the_second_message_to_its_second_handler =
            () => secondHandlerOfMessage2.LastHandledMessage.ShouldEqual(message2);
    }
}