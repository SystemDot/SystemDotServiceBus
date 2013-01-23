using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class RequestRecieveChannelDistributor : ChannelDistributor<MessagePayload>
    {
        readonly RequestRecieveChannelBuilder builder;
        readonly RequestRecieveChannelSchema schema;

        public RequestRecieveChannelDistributor(
            IChangeStore changeStore, 
            RequestRecieveChannelBuilder builder, 
            RequestRecieveChannelSchema schema) 
            : base(changeStore)
        {
            Contract.Requires(changeStore != null);
            Contract.Requires(builder != null);
            Contract.Requires(schema != null);
            
            this.builder = builder;
            this.schema = schema;

            Id = string.Concat(this.GetType().Name, "|", this.schema.Address);
        }

        protected override void Distribute(MessagePayload toDistribute)
        {
            GetChannel(toDistribute.GetFromAddress()).InputMessage(toDistribute);
        }

        public void RegisterChannel(EndpointAddress address)
        {
            AddChange(new RegisterRequestReceiveChannelChange() { Address = address });
        }

        public void ApplyChange(RegisterRequestReceiveChannelChange change)
        {
            RegisterChannel(change.Address, () => this.builder.Build(this.schema, change.Address));
        }
    }
}