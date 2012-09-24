using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    [Subject("Message distribution")]
    public class when_subscribing_with_the_same_subscriber_key_twice 
        : WithSubject<Publisher>
    {
        static EndpointAddress address;
        static Pipe<MessagePayload> subscriber1;
        static Pipe<MessagePayload> subscriber2;
        static MessagePayload message;
        static MessagePayload secondMessageDistributed;
        
        Establish context = () =>
        {
            Configure<MessagePayloadCopier>(new MessagePayloadCopier(new PlatformAgnosticSerialiser()));
            message = new MessagePayload();
            
            subscriber1 = new Pipe<MessagePayload>();
            subscriber1.MessageProcessed += payload => { };
            subscriber2 = new Pipe<MessagePayload>();
            subscriber2.MessageProcessed += payload => secondMessageDistributed = payload;

            address = new EndpointAddress("TestAddress", "TestServer");
            Subject.Subscribe(address, subscriber1);
            Subject.Subscribe(address, subscriber2);

            Subject.MessageProcessed += _ => { };
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_distribute_the_message_to_the_second_subscriber = () => 
            secondMessageDistributed.ShouldBeNull();
    }
}