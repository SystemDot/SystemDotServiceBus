using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications
{
    public class WithHttpConfigurationSubject : WithConfigurationSubject
    {
        protected static TestWebRequestor WebRequestor { get; private set; }

        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            WebRequestor = new TestWebRequestor(Resolve<ISerialiser>());
            ConfigureAndRegister<IWebRequestor>(WebRequestor);
        };
    }
}