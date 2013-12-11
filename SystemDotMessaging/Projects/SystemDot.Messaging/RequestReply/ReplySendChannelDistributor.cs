using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Distribution;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.RequestReply
{
    class ReplySendChannelDistributor : ChannelDistributor<object>
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ReplySendChannelBuilder builder;
        readonly ReplySendChannelSchema schema;
    
        public ReplySendChannelDistributor(
            ChangeStore changeStore, 
            ReplyAddressLookup replyAddressLookup, 
            ReplySendChannelBuilder builder,
            ReplySendChannelSchema schema,
            ICheckpointStrategy checkPointStrategy)
            : base(changeStore, checkPointStrategy)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(builder != null);
            Contract.Requires(schema != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.builder = builder;
            this.schema = schema;

            Id = string.Concat(this.GetType().Name, "|", this.schema.FromAddress);
        }

        protected override void Distribute(object toDistribute)
        {
            GetChannel(this.replyAddressLookup.GetCurrentSenderAddress()).InputMessage(toDistribute);
        }

        protected override void AddRegisterChannelChange(EndpointAddress address)
        {
            AddChange(new RegisterReplySendChannelChange { Address = address });
        }

        public void ApplyChange(RegisterReplySendChannelChange change)
        {
            RegisterChannel(change.Address, () => this.builder.Build(this.schema, change.Address));
        }
    }
}