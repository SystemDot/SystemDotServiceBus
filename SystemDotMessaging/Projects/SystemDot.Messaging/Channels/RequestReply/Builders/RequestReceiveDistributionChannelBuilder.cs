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

        public RequestReceiveDistributionChannelBuilder(
            IMessageReciever messageReciever, 
            RequestRecieveChannelBuilder builder, 
            IChangeStore changeStore)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(builder != null);
            Contract.Requires(changeStore != null);
            
            this.messageReciever = messageReciever;
            this.builder = builder;
            this.changeStore = changeStore;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new RequestRecieveChannelDistributor(this.changeStore, this.builder, schema);

            MessagePipelineBuilder.Build()  
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestRecieveSubscriptionHandler(distributor));

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(distributor);
        }
    }
}