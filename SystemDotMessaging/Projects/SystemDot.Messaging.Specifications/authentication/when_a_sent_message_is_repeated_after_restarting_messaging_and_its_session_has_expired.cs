using System;
using System.Linq;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Packaging;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_sent_message_is_repeated_after_restarting_messaging_and_its_session_has_expired
        : WithHttpServerConfigurationSubject
    {
        const string SenderServer = "SenderServer";
        const string ReceiverServer = "ReceiverServer";
        const string Channel = "Channel";
        const int Message = 1;

        static ChangeStore changeStore;
        static AuthenticationSession session;

        Establish context = () =>
        {
            changeStore = new InMemoryChangeStore();
            ConfigureAndRegister(changeStore);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(SenderServer)
                        .AuthenticateToServer(ReceiverServer).WithRequest<TestAuthenticationRequest>()
                        .OpenChannel(Channel)
                            .ForPointToPointSendingTo(Channel + "@" + ReceiverServer)
                            .WithDurability()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .Initialise();

            var authenticationResponse = new MessagePayload()
                .SetAuthenticationRequestChannels()
                .SetMessageBody(new TestAuthenticationRequest())
                .SetFromServer(ReceiverServer)
                .SetToServer(SenderServer)
                .SetAuthenticationSession();

            authenticationResponse.GetAuthenticationSession().ExpiresOn = SystemTime.GetCurrentDate().AddHours(1);
            WebRequestor.AddMessages(authenticationResponse);

            Bus.SendDirect(new TestAuthenticationRequest(), new TestMessageHandler<TestAuthenticationResponse>(), e => { });

            Bus.Send(Message);

            Reset();
            ReInitialise();

            ConfigureAndRegister(changeStore);
            SystemTime.AdvanceTime(TimeSpan.FromDays(1));

            WebRequestor.RequestsMade.Clear();
        };

        Because of = () => 
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer(SenderServer)
                        .AuthenticateToServer(ReceiverServer).WithRequest<TestAuthenticationRequest>()
                        .OpenChannel(Channel)
                            .ForPointToPointSendingTo(Channel + "@" + ReceiverServer)
                            .WithDurability()
                    .RegisterHandlers(r => r.RegisterHandler(new TestReplyMessageHandler<TestAuthenticationRequest, TestAuthenticationResponse>()))
                .Initialise();

        It should_not_send_the_message = () => WebRequestor.RequestsMade.ShouldBeEmpty();
    }
}