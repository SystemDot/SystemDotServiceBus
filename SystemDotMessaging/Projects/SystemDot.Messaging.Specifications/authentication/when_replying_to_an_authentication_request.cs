using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_replying_to_an_authentication_request : WithHttpServerConfigurationSubject
    {
       const string ReceiverServer = "ReceiverServer";

        static MessagePayload payload;
        static TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse> handler;
        static IEnumerable<MessagePayload> returnedMessages;

        Establish context = () =>
        {
            handler = new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>();

            payload = new MessagePayload()
                .MakeAuthenticationRequest<TestAuthenticationRequest>()
                .SetFromServer("SenderServer")
                .SetToServer(ReceiverServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();
        };

        Because of = () => returnedMessages = SendMessagesToServer(payload);

        It should_reply_with_the_specified_authentication_response_containing_the_new_authentication_session_in_the_headers = () =>
            returnedMessages.Single().GetAuthenticationSession().Id.ShouldNotEqual(Guid.Empty);
    }
}