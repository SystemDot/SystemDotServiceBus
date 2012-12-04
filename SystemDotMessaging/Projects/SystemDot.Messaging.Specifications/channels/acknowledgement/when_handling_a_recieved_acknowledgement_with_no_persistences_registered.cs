using System;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement_with_no_persistences_registered : WithSubject<MessageAcknowledgementHandler>
    {
        static Exception exception;
        static MessagePayload acknowledgement;
        
        Establish context = () =>
        {            
            acknowledgement = new MessagePayload();

            acknowledgement.SetAcknowledgementId(
                new MessagePersistenceId(
                    Guid.NewGuid(), 
                    new EndpointAddress("Channel", "Server"), 
                    PersistenceUseType.RequestReceive));
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(acknowledgement));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}