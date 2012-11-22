using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplySendSubscriptionHandler : IMessageInputter<MessagePayload>
    {
        readonly ReplySendChannelBuilder builder;
        readonly ChannelDistributor<object> distributor;
        readonly ReplySendChannelSchema schema;

        public ReplySendSubscriptionHandler(
            ReplySendChannelBuilder builder,
            ChannelDistributor<object> distributor,
            ReplySendChannelSchema schema)
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