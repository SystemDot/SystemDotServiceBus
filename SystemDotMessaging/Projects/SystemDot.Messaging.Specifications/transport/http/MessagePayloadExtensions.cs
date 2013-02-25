using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.http
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeReceiveable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string toAddress,
            string serverInstance,
            string proxyInstance,
            PersistenceUseType useType)
        {
            payload.SetBody(Resolve<ISerialiser>().Serialise(message));
            payload.SetFromAddress(BuildAddress(fromAddress, serverInstance, proxyInstance));
            payload.SetToAddress(BuildAddress(toAddress, serverInstance, proxyInstance));
            payload.SetPersistenceId(BuildAddress(fromAddress, serverInstance, proxyInstance), useType);
            payload.SetSourcePersistenceId(payload.GetPersistenceId());
            
            return payload;
        }

        static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }

        static EndpointAddress BuildAddress(string fromAddress, string serverInstance, string remoteProxyInstance)
        {
            return TestEndpointAddressBuilder.Build(fromAddress, serverInstance, remoteProxyInstance);
        }
    }
}