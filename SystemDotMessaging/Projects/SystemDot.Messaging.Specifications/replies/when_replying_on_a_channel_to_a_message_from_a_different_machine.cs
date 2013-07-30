using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.transport.http;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_on_a_channel_to_a_message_from_a_different_machine : WithServerConfigurationSubject
    {
        const string ReceiverChannelName = "Test";
        const string ReceiverServer = "ReceiverServer";
        const string SenderChannelName = "TestSender";
        const string SenderMachine = "SenderMachine";

        const Int64 Reply = 1;
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .OpenChannel(ReceiverChannelName).ForRequestReplyRecieving()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(SenderChannelName)
                .SetFromServer("SenderServer")
                .SetFromMachine(SenderMachine)
                .SetToChannel(ReceiverChannelName)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced();

            SendMessagesToServer(messagePayload);

            WebRequestor.RequestsMade.Clear();
        };
        
        Because of = () => Bus.Reply(Reply);

        It should_reply_with_a_message = () =>  WebRequestor.DeserialiseSingleRequest<MessagePayload>().ShouldNotBeNull();
    }
}