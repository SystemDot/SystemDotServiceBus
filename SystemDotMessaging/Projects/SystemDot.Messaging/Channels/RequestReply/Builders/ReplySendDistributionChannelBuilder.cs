using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendDistributionChannelBuilder
    {
        readonly IMessageReciever messageReciever;
        readonly ReplySendChannelBuilder builder;
        readonly ReplyAddressLookup replyAddressLookup;
        readonly IChangeStore changeStore;

        public ReplySendDistributionChannelBuilder(
            IMessageReciever messageReciever,
            ReplySendChannelBuilder builder,
            ReplyAddressLookup replyAddressLookup, IChangeStore changeStore)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(builder != null);
            Contract.Requires(replyAddressLookup != null);

            this.messageReciever = messageReciever;
            this.builder = builder;
            this.replyAddressLookup = replyAddressLookup;
            this.changeStore = changeStore;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new ReplySendChannelDistributor(
                this.changeStore, 
                this.replyAddressLookup, 
                this.builder, 
                schema);

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.FromAddress))
                .ToEndPoint(new ReplySendSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(
                    new ReplyChannelMessageFilterStategy(this.replyAddressLookup, schema.FromAddress)))
                .ToEndPoint(distributor);

            distributor.Initialise();
        }
    }
}