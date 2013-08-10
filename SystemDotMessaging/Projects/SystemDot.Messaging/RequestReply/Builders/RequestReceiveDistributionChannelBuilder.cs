using System.Diagnostics.Contracts;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestReceiveDistributionChannelBuilder 
    {
        readonly MessageReceiver messageReceiver;
        readonly RequestRecieveChannelBuilder builder;
        readonly IChangeStore changeStore;
        readonly InMemoryChangeStore inMemoryStore;

        public RequestReceiveDistributionChannelBuilder(
            MessageReceiver messageReceiver, 
            RequestRecieveChannelBuilder builder, 
            IChangeStore changeStore, 
            InMemoryChangeStore inMemoryStore)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(inMemoryStore != null);
            
            this.messageReceiver = messageReceiver;
            this.builder = builder;
            this.changeStore = changeStore;
            this.inMemoryStore = inMemoryStore;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new RequestRecieveChannelDistributor(GetChangeStore(schema), builder, schema);
            
            MessagePipelineBuilder.Build()  
                .With(this.messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestReceiveSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .With(this.messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(distributor);

            distributor.Initialise();
        }

        IChangeStore GetChangeStore(RequestRecieveChannelSchema schema)
        {
            return schema.IsDurable ? this.changeStore : this.inMemoryStore;
        }
    }
}