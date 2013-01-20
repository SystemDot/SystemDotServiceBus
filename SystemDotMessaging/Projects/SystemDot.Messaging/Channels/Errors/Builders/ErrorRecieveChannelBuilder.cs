using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Errors.Builders
{
    public class ErrorRecieveChannelBuilder
    {
        readonly MessageCacheFactory messageCacheFactory;
        readonly ErrorReciever errorReciever;
        readonly AcknowledgementSender sender;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public ErrorRecieveChannelBuilder(MessageCacheFactory messageCacheFactory, ErrorReciever errorReciever, AcknowledgementSender sender, MessageAcknowledgementHandler acknowledgementHandler)
        {
            this.messageCacheFactory = messageCacheFactory;
            this.errorReciever = errorReciever;
            this.sender = sender;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public void Build(EndpointAddress address)
        {
            MessageCache messageCache = this.messageCacheFactory.CreateCache(PersistenceUseType.Error, address);

            this.acknowledgementHandler.RegisterCache(messageCache);

            MessagePipelineBuilder.Build()
                .With(this.errorReciever)
                .ToProcessor(new PersistenceSourceRecorder())
                .ToProcessor(new MessageSendTimeRemover())
                .ToProcessor(new MessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.sender))
                .ToEndPoint(new NullEndpoint());
        }
    }
}