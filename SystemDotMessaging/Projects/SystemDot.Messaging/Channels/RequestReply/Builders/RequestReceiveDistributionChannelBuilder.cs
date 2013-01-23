using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestReceiveDistributionChannelBuilder 
    {
        readonly IMessageReciever messageReciever;
        readonly RequestRecieveChannelBuilder builder;
        readonly IChangeStore changeStore;
        readonly InMemoryChangeStore inMemoryStore;

        public RequestReceiveDistributionChannelBuilder(
            IMessageReciever messageReciever, 
            RequestRecieveChannelBuilder builder, 
            IChangeStore changeStore, 
            InMemoryChangeStore inMemoryStore)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(inMemoryStore != null);
            
            this.messageReciever = messageReciever;
            this.builder = builder;
            this.changeStore = changeStore;
            this.inMemoryStore = inMemoryStore;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new RequestRecieveChannelDistributor(GetChangeStore(schema), this.builder, schema);
            
            MessagePipelineBuilder.Build()  
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestRecieveSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
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