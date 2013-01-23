using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;

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