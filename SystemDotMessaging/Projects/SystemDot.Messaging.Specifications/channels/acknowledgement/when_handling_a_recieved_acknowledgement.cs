using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        
        Establish context = () =>
        {
            message = new MessagePayload();
            message.IncreaseAmountSent();
            
            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(message.Id);
            
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            acknowledgement.SetToAddress(The<EndpointAddress>());

            Configure<IMessageCache>(new MessageCache(new TestPersistence(), The<EndpointAddress>()));
            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_remove_the_corresponding_message_from_the_message_store = () =>
            The<IMessageCache>().GetAll().ShouldNotContain(message);
    }
}