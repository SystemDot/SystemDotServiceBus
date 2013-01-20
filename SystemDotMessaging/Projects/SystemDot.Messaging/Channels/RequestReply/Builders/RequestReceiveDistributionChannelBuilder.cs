using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Packaging;
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
            
            this.messageReciever = messageReciever;
            this.builder = builder;
            this.changeStore = changeStore;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var channelDistributor = new RequestRecieveChannelDistributor(
                this.changeStore, 
                this.builder, 
                schema);

            MessagePipelineBuilder.Build()  
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestRecieveSubscriptionHandler(channelDistributor));

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(channelDistributor);

            channelDistributor.Initialise();
        }
    }
}