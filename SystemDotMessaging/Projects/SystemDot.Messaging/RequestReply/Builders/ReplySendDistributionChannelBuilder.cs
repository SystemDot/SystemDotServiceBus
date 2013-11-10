using System.Diagnostics.Contracts;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendDistributionChannelBuilder
    {
        readonly MessageReceiver messageReceiver;
        readonly ReplySendChannelBuilder builder;
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ChangeStoreSelector changeStoreSelector;
        
        public ReplySendDistributionChannelBuilder(
            MessageReceiver messageReceiver,
            ReplySendChannelBuilder builder, 
            ReplyAddressLookup replyAddressLookup,
            ChangeStoreSelector changeStoreSelector)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(builder != null);
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(changeStoreSelector != null);

            this.messageReceiver = messageReceiver;
            this.builder = builder;
            this.replyAddressLookup = replyAddressLookup;
            this.changeStoreSelector = changeStoreSelector;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new ReplySendChannelDistributor(
                changeStoreSelector.SelectChangeStore(schema), 
                replyAddressLookup, 
                builder, 
                schema);

            MessagePipelineBuilder.Build()      
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.FromAddress))
                .ToEndPoint(new ReplySendSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(CreateReplyChannelMessageFilter(schema))
                .ToEndPoint(distributor);

            distributor.Initialise();
        }

        MessageFilter CreateReplyChannelMessageFilter(ReplySendChannelSchema schema)
        {
            return new MessageFilter(CreateReplyChannelMessageFilterStategy(schema));
        }

        ReplyChannelMessageFilterStategy CreateReplyChannelMessageFilterStategy(ReplySendChannelSchema schema)
        {
            return new ReplyChannelMessageFilterStategy(replyAddressLookup, schema.FromAddress);
        }
    }
}