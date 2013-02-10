using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.InProcess;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestTaskRepeater TaskRepeater;
        protected static TestMessageServer MessageServer;
        
        Establish context = () => Initialise();

        protected static void Initialise()
        {
            TaskRepeater = new TestTaskRepeater();
            ConfigureAndRegister<ITaskRepeater>(TaskRepeater);

            MessageServer = new TestMessageServer();
            ConfigureAndRegister<IInProcessMessageServer>(MessageServer);
        }
    }
}