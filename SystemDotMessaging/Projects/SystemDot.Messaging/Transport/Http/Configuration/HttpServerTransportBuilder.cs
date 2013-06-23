using System.Diagnostics.Contracts;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Transport.Http.Configuration
{
    class HttpServerTransportBuilder : ITransportBuilder
    {
        readonly IHttpServerBuilder httpServerBuilder;
        readonly ISerialiser serialiser;
        readonly IMessageReceiver messageReceiver;

        public HttpServerTransportBuilder(
            IHttpServerBuilder httpServerBuilder, 
            ISerialiser serialiser, 
            IMessageReceiver messageReceiver)
        {
            Contract.Requires(httpServerBuilder != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageReceiver != null);
            
            this.httpServerBuilder = httpServerBuilder;
            this.serialiser = serialiser;
            this.messageReceiver = messageReceiver;
        }

        public void Build(ServerRoute toListenFor)
        {
            this.httpServerBuilder
                .Build(toListenFor.GetUrl(), BuildMessagingServerHandler())
                .Start();
        }

        IHttpHandler BuildMessagingServerHandler()
        {
            return new HttpMessagingServer(this.serialiser, new MessageReceiverHandler(this.messageReceiver));
        }
    }
}