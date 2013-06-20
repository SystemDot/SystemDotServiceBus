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
            EndpointAddress from,
            EndpointAddress to,
            PersistenceUseType useType)
        {
            payload.SetBody(Resolve<ISerialiser>().Serialise(message));
            payload.SetFromAddress(from);
            payload.SetToAddress(to);
            payload.SetPersistenceId(from, useType);
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
    }
}