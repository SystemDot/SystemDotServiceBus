using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        
        Establish context = () =>
        {
            message = new MessagePayload();

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(message.Id);
            
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            acknowledgement.SetToAddress(The<EndpointAddress>());

            Configure<IMessageCache>(new MessageCache(new InMemoryPersistence(), The<EndpointAddress>()));
            The<IMessageCache>().Cache(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_remove_the_corresponding_message_from_the_message_store = () =>
            The<IMessageCache>().GetAll().ShouldNotContain(message);
    }
}