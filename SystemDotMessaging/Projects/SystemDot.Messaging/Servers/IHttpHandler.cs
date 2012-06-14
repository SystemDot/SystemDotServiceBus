using System.IO;

namespace SystemDot.Messaging.Servers
{
    public interface IHttpHandler
    {
        void HandleRequest(Stream inputStream);
        void Respond(Stream outputStream);
    }
}