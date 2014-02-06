using SystemDot.Core;
using SystemDot.Messaging.Transport.InProcess;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageConfigurationSubject : WithConfigurationSubject
    {
        Establish context = () => RegisterComponents();
        
        protected new static void ReInitialise()
        {
            WithConfigurationSubject.ReInitialise();
            RegisterComponents();
        }

        static void RegisterComponents()
        {
            Register<IInProcessMessageServerFactory>(new TestInProcessMessageServerFactory());
        }

        protected static TestInProcessMessageServer GetServer()
        {
            return Resolve<IInProcessMessageServer>().As<TestInProcessMessageServer>();
        }       
    }
}