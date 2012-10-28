using SystemDot.Ioc;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration
{
    public class WithMessageConfigurationSubject : WithConfiguationSubject
    {
        protected static TestMessageSender MessageSender;
        protected static TestMessageReciever MessageReciever;

        Establish context = () =>
        {
            ConfigureAndRegister(TestTaskStarter.Umlimited());
            ConfigureAndRegister<ITaskRepeater>();
            
            MessageReciever = new TestMessageReciever();
            ConfigureAndRegister<IMessageReciever>(MessageReciever);
            
            MessageSender = new TestMessageSender();
            ConfigureAndRegister<IMessageSender>(MessageSender);
        };

        
        protected static MessagePayload CreateRecieveablePayload(
            object message,
            string fromAddress,
            string toAddress,
            PersistenceUseType useType)
        {
            var payload = new MessagePayload();
            payload.SetBody(Resolve<ISerialiser>().Serialise(message));
            payload.SetFromAddress(BuildAddress(fromAddress));
            payload.SetToAddress(BuildAddress(toAddress));
            payload.SetPersistenceId(BuildAddress(fromAddress), useType);

            return payload;
        }

        protected static T Deserialise<T>(byte[] toDeserialise)
        {
            return Resolve<ISerialiser>().Deserialise(toDeserialise).As<T>();
        }

        protected static EndpointAddress BuildAddress(string fromAddress)
        {
            return Resolve<EndpointAddressBuilder>().Build(fromAddress, MessageServer.Local().Name);
        }

        protected static T Resolve<T>() where T : class
        {
            return IocContainerLocator.Locate().Resolve<T>();
        }
    }
}