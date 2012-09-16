using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Packaging.Headers;
using SystemDot.Messaging.Messages.Processing.Acknowledgement;
using SystemDot.Messaging.Messages.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.messages.acknowledgement
{
    public class when_handling_a_recieved_an_acknowledgement : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        
        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("Channel", "Server"));
            
            Configure<IMessageStore>(new InMemoryMessageStore());
            The<IMessageStore>().Store(message);

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(message.Id);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_remove_the_corresponding_message_from_the_message_store = () => 
            The<IMessageStore>().GetForChannel(message.GetToAddress()).ShouldNotContain(message);
    }
}