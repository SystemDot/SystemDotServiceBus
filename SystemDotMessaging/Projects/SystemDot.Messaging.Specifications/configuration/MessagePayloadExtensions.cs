using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using Machine.Specifications;

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

        public static void ShouldHaveCorrectPersistenceId(
            this MessagePayload payload, 
            string address, 
            PersistenceUseType persistenceUseType)
        {
            payload.GetPersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    payload.Id,
                    BuildAddress(address),
                    persistenceUseType));
        }

        static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        static EndpointAddress BuildAddress(string fromAddress)
        {
            return TestEndpointAddressBuilder.Build(fromAddress, MessageServer.Local().Name);
        }
    }
}