using SystemDot.Messaging.Transport.InProcess;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestTaskRepeater TaskRepeater;
        protected static TestMessageServer Server;
        
        Establish context = () => Initialise();

        protected static void Initialise()
        {
            TaskRepeater = new TestTaskRepeater();
            ConfigureAndRegister<ITaskRepeater>(TaskRepeater);

            Server = new TestMessageServer();
            ConfigureAndRegister<IInProcessMessageServer>(Server);
        }
    }
}