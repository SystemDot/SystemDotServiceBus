using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message publishing")]
    public class when_publishing_a_message_with_a_registered_subscriber : WithSubject<Publisher>
    {
        static MessagePayload inputMessage;
        static Pipe<MessagePayload> subscriber;
        static MessagePayload message;
        static MessagePayload processedSubscriberMessage;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            var address = new EndpointAddress("TestAddress", "TestServer");
            
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            
            subscriber = new Pipe<MessagePayload>();
            subscriber.MessageProcessed += m => processedSubscriberMessage = m;
            Subject.Subscribe(new object(), subscriber);

            Subject.MessageProcessed += m => processedMessage = m;

            message = new MessagePayload();
            message.SetToAddress(address);
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_an_equivelent_message_to_the_subscriber = () => 
            processedSubscriberMessage.GetToAddress().ShouldEqual(message.GetToAddress());

        It should_copy_the_message_to_the_subscriber = () => processedSubscriberMessage.ShouldNotBeTheSameAs(message);
        
        It should_process_the_message = () => processedMessage.ShouldBeTheSameAs(message);

    }
}