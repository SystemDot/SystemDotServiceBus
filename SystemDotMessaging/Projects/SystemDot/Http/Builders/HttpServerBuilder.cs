namespace SystemDot.Http.Builders
{
    public class HttpServerBuilder : IHttpServerBuilder
    {
        public IHttpServer Build(FixedPortAddress address, IHttpHandler handler)
        {
            return new HttpServer(address, handler);
        }
    }
}