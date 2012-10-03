using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Channels.Repeating;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement_that_is_not_for_the_current_channel 
        : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        
        Establish context = () =>
        {
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));

            message = new MessagePayload();
            message.SetFromAddress(The<EndpointAddress>());
            message.IncreaseAmountSent();
            
            Configure<IMessageCache>(new MessageCache(new TestPersistence(), The<EndpointAddress>()));
            The<IMessageCache>().Cache(message);

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(message.Id);
            acknowledgement.SetToAddress(new EndpointAddress("Channel1", "Server2"));
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_not_remove_the_corresponding_message_from_the_message_store = () => 
            The<IMessageCache>().GetAll().ShouldContain(message);
    }
}