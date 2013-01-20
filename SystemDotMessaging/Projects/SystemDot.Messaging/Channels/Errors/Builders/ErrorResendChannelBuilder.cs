using System;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.Errors.Builders
{
    public class ErrorResendChannelBuilder
    {
        readonly MessageCacheFactory messageCacheFactory;
        readonly IMessageSender sender;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly EndpointAddressBuilder addressBuilder;

        public ErrorResendChannelBuilder(MessageCacheFactory messageCacheFactory, IMessageSender sender, MessageAcknowledgementHandler acknowledgementHandler, EndpointAddressBuilder addressBuilder)
        {
            this.messageCacheFactory = messageCacheFactory;
            this.sender = sender;
            this.acknowledgementHandler = acknowledgementHandler;
            this.addressBuilder = addressBuilder;
        }

        public ErrorResender Build()
        {
            EndpointAddress address = this.addressBuilder.Build("errors", Environment.MachineName);

            MessageCache messageCache = this.messageCacheFactory.CreateCache(PersistenceUseType.Error, address);

            this.acknowledgementHandler.RegisterCache(messageCache);

            var errorResender = new ErrorResender(messageCache);

            MessagePipelineBuilder.Build()
                .With(errorResender)
                .ToEndPoint(this.sender);

            return errorResender;
        }
    }
}