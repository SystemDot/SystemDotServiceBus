using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.remote_serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_long_poll_request_for_messages_with_two_messages_in_the_queue 
        : WithHttpServerConfigurationSubject
    {
        static MessagePayload sentMessageInQueue1;
        static MessagePayload sentMessageInQueue2;
        static MessagePayload longPollRequest;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAProxyFor("RemoteServerName")
                .Initialise();
            
            sentMessageInQueue1 = new MessagePayload();
            sentMessageInQueue1.SetToAddress(BuildAddress("Address2", "TestServer"));

            sentMessageInQueue2 = new MessagePayload();
            sentMessageInQueue2.SetToAddress(BuildAddress("Address2", "TestServer"));

            SendMessagesToServer(sentMessageInQueue1, sentMessageInQueue2);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(BuildAddress("Address2", "TestServer").Server);
        };

        Because of = () => returnedMessages = SendMessagesToServer(longPollRequest);

        It should_put_the_first_message_in_the_response = () =>
           returnedMessages.First().GetToAddress().ShouldEqual(sentMessageInQueue1.GetToAddress());

        It should_put_the_second_message_in_the_response = () =>
            returnedMessages.Last().GetToAddress().ShouldEqual(sentMessageInQueue2.GetToAddress());
    }
}