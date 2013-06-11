using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Specifications.transport.http
{
    public static class MessagePayloadExtensions
    {
        public static MessagePayload MakeSequencedReceivable(
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
            payload.SetSequenceOriginSetOn(DateTime.Today);
            payload.SetFirstSequence(1);
            payload.SetSequence(1);
            
            return payload;
        }

        public static MessagePayload MakeSequencedReceivable(
            this MessagePayload payload,
            object message,
            string fromAddress,
            string fromServerName,
            string fromProxyName, 
            string toAddress,
            string toServerName,
            string toProxyName,
            PersistenceUseType useType)
        {
            payload.SetBody(Resolve<ISerialiser>().Serialise(message));
            payload.SetFromAddress(BuildAddress(fromAddress, fromServerName, fromProxyName));
            payload.SetToAddress(BuildAddress(toAddress, toServerName, toProxyName));
            payload.SetPersistenceId(BuildAddress(fromAddress, fromServerName, fromProxyName), useType);
            payload.SetSourcePersistenceId(payload.GetPersistenceId());
            payload.SetSequenceOriginSetOn(DateTime.Today);
            payload.SetFirstSequence(1);
            payload.SetSequence(1);

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