using System.Diagnostics.Contracts;
using System.IO;

namespace SystemDot.Http
{
    [ContractClass(typeof(IHttpHandlerContract))]
    public interface IHttpHandler
    {
        void HandleRequest(Stream inputStream, Stream outputStream);
    }

    [ContractClassFor(typeof(IHttpHandler))]
    public class IHttpHandlerContract : IHttpHandler
    {
        public void HandleRequest(Stream inputStream, Stream outputStream)
        {
            Contract.Requires(inputStream != null);
            Contract.Requires(inputStream.CanRead);
            Contract.Ensures(inputStream.CanRead);
            
            Contract.Requires(outputStream != null);
            Contract.Requires(outputStream.CanRead);
            Contract.Ensures(outputStream.CanRead);
        }
    }
}