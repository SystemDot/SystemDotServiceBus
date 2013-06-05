using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.InProcess;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        protected static TestMessageServer Server;

        Establish context = () => RegisterComponents();
        
        protected new static void ReInitialise()
        {
            WithConfigurationSubject.ReInitialise();
            RegisterComponents();
        }

        static void RegisterComponents()
        {
            Server = new TestMessageServer();
            ConfigureAndRegister<IInProcessMessageServer>(Server);

            ConfigureAndRegister<MessageHandlerRouter>(new MessageHandlerRouter());
        }
    }
}