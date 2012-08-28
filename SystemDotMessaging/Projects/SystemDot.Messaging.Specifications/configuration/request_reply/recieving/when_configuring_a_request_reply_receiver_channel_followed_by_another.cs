using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.recieving
{
    [Subject("Request reply configuration")] 
    public class when_configuring_a_request_reply_receiver_channel_followed_by_another : WithRecieverSubject
    {
        const string Channel2Name = "TestChannel2";

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("TestChannel1").ForRequestReplyRecieving()
                .OpenChannel(Channel2Name).ForRequestReplyRecieving()
            .Initialise();

        It should_register_the_listening_address_of_the_second_channel_with_the_message_reciever = () =>
            The<IMessageReciever>().WasToldTo(r => 
                r.StartPolling(GetEndpointAddress(Channel2Name, The<IMachineIdentifier>().GetMachineName())));
    }
}