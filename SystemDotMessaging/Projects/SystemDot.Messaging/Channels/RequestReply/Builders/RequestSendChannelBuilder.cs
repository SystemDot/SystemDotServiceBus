using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelBuilder : IRequestSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;

        public RequestSendChannelBuilder(IMessageSender messageSender, ISerialiser serialiser)
        {
            this.messageSender = messageSender;
            this.serialiser = serialiser;
        }

        public void Build(
            IMessageFilterStrategy filteringStrategy, 
            EndpointAddress fromAddress, 
            EndpointAddress recieverAddress)
        {
            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(filteringStrategy))
                .Pump()
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new MessageAddresser(fromAddress, recieverAddress))
                .ToEndPoint(this.messageSender);
        }
    }
}