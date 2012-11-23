using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendDistributionChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly ReplySendChannelBuilder builder;
        readonly ReplySendChannelDistrbutionStrategy channelDistrbutionStrategy;
        readonly ReplyAddressLookup replyAddressLookup;

        public ReplySendDistributionChannelBuilder(
            IMessageReciever messageReciever,
            ReplySendChannelBuilder builder,
            ReplySendChannelDistrbutionStrategy channelDistrbutionStrategy, 
            ReplyAddressLookup replyAddressLookup)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(builder != null);
            Contract.Requires(channelDistrbutionStrategy != null);
            Contract.Requires(replyAddressLookup != null);

            this.messageReciever = messageReciever;
            this.builder = builder;
            this.channelDistrbutionStrategy = channelDistrbutionStrategy;
            this.replyAddressLookup = replyAddressLookup;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new ChannelDistributor<object>(this.channelDistrbutionStrategy);

            MessagePipelineBuilder.Build()      
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.FromAddress))
                .ToEndPoint(new ReplySendSubscriptionHandler(this.builder, distributor, schema));

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(new ReplyChannelMessageFilterStategy(this.replyAddressLookup, schema.FromAddress)))
                .ToEndPoint(distributor);
        }
    }
}