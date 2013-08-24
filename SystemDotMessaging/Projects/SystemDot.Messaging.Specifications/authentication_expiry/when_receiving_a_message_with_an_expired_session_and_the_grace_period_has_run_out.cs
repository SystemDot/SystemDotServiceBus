using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.authentication;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication_expiry
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_an_expired_session_and_the_grace_period_has_run_out : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string SenderServer = "SenderServer";
        const string ReceiverChannel = "ReceiverChannel";
        const int GracePeriodInMinutes = 10;
        const int ExpiryInMinutes = 20;

        static MessagePayload payload;
        static TestMessageHandler<long> handler;

        Establish context = () =>
        {
            handler = new TestMessageHandler<long>();
            
            var authenticationRequestPayload = new MessagePayload()
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
                        .Expires(ExpiryPlan.ExpiresAfter(TimeSpan.FromMinutes(ExpiryInMinutes)).WithGracePeriodOf(TimeSpan.FromMinutes(GracePeriodInMinutes)))
                    .OpenChannel(ReceiverChannel)
                        .ForPointToPointReceiving()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                    .RegisterHandlers(r => r.RegisterHandler(handler))
                .Initialise();

            AuthenticationSession session = SendMessagesToServer(authenticationRequestPayload).Single().GetAuthenticationSession();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServer)
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();

            payload.SetAuthenticationSession(session);

            SystemTime.AdvanceTime(TimeSpan.FromMinutes(ExpiryInMinutes));
            SystemTime.AdvanceTime(TimeSpan.FromMinutes(GracePeriodInMinutes));
            SystemTime.AdvanceTime(TimeSpan.FromTicks(1));
        };

        Because of = () => SendMessagesToServer(payload);

        It should_not_handle_the_message_in_the_registered_handler = () => handler.HandledMessages.Count.ShouldEqual(0);
    }
}