using SystemDot.Http;
using SystemDot.Http.Builders;

namespace SystemDot.Messaging.Specifications
{
    public class TestHttpServerBuilder : IHttpServerBuilder
    {
        public IHttpServer Build(FixedPortAddress address, IHttpHandler handler)
        {
            return new TestHttpServer(address, handler);
        }
    }
}