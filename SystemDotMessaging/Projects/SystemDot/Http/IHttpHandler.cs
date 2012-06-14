using System.IO;

namespace SystemDot.Http
{
    public interface IHttpHandler
    {
        void HandleRequest(Stream inputStream);
        void Respond(Stream outputStream);
    }
}