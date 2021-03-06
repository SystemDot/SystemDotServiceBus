using System;
using System.Collections.Generic;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.proxying
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_long_poll_request_with_a_different_address_to_the_queued_message
        : WithHttpServerConfigurationSubject
    {
        static MessagePayload sentMessageInQueue;
        static MessagePayload longPollRequest;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAProxyFor("RemoteServerName")
                .Initialise();
            
            sentMessageInQueue = new MessagePayload();
            sentMessageInQueue.SetToAddress(TestEndpointAddressBuilder.Build("Address1", "TestServer"));
            SendMessageToServer(sentMessageInQueue);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(TestEndpointAddressBuilder.Build("Address1", "TestServer1").Server);

            SystemTime.FromSecondsSpanOverride = TimeSpan.FromSeconds(0);
        };

        Because of = () => returnedMessages = SendMessageToServer(longPollRequest);

        It should_not_output_the_message_in_the_response = () => returnedMessages.ShouldBeEmpty();
    }
}