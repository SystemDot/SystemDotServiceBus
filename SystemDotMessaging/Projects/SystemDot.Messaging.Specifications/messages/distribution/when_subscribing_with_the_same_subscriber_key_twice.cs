using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Distribution;
using SystemDot.Messaging.Messages.Packaging;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.distribution
{
    [Subject("Message distribution")]
    public class when_subscribing_with_the_same_subscriber_key_twice 
        : WithSubject<Distributor>
    {
        static EndpointAddress address;
        static Pipe<MessagePayload> subscriber1;
        static Pipe<MessagePayload> subscriber2;
        static MessagePayload message;
        static MessagePayload secondMessageDistributed;
        
        Establish context = () =>
        {
            Configure<MessagePayloadCopier>(new MessagePayloadCopier());
            message = new MessagePayload();
            
            subscriber1 = new Pipe<MessagePayload>();
            subscriber1.MessageProcessed += payload => { };
            subscriber2 = new Pipe<MessagePayload>();
            subscriber2.MessageProcessed += payload => secondMessageDistributed = payload;

            address = new EndpointAddress("test@test");
            Subject.Subscribe(address, subscriber1);
            Subject.Subscribe(address, subscriber2);
        };

        Because of = () => Subject.InputMessage(message);

        It should_not_distribute_the_message_to_the_second_subscriber = () => 
            secondMessageDistributed.ShouldBeNull();

    }
}