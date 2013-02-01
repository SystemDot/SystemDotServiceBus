using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Distribution;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.RequestReply
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