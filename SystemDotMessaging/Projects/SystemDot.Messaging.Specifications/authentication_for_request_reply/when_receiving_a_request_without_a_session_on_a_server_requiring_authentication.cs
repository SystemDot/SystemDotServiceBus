using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_without_a_session_on_a_server_requiring_authentication : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const long Message = 1;

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();

            var authenticationRequest = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(SenderServer)
                .SetToServer(ReceiverServer);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(ReceiverServer)
                    .RequiresAuthentication()
                        .AcceptsRequest<TestAuthenticationRequest>()
                        .AuthenticatesOnReply<TestAuthenticationResponse>()
                    .OpenChannel(ReceiverChannel)
                        .ForRequestReplyReceiving()
                            .OnException().ContinueProcessingMessages()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                    .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            SendMessageToServer(authenticationRequest);

            payload = new MessagePayload()
                .SetMessageBody(Message)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.RequestSend)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(payload);

        It should_not_handle_the_message_in_the_registered_handler = () => handler.HandledMessages.ShouldBeEmpty();

        It should_send_an_exception_occurred_message_for_the_disallowed_message = () => 
            WebRequestor
                .RequestsMade.DeserialiseToPayloads()
                .Last()
                .DeserialiseTo<ExceptionOccured>()
                .Message.ShouldEqual(String.Format("Message {0} was not handled because its session has expired", payload.Id));
    }
}