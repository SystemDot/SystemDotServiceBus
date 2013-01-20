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
            this.builder = builder;
            this.schema = schema;
            Id = typeof(RequestRecieveChannelDistributor).Name + "|" + schema.Address;
        }

        protected override void Distribute(MessagePayload toDistribute)
        {
            GetChannel(toDistribute.GetFromAddress()).InputMessage(toDistribute);
        }

        public void RegisterChannel(EndpointAddress address)
        {
            AddChange(new RegisterRecieveChannelChange { Address = address });
        }

        public void ApplyChange(RegisterRecieveChannelChange change)
        {
            RegisterChannel(change.Address, () => this.builder.Build(this.schema, change.Address));
        }
    }
}