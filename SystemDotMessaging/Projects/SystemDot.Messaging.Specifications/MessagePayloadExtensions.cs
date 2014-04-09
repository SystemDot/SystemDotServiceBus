using System;
using SystemDot.Http;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Correlation;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Publishing;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using FluentAssertions;

namespace SystemDot.Messaging.Specifications
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeReceivable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string toAddress,
            PersistenceUseType useType)
        {
            payload.SetMessageBody(message);
            payload.SetFromAddress(BuildAddress(fromAddress));
            payload.SetToAddress(BuildAddress(toAddress));
            payload.SetChannelType(useType);

            return payload;
        }

        public static MessagePayload MakeSequencedReceivable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string toAddress,
            PersistenceUseType useType)
        {
            payload.MakeReceivable(message, fromAddress, toAddress, useType);
            payload.Sequenced();

            return payload;
        }
        
        public static MessagePayload MakeSequencedReceivable(
            this MessagePayload payload,
            object message,
            EndpointAddress from,
            EndpointAddress to,
            PersistenceUseType useType)
        {
            payload.SetMessageBody(message);
            payload.SetFromAddress(from);
            payload.SetToAddress(to);
            payload.SetChannelType(useType);
            payload.Sequenced();

            return payload;
        }

        public static MessagePayload SetMessageBody(this MessagePayload payload, object toSet)
        {
            payload.SetBody(Resolve<ISerialiser>().Serialise(toSet));
            return payload;
        }

        public static MessagePayload SetFromChannel(this MessagePayload payload, string toSet)
        {
            payload.SetFromAddress(new EndpointAddress(toSet, MessageServer.None));
            return payload;
        }

        public static MessagePayload SetFromServer(this MessagePayload payload, string toSet)
        {
            payload.GetFromAddress().Server = MessageServer.Named(toSet, ServerAddress.Local);
            return payload;
        }

        public static MessagePayload SetFromServerAddress(this MessagePayload payload, string toSet)
        {
            payload.GetFromAddress().Server.Address = new ServerAddress(toSet, false);
            return payload;
        }

        public static MessagePayload SetFromEndpointAddress(this MessagePayload payload, EndpointAddress toSet)
        {
            payload.SetFromAddress(toSet);
            return payload;
        }

        public static MessagePayload SetToChannel(this MessagePayload payload, string toSet)
        {
            payload.SetToAddress(new EndpointAddress(toSet, MessageServer.None));
            return payload;
        }

        public static MessagePayload SetToServer(this MessagePayload payload, string toSet)
        {
            payload.GetToAddress().Server = MessageServer.Named(toSet, ServerAddress.Local);
            return payload;
        }

        public static MessagePayload SetToEndpointAddress(this MessagePayload payload, EndpointAddress toSet)
        {
            payload.SetToAddress(toSet);
            return payload;
        }

        public static MessagePayload SetToMachineLocal(this MessagePayload payload)
        {
            payload.GetToAddress().Server.MachineName = System.Environment.MachineName;
            return payload;
        }

        public static MessagePayload SetChannelType(this MessagePayload payload, PersistenceUseType toSet)
        {
            payload.SetPersistenceId(payload.GetFromAddress(), toSet);
            payload.SetSourcePersistenceId(payload.GetPersistenceId());
            payload.SetSequenceOriginSetOn(DateTime.Today);
            return payload;
        }

        public static MessagePayload Sequenced(this MessagePayload payload)
        {
            return payload.WithSequenceOf(1);
        }

        public static MessagePayload WithSequenceOf(this MessagePayload payload, int sequence)
        {
            payload.SetFirstSequence(sequence);
            payload.SetSequence(sequence);
            return payload;
        }

        public static MessagePayload SetNewCorrelationId(this MessagePayload payload)
        {
            payload.SetCorrelationId(Guid.NewGuid());
            return payload;
        }

        public static MessagePayload AsSubscriptionRequest(this MessagePayload payload)
        {
            payload.SetSubscriptionRequest(new SubscriptionSchema
            {
                SubscriberAddress = payload.GetFromAddress(),
                RepeatStrategy = new ConstantTimeRepeatStrategy { RepeatEvery = TimeSpan.FromSeconds(10) }
            });

            payload.SetChannelType(PersistenceUseType.SubscriberRequestReceive);
            payload.SetSequence(1);

            return payload;
        }

        public static MessagePayload AsAcknowlegement(this MessagePayload payload, MessagePersistenceId acknowledgementId)
        {
            payload.SetAcknowledgementId(acknowledgementId);
            return payload;
        }

        
        static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        static EndpointAddress BuildAddress(string fromAddress)
        {
            return new EndpointAddress(fromAddress, MessageServer.None);
        }

        public static T DeserialiseTo<T>(this MessagePayload payload)
        {
            return Resolve<ISerialiser>()
                .Deserialise(payload.GetBody())
                .As<T>();
        }

        public static void ShouldHaveCorrectPersistenceId(
            this MessagePayload payload,
            string address,
            PersistenceUseType persistenceUseType)
        {
            payload.ShouldHaveCorrectPersistenceId(BuildAddress(address), persistenceUseType);
        }

        public static void ShouldHaveCorrectPersistenceId(
            this MessagePayload payload,
            EndpointAddress address,
            PersistenceUseType persistenceUseType)
        {
            payload.GetPersistenceId().ShouldBeEquivalentTo(new MessagePersistenceId(payload.Id, address, persistenceUseType));
        }
    }
}