using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplySendDistributionChannelBuilder
    {
        readonly IMessageReceiver messageReceiver;
        readonly ReplySendChannelBuilder builder;
        readonly ReplyAddressLookup replyAddressLookup;
        readonly IChangeStore changeStore;
        readonly InMemoryChangeStore inMemoryStore;

        public ReplySendDistributionChannelBuilder(
            IMessageReceiver messageReceiver,
            ReplySendChannelBuilder builder, 
            ReplyAddressLookup replyAddressLookup, 
            IChangeStore changeStore, 
            InMemoryChangeStore inMemoryStore)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(builder != null);
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(inMemoryStore != null);

            this.messageReceiver = messageReceiver;
            this.builder = builder;
            this.replyAddressLookup = replyAddressLookup;
            this.changeStore = changeStore;
            this.inMemoryStore = inMemoryStore;
        }

        public void Build(ReplySendChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new ReplySendChannelDistributor(
                GetChangeStore(schema), 
                replyAddressLookup, 
                builder, 
                schema);

            MessagePipelineBuilder.Build()      
                .With(this.messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.FromAddress))
                .ToEndPoint(new ReplySendSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .WithBusReplyTo(new MessageFilter(
                    new ReplyChannelMessageFilterStategy(replyAddressLookup, schema.FromAddress)))
                .ToEndPoint(distributor);

            distributor.Initialise();
        }

        IChangeStore GetChangeStore(ReplySendChannelSchema schema)
        {
            return schema.IsDurable ? this.changeStore : this.inMemoryStore;
        }
    }
}