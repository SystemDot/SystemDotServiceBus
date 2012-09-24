using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject("Message distribution")]
    public class when_publishing_a_message_to_a_distributor_with_a_registered_subscriber 
        : WithSubject<Distributor>
    {
        static MessagePayload inputMessage;
        static Pipe<MessagePayload> distributionSubscriber;
        static MessagePayload message;
        static List<MessagePayload> processedMessages;

        Establish context = () =>
        {
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            
            processedMessages = new List<MessagePayload>();
            distributionSubscriber = new Pipe<MessagePayload>();
            distributionSubscriber.MessageProcessed += m => processedMessages.Add(m);
            Subject.Subscribe(new object(), distributionSubscriber);

            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("TestAddress", "TestServer"));
            inputMessage = message;
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_an_equivelent_message_to_the_subscriber = () =>
            processedMessages.First().GetToAddress().ShouldEqual(message.GetToAddress());

        It should_copy_the_message_to_the_subscriber = () =>
            processedMessages.First().ShouldNotBeTheSameAs(message);

        
    }
}