using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Serialisation;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications
{
    public class WithHttpConfigurationSubject : WithConfigurationSubject
    {
        protected static TestWebRequestor WebRequestor { get; private set; }

        Establish context = () => RegisterServerComponents();

        static void RegisterServerComponents()
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            WebRequestor = new TestWebRequestor(Resolve<ISerialiser>());
            ConfigureAndRegister<IWebRequestor>(WebRequestor);
        }

        protected new static void ReInitialise()
        {
            WithConfigurationSubject.ReInitialise();
            RegisterServerComponents();
        }
    }
}