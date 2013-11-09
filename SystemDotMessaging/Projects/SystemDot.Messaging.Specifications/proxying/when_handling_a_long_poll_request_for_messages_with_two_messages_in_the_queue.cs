using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.proxying
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

            EndpointAddress address = BuildAddress("Address2", "TestServer");

            sentMessageInQueue1 = new MessagePayload();
            sentMessageInQueue1.SetToAddress(address);
            SendMessagesToServer(sentMessageInQueue1);

            sentMessageInQueue2 = new MessagePayload();
            sentMessageInQueue2.SetToAddress(address);
            SendMessagesToServer(sentMessageInQueue2);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(address.Server);
        };

        Because of = () => returnedMessages = SendMessagesToServer(longPollRequest);

        It should_put_the_first_message_in_the_response = () => returnedMessages.ShouldContain(sentMessageInQueue1);

        It should_put_the_second_message_in_the_response = () => returnedMessages.ShouldContain(sentMessageInQueue2);
    }
}