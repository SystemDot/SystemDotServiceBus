using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_repeating_messages_after_receiving_an_acknowledgement_for_a_message_with_no_channels_configured 
        : WithMessageConfigurationSubject
    {
        static Exception exception; 
        static MessagePayload acknowledgement;
        
        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenLocalChannel()
                .Initialise();

            var address = GetEndpointAddress("Test", "Test");

            acknowledgement = new MessagePayload();
            
            acknowledgement.SetAcknowledgementId(
                new MessagePersistenceId(
                    Guid.NewGuid(), 
                    address, 
                    PersistenceUseType.PointToPointReceive));

            acknowledgement.SetToAddress(address);
        };

        Because of = () => exception = Catch.Exception(() => Server.ReceiveMessage(acknowledgement));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}