using System.Diagnostics.Contracts;
using SystemDot.Messaging.Builders;
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
        readonly ChangeStoreSelector changeStoreSelector;
        readonly ICheckpointStrategy checkPointStrategy;

        public RequestReceiveDistributionChannelBuilder(
            MessageReceiver messageReceiver, 
            RequestRecieveChannelBuilder builder,
            ChangeStoreSelector changeStoreSelector,
            ICheckpointStrategy checkPointStrategy)
        {
            Contract.Requires(messageReceiver != null);
            Contract.Requires(builder != null);
            Contract.Requires(changeStoreSelector != null);
            
            this.messageReceiver = messageReceiver;
            this.builder = builder;
            this.changeStoreSelector = changeStoreSelector;
            this.checkPointStrategy = checkPointStrategy;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new RequestRecieveChannelDistributor(SelectChangeStore(schema), builder, schema, checkPointStrategy);
            
            MessagePipelineBuilder.Build()  
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestReceiveSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .With(messageReceiver)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(distributor);

            distributor.Initialise();
        }

        ChangeStore SelectChangeStore(RequestRecieveChannelSchema schema)
        {
            return changeStoreSelector.SelectChangeStore(schema);
        }
    }
}