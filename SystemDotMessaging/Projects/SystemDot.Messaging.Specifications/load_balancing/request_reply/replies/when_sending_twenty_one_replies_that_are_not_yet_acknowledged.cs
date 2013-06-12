using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.InProcess.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.load_balancing.request_reply.replies
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_twenty_one_replies_that_are_not_yet_acknowledged : WithMessageConfigurationSubject
    {
        static List<int> messages;
        

        Establish context = () =>
        {
            Messaging.Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverAddress")
                .ForRequestReplyRecieving()
                .Initialise();

            Server.ReceiveMessage(new MessagePayload().MakeSequencedReceivable(
                1,
                "SenderAddress",
                "ReceiverAddress",
                PersistenceUseType.RequestSend));

            messages = Enumerable.Range(1, 21).ToList();
        };

        Because of = () => messages.ForEach(m => Bus.Reply(m));

        It should_not_send_the_twenty_first_message = () => Server.SentMessages.ExcludeAcknowledgements().Count.ShouldEqual(20);
    }
}
