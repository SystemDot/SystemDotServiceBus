namespace SystemDot.Http.Builders
{
    public interface IHttpServerBuilder
    {
        IHttpServer Build(FixedPortAddress address, IHttpHandler handler);
    }
}