using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestReceiveDistributionChannelBuilder 
    {
        readonly IMessageReciever messageReciever;
        readonly RequestRecieveChannelBuilder builder;
        readonly RequestRecieveChannelDistrbutionStrategy channelDistrbutionStrategy;

        public RequestReceiveDistributionChannelBuilder(
            IMessageReciever messageReciever, 
            RequestRecieveChannelBuilder builder, 
            RequestRecieveChannelDistrbutionStrategy channelDistrbutionStrategy)
        {
            Contract.Requires(messageReciever != null);
            Contract.Requires(builder != null);
            Contract.Requires(channelDistrbutionStrategy != null);
            
            this.messageReciever = messageReciever;
            this.builder = builder;
            this.channelDistrbutionStrategy = channelDistrbutionStrategy;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            Contract.Requires(schema != null);

            var distributor = new ChannelDistributor<MessagePayload>(this.channelDistrbutionStrategy);

            MessagePipelineBuilder.Build()  
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(new RequestRecieveSubscriptionHandler(this.builder, distributor, schema));

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEndPoint(distributor);
        }
    }
}