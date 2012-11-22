using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveSubscriptionHandler : IMessageInputter<MessagePayload>
    {
        readonly RequestRecieveChannelBuilder builder;
        readonly ChannelDistributor<MessagePayload> distributor;
        readonly RequestRecieveChannelSchema schema;

        public RequestRecieveSubscriptionHandler(
            RequestRecieveChannelBuilder builder, 
            ChannelDistributor<MessagePayload> distributor, 
            RequestRecieveChannelSchema schema)
        {
            Contract.Requires(builder != null);
            Contract.Requires(distributor != null);
            Contract.Requires(schema != null);
            
            this.builder = builder;
            this.distributor = distributor;
            this.schema = schema;
        }

        public void InputMessage(MessagePayload toInput)
        {
            this.distributor.RegisterChannel(
                toInput.GetFromAddress(), 
                () => this.builder.Build(this.schema, toInput.GetFromAddress()));
        }
    }
}