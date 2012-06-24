using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Channels.Building;
using SystemDot.Messaging.Channels.Messages;
using SystemDot.Messaging.Channels.Messages.Distribution;
using SystemDot.Messaging.Channels.Messages.Processing;
using SystemDot.Messaging.Channels.Messages.Sending;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.PubSub
{
    public class SubscriptionChannelBuilder : ISubscriptionChannelBuilder 
    {
        public IMessageInputter<MessagePayload> Build(SubscriptionSchema toSchema) 
        {
            var subscriberStartPoint = new Pipe<MessagePayload>();

            ChannelBuilder.Build()
                .With(subscriberStartPoint)
                .ToProcessor(new MessageAddresser())
                .ToEndPoint(BuildSender());

            return subscriberStartPoint;
        }

        static MessageSender BuildSender()
        {
            return new MessageSender(
                MessagingEnvironment.GetComponent<IFormatter>(), 
                MessagingEnvironment.GetComponent<IWebRequestor>());
        }
    }
}