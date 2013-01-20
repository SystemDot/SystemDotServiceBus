using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplySendChannelDistributor : ChannelDistributor<object>
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ReplySendChannelBuilder builder;
        readonly ReplySendChannelSchema schema;

        public ReplySendChannelDistributor(
            IChangeStore changeStore, 
            ReplyAddressLookup replyAddressLookup, 
            ReplySendChannelBuilder builder,
            ReplySendChannelSchema schema)
            : base(changeStore)
        {
            this.replyAddressLookup = replyAddressLookup;
            this.builder = builder;
            this.schema = schema;
            Id = typeof(ReplySendChannelDistributor).Name + "|" + schema.FromAddress;
        }

        protected override void Distribute(object toDistribute)
        {
            GetChannel(this.replyAddressLookup.GetCurrentSenderAddress()).InputMessage(toDistribute);
        }

        public void RegisterChannel(EndpointAddress address)
        {
            AddChange(new RegisterReplySendChannelChange { Address =  address } );
        }

        public void ApplyChange(RegisterReplySendChannelChange change)
        {
            RegisterChannel(change.Address, () => this.builder.Build(this.schema, change.Address));
        }
    }
}