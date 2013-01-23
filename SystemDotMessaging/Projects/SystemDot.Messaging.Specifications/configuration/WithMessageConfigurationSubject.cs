using SystemDot.Messaging.Transport;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestMessageSender MessageSender;
        protected static TestMessageReciever MessageReciever;

        Establish context = () =>
        {
            MessageReciever = new TestMessageReciever();
            ConfigureAndRegister<IMessageReciever>(MessageReciever);

            MessageSender = new TestMessageSender();
            ConfigureAndRegister<IMessageSender>(MessageSender);
        };
    }
}