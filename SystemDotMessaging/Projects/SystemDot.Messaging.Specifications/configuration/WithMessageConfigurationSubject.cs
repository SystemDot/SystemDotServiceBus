using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestTaskRepeater TaskRepeater;
        protected static TestMessageSender MessageSender;
        protected static TestMessageReciever MessageReciever;

        Establish context = () => Initialise();

        protected static void Initialise()
        {
            TaskRepeater = new TestTaskRepeater();
            ConfigureAndRegister<ITaskRepeater>(TaskRepeater);

            MessageReciever = new TestMessageReciever();
            ConfigureAndRegister<IMessageReciever>(MessageReciever);

            MessageSender = new TestMessageSender();
            ConfigureAndRegister<IMessageSender>(MessageSender);
        }
    }
}