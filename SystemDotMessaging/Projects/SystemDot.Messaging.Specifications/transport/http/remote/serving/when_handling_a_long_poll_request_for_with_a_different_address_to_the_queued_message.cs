using System;
using System.Collections.Generic;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Specifications;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_handling_a_long_poll_request_with_a_different_address_to_the_queued_message
        : WithServerConfigurationSubject
    {
        static MessagePayload sentMessageInQueue;
        static MessagePayload longPollRequest;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            ConfigureAndRegister<ISystemTime>(new TestSystemTime(DateTime.Now, TimeSpan.FromSeconds(0)));

            Messaging.Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAProxy("RemoteServerName")
                .Initialise();

            sentMessageInQueue = new MessagePayload();
            sentMessageInQueue.SetToAddress(TestEndpointAddressBuilder.Build("Address1", "TestServer"));
            SendMessagesToServer(sentMessageInQueue);

            longPollRequest = new MessagePayload();
            longPollRequest.SetLongPollRequest(TestEndpointAddressBuilder.Build("Address1", "TestServer1").ServerPath);
        };

        Because of = () => returnedMessages = SendMessagesToServer(longPollRequest);

        It should_not_output_the_message_in_the_response = () => returnedMessages.ShouldBeEmpty();
    }
}