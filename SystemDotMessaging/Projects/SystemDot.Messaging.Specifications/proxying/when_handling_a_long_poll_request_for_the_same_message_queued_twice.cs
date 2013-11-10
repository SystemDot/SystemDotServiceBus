using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.proxying
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_long_poll_request_for_the_same_message_queued_twice : WithHttpServerConfigurationSubject
    {
        static MessagePayload longPollRequest;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAProxyFor("RemoteServerName")
                .Initialise();

            EndpointAddress address = BuildAddress("Address2", "TestServer");
            
            var sentMessageInQueue1 = new MessagePayload();
            sentMessageInQueue1.SetToAddress(address);
            SendMessagesToServer(sentMessageInQueue1);

            var sentMessageInQueue2 = new MessagePayload { Id = sentMessageInQueue1.Id };
            sentMessageInQueue2.SetToAddress(address);
            SendMessagesToServer(sentMessageInQueue2);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(address.Server);
        };

        Because of = () => returnedMessages = SendMessagesToServer(longPollRequest);

        It should_only_return_the_first_copy_of_the_message = () => returnedMessages.Count().ShouldEqual(1);
    }
}