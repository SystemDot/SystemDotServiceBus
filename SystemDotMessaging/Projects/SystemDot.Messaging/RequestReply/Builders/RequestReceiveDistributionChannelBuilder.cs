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

        public RequestReceiveDistributionChannelBuilder(
            MessageReceiver messageReceiver, 
            RequestRecieveChannelBuilder builder, 
            IChangeStore changeStore)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);
            
            this.messageReceiver = messageReceiver;
            this.builder = builder;
            this.changeStore = changeStore;
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
            return schema.IsDurable ? changeStore : new NullChangeStore();
        }
    }
}