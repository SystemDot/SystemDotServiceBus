using System.IO;

namespace SystemDot.Http
{
    public interface IHttpHandler
    {
        void HandleRequest(Stream inputStream, Stream outputStream);
    }
}