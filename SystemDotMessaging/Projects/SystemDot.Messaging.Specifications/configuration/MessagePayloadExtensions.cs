using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Specifications.configuration
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeReceiveable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string toAddress,
            PersistenceUseType useType)
        {
            payload.SetBody(Resolve<ISerialiser>().Serialise(message));
            payload.SetFromAddress(BuildAddress(fromAddress));
            payload.SetToAddress(BuildAddress(toAddress));
            payload.SetPersistenceId(BuildAddress(fromAddress), useType);
            payload.SetSourcePersistenceId(payload.GetPersistenceId());
            payload.IncreaseAmountSent();

            return payload;
        }

        public static MessagePayload MakeSequencedReceivable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string toAddress,
            PersistenceUseType useType)
        {
            payload.MakeReceiveable(message, fromAddress, toAddress, useType);
            payload.SetSequence(1);
            return payload;
        } 
        
        public static T DeserialiseTo<T>(this MessagePayload payload)
        {
            return Resolve<ISerialiser>()
                .Deserialise(payload.GetBody())
                .As<T>();
        }

        static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        static EndpointAddress BuildAddress(string fromAddress)
        {
            return Resolve<EndpointAddressBuilder>()
                .Build(fromAddress, MessageServer.Local().Name);
        }

    }
}