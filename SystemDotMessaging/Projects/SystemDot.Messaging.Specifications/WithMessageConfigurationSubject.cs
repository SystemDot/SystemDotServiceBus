using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.InProcess;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestTaskRepeater TaskRepeater;
        protected static TestMessageServer Server;

        Establish context = () => Initialise();
        
        protected static void Initialise()
        {
            ConfigureAndRegister<ISerialiser>(new PlatformAgnosticSerialiser());

            TaskRepeater = new TestTaskRepeater();
            ConfigureAndRegister<ITaskRepeater>(TaskRepeater);

            Server = new TestMessageServer();
            ConfigureAndRegister<IInProcessMessageServer>(Server);

            ConfigureAndRegister<MessageHandlerRouter>(new MessageHandlerRouter());
        }
    }
}